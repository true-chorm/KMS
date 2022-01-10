using System;
using System.Collections.Generic;
using System.Text;

namespace KMS.src.tool
{
    static class Timer
    {
        internal delegate void TimerCallback(object obj);

        private static System.Threading.Timer timer;
        private static List<TimerCallback> timerCallbackList; //为了保证顺序，不能用Dictionary。

        internal static void StartTimer()
        {
            if (timer is null)
            {
                timer = new System.Threading.Timer(TickToc, null, 50000, 60000);
            }

            if (timerCallbackList is null)
            {
                timerCallbackList = new List<TimerCallback>();
            }
            else
            {
                timerCallbackList.Clear();
            }
        }

        internal static void RegisterTimerCallback(TimerCallback cb)
        {
            timerCallbackList.Add(cb);
        }

        private static void TickToc(object state)
        {
            foreach (TimerCallback cb in timerCallbackList)
            {
                cb(state);
            }
        }

        internal static void DestroyTimer()
        {
            timer.Dispose();
            timerCallbackList.Clear();
        }
    }
}
