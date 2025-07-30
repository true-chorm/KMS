using KMS.src.tool;
using System;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using System.Windows.Forms;

namespace KMS.src.core
{
    /// <summary>
    /// Catch the global keyboard and mouse event.
    /// </summary>
    internal static class KMEventHook
    {
        private const string TAG = "GlobalEventListener";

        private const int WH_KEYBOARD_LL = 13; //low-level keyboard event symbol.
        private const int WH_MOUSE_LL = 14; //mouse event as above.

        private static IntPtr pKeyboardHook = IntPtr.Zero; //键盘钩子句柄，通过句柄值来判断是否已注册钩子监听。
        private static IntPtr pMouseHook = IntPtr.Zero; //The hook reference of global mouse event.
        //钩子委托声明
        public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        private static HookProc keyboardHookProc;
        private static HookProc mouseHookProc;

        private static Keyboard_LL_Hook_Data khd;
        private static Mouse_LL_Hook_Data mhd;

        //安装钩子
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr pInstance, int threadID);
        //卸载钩子
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);
        //使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam); //parameter 'hhk' is ignored.
        [DllImport("user32.dll")]
        public static extern int keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtralInfo);


        private static int KeyboardHookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
            {
                //TODO 把异常事件记录下来。写到数据库中。
                return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            }

            khd = (Keyboard_LL_Hook_Data)Marshal.PtrToStructure(lParam, typeof(Keyboard_LL_Hook_Data));
            EventQueue.enqueue(Constants.HookEvent.KEYBOARD_EVENT, (short)wParam.ToInt32(), (short)khd.vkCode, 0, 0);
            if (keyMappingHook(wParam.ToInt32(), khd.vkCode)) return 1;

            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        private static bool lctlPressed = false;

        private static Boolean keyMappingHook(int w, int kc)
        {
            Boolean ret = false;

            if (w == Constants.KeyEvent.WM_KEYDOWN)
            {
                switch (kc)
                {
                    case Constants.TypeNumber.LEFT_CTRL:
                        lctlPressed = true;
                        break;
                    case Constants.TypeNumber.SPACE_BAR:
                        if (lctlPressed)
                        {
                            keybd_event(0xa6, 0, 0, UIntPtr.Zero);
                            keybd_event(0xa6, 0, 2, UIntPtr.Zero);
                            ret = true;
                        }
                        break;
                }
            }
            else if (w == Constants.KeyEvent.WM_KEYUP)
            {
                switch (kc)
                {
                    case Constants.TypeNumber.LEFT_CTRL:
                        lctlPressed = false;
                        break;
                }
            }

            return ret;
        }

        private static int MouseHookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
            {
                return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            }
            else
            {
                //Ignore the mouse-move event.
                if (wParam.ToInt32() != Constants.MouseEvent.WM_MOUSEMOVE)
                {
                    mhd = (Mouse_LL_Hook_Data)Marshal.PtrToStructure(lParam, typeof(Mouse_LL_Hook_Data));
                    EventQueue.enqueue(Constants.HookEvent.MOUSE_EVENT, (short)wParam.ToInt32(), (short)(mhd.mouseData >> 16),
                        (short)(mhd.yx & 0xffffffff), (short)(mhd.yx >> 32));
                }
            }

            return 0;
        }

        internal static bool InsertHook()
        {
            bool iRet;
            iRet = InsertKeyboardHook();
            if (!iRet)
            {
                return false;
            }

            iRet = InsertMouseHook();
            if (!iRet)
            {
                removeKeyboardHook();
                return false;
            }

            return true;
        }

        //安装钩子方法
        private static bool InsertKeyboardHook()
        {
            Logger.v(TAG, "InsertKeyboardHook");
            if (pKeyboardHook == IntPtr.Zero)//不存在钩子时
            {
                //创建钩子
                keyboardHookProc = KeyboardHookCallback;
                pKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL,
                    keyboardHookProc,
                    /*GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName)*/(IntPtr)0,
                    0);

                if (pKeyboardHook == IntPtr.Zero)//如果安装钩子失败
                {
                    Logger.v(TAG, "hook insert failed");
                    removeKeyboardHook();
                    return false;
                }
            }
            else
            {
                Logger.v(TAG, "Hook already working");
            }

            return true;
        }

        private static bool InsertMouseHook()
        {
            Logger.v(TAG, "InsertMouseHook()");
            if (pMouseHook == IntPtr.Zero)
            {
                mouseHookProc = MouseHookCallback;
                pMouseHook = SetWindowsHookEx(WH_MOUSE_LL,
                    mouseHookProc,
                    (IntPtr)0,
                    0);

                if (pMouseHook == IntPtr.Zero)
                {
                    Logger.v(TAG, "Mouse hook insert failed");
                    removeMouseHook();
                    return false;
                }
            }
            else
            {
                Logger.v(TAG, "The mouse hook already working");
            }

            return true;
        }

        internal static bool RemoveHook()
        {
            bool iRet;
            iRet = removeKeyboardHook();
            if (iRet)
            {
                iRet = removeMouseHook();
            }

            return iRet;
        }

        private static bool removeKeyboardHook()
        {
            Logger.v(TAG, "RemoveKeyboardHook()");
            if (pKeyboardHook != IntPtr.Zero)
            {
                if (UnhookWindowsHookEx(pKeyboardHook))
                {
                    pKeyboardHook = IntPtr.Zero;
                }
                else
                {
                    Logger.v(TAG, "keyboard hook remove failed");
                    return false;
                }
            }

            return true;
        }

        private static bool removeMouseHook()
        {
            Logger.v(TAG, "RemoveMouseHook()");
            if (pMouseHook != IntPtr.Zero)
            {
                if (UnhookWindowsHookEx(pMouseHook))
                {
                    pMouseHook = IntPtr.Zero;
                }
                else
                {
                    Logger.v(TAG, "mouse hook remove failed");
                    return false;
                }
            }

            return true;
        }

        internal struct Keyboard_LL_Hook_Data
        {
            internal readonly int vkCode;
            internal readonly int scanCode;
            internal readonly int flags;
            internal readonly int time;
            internal readonly IntPtr extraInfo;
        }

        internal struct Mouse_LL_Hook_Data
        {
            internal long yx;
            internal readonly int mouseData;
            internal readonly uint flags;
            internal readonly uint time;
            internal readonly IntPtr dwExtraInfo;
        }
    }
}
