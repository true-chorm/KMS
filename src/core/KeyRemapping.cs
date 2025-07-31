using KMS.src.tool;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KMS.src.core
{
    internal static class KeyRemapping
    {
        private static Dictionary<int/*keycode*/, Evt> keyStack = new Dictionary<int, Evt>();

        [DllImport("user32.dll")]
        public static extern int keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtralInfo);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        /**
         return true will intercept current key-event to spread in the system
         */
        internal static bool RemappingCheck(int w, int kc)
        {
            // 1. Env prepare
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // 2. Remove the outdated Evt
            ClearOutdatedEles(now);

            // 3. check it
            bool ret = false;
            if (w == Constants.KeyEvent.WM_KEYDOWN)
            {
                ret = KeyDown(kc, now);
            }
            else if (w == Constants.KeyEvent.WM_KEYUP)
            {
                ret = KeyUp(kc);
            }

            return ret;
        }

        internal static void mouseKeyEvtOccured()
        {
            if (keyStack.Count == 1 && keyStack.ContainsKey(Constants.TypeNumber.LEFT_CTRL))
            {
                keybd_event(Constants.TypeNumber.LEFT_CTRL, 0, 0, UIntPtr.Zero); // press the left ctrl
                keyStack.Remove(Constants.TypeNumber.LEFT_CTRL);
            }
        }

        private static void ClearOutdatedEles(long now)
        {
            List<int> forRms = new List<int>();

            foreach (var entry in keyStack)
            {
                if (entry.Value.epoch > now || now - entry.Value.epoch > 1)
                {
                    forRms.Add(entry.Key);
                }
            }

            foreach (var key in forRms)
            {
                keyStack.Remove(key);
            }
        }

        private static bool KeyDown(int kc, long now)
        {
            bool ret = false;

            switch (kc)
            {
                case Constants.TypeNumber.LEFT_CTRL:
                    if (keyStack.Count == 0)
                    {
                        keyStack.Add(Constants.TypeNumber.LEFT_CTRL, new Evt { keyevent = Constants.KeyEvent.WM_KEYDOWN, epoch = now });
                        ret = true;
                    }
                    break;
                case Constants.TypeNumber.SPACE_BAR:
                    if (keyStack.Count == 1 && keyStack.TryGetValue(Constants.TypeNumber.LEFT_CTRL, out Evt evt1))
                    {
                        // Hit it! ctrl + space pressed
                        keyStack.Add(Constants.TypeNumber.SPACE_BAR, new Evt { keyevent = Constants.KeyEvent.WM_KEYDOWN, epoch = now });
                        mouse_event(0x080, 0, 0, 0x01, UIntPtr.Zero); // press
                        mouse_event(0x100, 0, 0, 0x01, UIntPtr.Zero); // release
                        evt1.epoch = now;
                        ret = true;
                    }
                    else if (keyStack.Count == 2 && keyStack.TryGetValue(Constants.TypeNumber.LEFT_CTRL, out Evt evt2) && keyStack.TryGetValue(Constants.TypeNumber.SPACE_BAR, out Evt evt3))
                    {
                        evt2.epoch = now;
                        evt3.epoch = now;
                        ret = true;
                    }
                    break;
                default:
                    if (keyStack.Count == 1 && keyStack.ContainsKey(Constants.TypeNumber.LEFT_CTRL))
                    {
                        // ctrl + others pressed
                        keybd_event(Constants.TypeNumber.LEFT_CTRL, 0, 0, UIntPtr.Zero); // press the left ctrl
                        keyStack.Remove(Constants.TypeNumber.LEFT_CTRL);
                    }
                    break;
            }

            return ret;
        }

        private static bool KeyUp(int kc)
        {
            bool ret = false;

            switch (kc)
            {
                case Constants.TypeNumber.LEFT_CTRL:
                    if (keyStack.Count == 1 && keyStack.ContainsKey(Constants.TypeNumber.LEFT_CTRL))
                    {
                        // single ctrl key press
                        keybd_event(Constants.TypeNumber.LEFT_CTRL, 0, 0, UIntPtr.Zero); // press the left ctrl
                        keyStack.Remove(Constants.TypeNumber.LEFT_CTRL);
                    }
                    else
                    {
                        ret = keyStack.Remove(Constants.TypeNumber.LEFT_CTRL);
                    }
                    break;
                case Constants.TypeNumber.SPACE_BAR:
                    ret = keyStack.Remove(Constants.TypeNumber.SPACE_BAR);
                    break;
            }

            return ret;
        }

        private struct Evt
        {
            internal short keyevent;
            internal long epoch;
        }
    }
}
