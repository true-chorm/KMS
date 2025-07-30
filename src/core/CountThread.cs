using KMS.src.tool;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System;
using KMS.src.db;
using System.Windows.Input;

namespace KMS.src.core
{
    /// <summary>
    /// created at 2020-12-30 19:54
    /// </summary>
    static class CountThread
    {
        private const string TAG = "CountThread";

        internal static bool CanThreadRun;

        private static EventQueue.KMEvent[] events;
        private static int eventAmount;

        private const byte MAX_KEY_CHAIN_COUNT = 3;
        private static byte keyChainCount;
        private static byte fkey; //function key,indicate combo key typed.
        private static NormalKey[] keyChain = new NormalKey[MAX_KEY_CHAIN_COUNT]; //最多支持三连普通按键。
        private static NormalKey[] keyChainForDownUp = new NormalKey[MAX_KEY_CHAIN_COUNT];

        private static short msWheelCounter; //同一方向的滚轮滚动聚合成一条记录以节约性能。负值表示向后滚动，正值表示向前滚动。
        private static DateTime msWheelTime; //与 msWheelCounter 配合使用，记录最后一次滚动事件的时间。

        private static FunKey lctrl;
        private static FunKey rctrl;
        private static FunKey lshift;
        private static FunKey rshift;
        private static FunKey lalt;
        private static FunKey ralt;
        private static FunKey lwin;
        private static FunKey rwin;

        internal static void ThreadProc()
        {
            Logger.v(TAG, "ThreadProc() run");
            events = new EventQueue.KMEvent[EventQueue.MAX_EVENT_AMOUNT];
            CanThreadRun = true;
            while (CanThreadRun)
            {
                Statistic();
                Thread.Sleep(5000);
            }
            Logger.v(TAG, "ThreadProc() end");
        }

        private static void Statistic()
        {
            //Should only be call by sub-thread.
            eventAmount = 0; //Must do.
            EventQueue.Migrate(ref events, ref eventAmount);
            Logger.v(TAG, "event amount:" + eventAmount + ", keyChainCount:" + keyChainCount);

            for (int idx = 0; idx < eventAmount; idx++)
            {
                if (events[idx].type == Constants.HookEvent.KEYBOARD_EVENT)
                {
                    ParseKeyboardEvent(events[idx].eventCode, events[idx].keyCode, events[idx].time);
                }
                else if (events[idx].type == Constants.HookEvent.MOUSE_EVENT)
                {
                    ParseMouseEvent(events[idx].eventCode, events[idx].keyCode, events[idx].x, events[idx].y, events[idx].time);
                }
                else
                {
                    Logger.v(TAG, "Invalid event type");
                }
            }
        }

        private static void ParseKeyboardEvent(short eventCode, short keyCode, DateTime time)
        {
            switch (eventCode)
            {
                case Constants.KeyEvent.WM_KEYDOWN:
                case Constants.KeyEvent.WM_SYSKEYDOWN:
                    KeyDownProcess(keyCode, time);
                    break;
                case Constants.KeyEvent.WM_SYSKEYUP:
                case Constants.KeyEvent.WM_KEYUP:
                    if (msWheelCounter != 0)
                    {
                        if (msWheelCounter < 0)
                        {
                            StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD, msWheelCounter * -1, 0, 0, msWheelTime);
                        }
                        else
                        {
                            StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_WHEEL_FORWARD, msWheelCounter, 0, 0, msWheelTime);
                        }
                        msWheelCounter = 0;
                    }
                    KeyUpProcess(keyCode, time);
                    break;
                default:
                    break;
            }
        }

        private static void ParseMouseEvent(short eventCode, short mouseData, short x, short y, DateTime time)
        {
            if (eventCode != Constants.MouseEvent.WM_MOUSEWHEEL && msWheelCounter != 0)
            {
                if (msWheelCounter < 0)
                {
                    StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD, msWheelCounter * -1, 0, 0, msWheelTime);
                }
                else
                {
                    StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_WHEEL_FORWARD, msWheelCounter, 0, 0, msWheelTime);
                }
                msWheelCounter = 0;
            }

            switch (eventCode)
            {
                case Constants.MouseEvent.WM_LBUTTONDOWN:
                    StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_LEFT_BTN, 0, x, y, time);
                    break;
                case Constants.MouseEvent.WM_RBUTTONDOWN:
                    StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_RIGHT_BTN, 0, x, y, time);
                    break;
                case Constants.MouseEvent.WM_MOUSEWHEEL:
                    if (mouseData == -120) //后向滚动
                    {
                        if (msWheelCounter > 0)
                        {
                            //上一次仍是前向滚动，本次突然变成后向滚动，需要将前面的前向滚动事件记录下来。
                            StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_WHEEL_FORWARD, msWheelCounter, 0, 0, msWheelTime);
                            msWheelCounter = -1;
                        }
                        else
                        {
                            msWheelCounter--;
                            msWheelTime = time;
                        }
                    }
                    else if (mouseData == 120) //前向滚动
                    {
                        if (msWheelCounter < 0)
                        {
                            //上一次仍是后向滚动，本次突然变成前向滚动，需要将前面的后向滚动事件记录下来。
                            StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_WHEEL_BACKWARD, msWheelCounter * -1, 0, 0, msWheelTime);
                            msWheelCounter = 1;
                        }
                        else
                        {
                            msWheelCounter++;
                            msWheelTime = time;
                        }
                    }
                    break;
                case Constants.MouseEvent.WM_MOUSESIDEDOWN:
                    if (mouseData == Constants.MouseDataHighOrder.SIDE_BACKWARD)
                    {
                        StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_SIDE_BACKWARD, 0, 0, 0, time);
                    }
                    else if (mouseData == Constants.MouseDataHighOrder.SIDE_FORWARD)
                    {
                        StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_SIDE_FORWARD, 0, 0, 0, time);
                    }
                    break;
                case Constants.MouseEvent.WM_WHEEL_CLK_DOWN:
                    StatisticManager.GetInstance.MouseEventHappen(Constants.TypeNumber.MOUSE_WHEEL_CLICK, 0, 0, 0, time);
                    break;
            }
        }

        private static void KeyDownProcess(short keycode, DateTime time)
        {
            //清除普通按键过期事件记录。
            if (keyChainCount > 0)
            {
                byte count = 0;
                for (byte i = 0; i < keyChainCount; i++)
                {
                    if (!isTimeout(keyChain[i].time, time))
                    {
                        keyChainForDownUp[count].keycode = keyChain[i].keycode;
                        keyChainForDownUp[count].time = keyChain[i].time;

                        count++;
                    }
                }
                for (byte i = 0; i < count; i++)
                {
                    keyChain[i].keycode = keyChainForDownUp[i].keycode;
                    keyChain[i].time = keyChainForDownUp[i].time;
                }
                keyChainCount = count;
            }

            //清除功能按键过期事件记录。
            if (lctrl.IsPress && isTimeout(lctrl.time, time))
            {
                lctrl.IsPress = false;
            }

            if (rctrl.IsPress && isTimeout(rctrl.time, time))
            {
                rctrl.IsPress = false;
            }

            if (lshift.IsPress && isTimeout(lshift.time, time))
            {
                lshift.IsPress = false;
            }

            if (rshift.IsPress && isTimeout(rshift.time, time))
            {
                rshift.IsPress = false;
            }

            if (lalt.IsPress && isTimeout(lalt.time, time))
            {
                lalt.IsPress = false;
            }

            if (ralt.IsPress && isTimeout(ralt.time, time))
            {
                ralt.IsPress = false;
            }

            if (lwin.IsPress && isTimeout(lwin.time, time))
            {
                lwin.IsPress = false;
            }

            if (rwin.IsPress && isTimeout(rwin.time, time))
            {
                rwin.IsPress = false;
            }

            if (keyChainCount >= MAX_KEY_CHAIN_COUNT)
                return;//忽略事件，不予记录。

            switch (keycode)
            {
                case Constants.TypeNumber.LEFT_CTRL:
                    if (lctrl.IsPress)
                    {
                        lctrl.time = time;
                        return;
                    }
                    else
                    {
                        lctrl.IsPress = true;
                        lctrl.time = time;
                    }
                    break;
                case Constants.TypeNumber.RIGHT_CTRL:
                    if (rctrl.IsPress)
                    {
                        rctrl.time = time;
                        return;
                    }
                    else
                    {
                        rctrl.IsPress = true;
                        rctrl.time = time;
                    }
                    break;
                case Constants.TypeNumber.LEFT_SHIFT:
                    if (lshift.IsPress)
                    {
                        lshift.time = time;
                        return;
                    }
                    else
                    {
                        lshift.IsPress = true;
                        lshift.time = time;
                    }
                    break;
                case Constants.TypeNumber.RIGHT_SHIFT:
                    if (rshift.IsPress)
                    {
                        rshift.time = time;
                        return;
                    }
                    else
                    {
                        rshift.IsPress = true;
                        rshift.time = time;
                    }
                    break;
                case Constants.TypeNumber.LEFT_ALT:
                    if (lalt.IsPress)
                    {
                        lalt.time = time;
                        return;
                    }
                    else
                    {
                        lalt.IsPress = true;
                        lalt.time = time;
                    }
                    break;
                case Constants.TypeNumber.RIGHT_ALT:
                    if (ralt.IsPress)
                    {
                        ralt.time = time;
                        return;
                    }
                    else
                    {
                        ralt.IsPress = true;
                        ralt.time = time;
                    }
                    break;
                case Constants.TypeNumber.LEFT_WIN:
                    if (lwin.IsPress)
                    {
                        lwin.time = time;
                        return;
                    }
                    else
                    {
                        lwin.IsPress = true;
                        lwin.time = time;
                    }
                    break;
                case Constants.TypeNumber.RIGHT_WIN:
                    if (rwin.IsPress)
                    {
                        rwin.time = time;
                        return;
                    }
                    else
                    {
                        rwin.IsPress = true;
                        rwin.time = time;
                    }
                    break;
                default:
                    //重复的按键不入链
                    if (keyChainCount > 0)
                    {
                        for (byte i = 0; i < keyChainCount; i++)
                        {
                            if (keyChain[i].keycode == keycode)
                            {
                                keyChain[i].time = time;
                                return;
                            }
                        }
                    }

                    keyChain[keyChainCount].keycode = keycode;
                    keyChain[keyChainCount].time = time;
                    keyChainCount++;
                    break;
            }
        }

        private static void KeyUpProcess(short keycode, DateTime time)
        {
            fkey = 0;
            if (keyChainCount > 0)
            {
                //Remove the key event from 'keyChain'.
                byte count = 0;
                byte i;
                for (i = 0; i < keyChainCount; i++)
                {
                    if (keyChain[i].keycode == keycode)
                    {
                        if (lctrl.IsPress)
                        {
                            fkey |= 1;
                            lctrl.IsPress = false;
                        }
                        if (rctrl.IsPress)
                        {
                            fkey |= 2;
                            rctrl.IsPress = false;
                        }
                        if (lshift.IsPress)
                        {
                            fkey |= 4;
                            lshift.IsPress = false;
                        }
                        if (rshift.IsPress)
                        {
                            fkey |= 8;
                            rshift.IsPress = false;
                        }
                        if (lalt.IsPress)
                        {
                            fkey |= 16;
                            lalt.IsPress = false;
                        }
                        if (ralt.IsPress)
                        {
                            fkey |= 32;
                            ralt.IsPress = false;
                        }
                        if (lwin.IsPress)
                        {
                            fkey |= 64;
                            lwin.IsPress = false;
                        }
                        if (rwin.IsPress)
                        {
                            fkey |= 128;
                            rwin.IsPress = false;
                        }

                        continue;
                    }
                    else
                    {
                        keyChainForDownUp[count].keycode = keyChain[i].keycode;
                        keyChainForDownUp[count++].time = keyChain[i].time;
                    }
                }

                for (i = 0; i < count; i++)
                {
                    keyChain[i].keycode = keyChainForDownUp[i].keycode;
                    keyChain[i].time = keyChainForDownUp[i].time;
                }
                keyChainCount = count;
            }
            else
            {
                if (lctrl.IsPress)
                {
                    if (keycode != Constants.TypeNumber.LEFT_CTRL)
                        fkey |= 1;
                    lctrl.IsPress = false;
                }
                if (rctrl.IsPress)
                {
                    if (keycode != Constants.TypeNumber.RIGHT_CTRL)
                        fkey |= 2;
                    rctrl.IsPress = false;
                }
                if (lshift.IsPress)
                {
                    if (keycode != Constants.TypeNumber.LEFT_SHIFT)
                        fkey |= 4;
                    lshift.IsPress = false;
                }
                if (rshift.IsPress)
                {
                    if (keycode != Constants.TypeNumber.RIGHT_SHIFT)
                        fkey |= 8;
                    rshift.IsPress = false;
                }
                if (lalt.IsPress)
                {
                    if (keycode != Constants.TypeNumber.LEFT_ALT)
                        fkey |= 16;
                    lalt.IsPress = false;
                }
                if (ralt.IsPress)
                {
                    if (keycode != Constants.TypeNumber.RIGHT_ALT)
                        fkey |= 32;
                    ralt.IsPress = false;
                }
                if (lwin.IsPress)
                {
                    if (keycode != Constants.TypeNumber.LEFT_WIN)
                        fkey |= 64;
                    lwin.IsPress = false;
                }
                if (rwin.IsPress)
                {
                    if (keycode != Constants.TypeNumber.RIGHT_WIN)
                        fkey |= 128;
                    rwin.IsPress = false;
                }
            }

            StatisticManager.GetInstance.KeyboardEventHappen(keycode, fkey, time);
        }

        /// <summary>
        /// 超时时长限制：5秒。
        /// </summary>
        private static bool isTimeout(DateTime oldTime, DateTime curTime)
        {
            double ts = (curTime - oldTime).TotalSeconds;
            return ts < -5 || ts > 5;
        }

        /// <summary>
        /// 用于记录功能键按下事件。
        /// </summary>
        private struct FunKey
        {
            internal bool IsPress;
            internal DateTime time;
        }

        private struct NormalKey
        {
            internal short keycode;
            internal DateTime time;
        }
    }
}
