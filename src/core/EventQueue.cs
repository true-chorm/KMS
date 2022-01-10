using KMS.src.tool;
using System;
using System.Threading;

namespace KMS.src.core
{
    /// <summary>
    /// Manage the keyboard and mouse event, storage and migrate it
    /// </summary>
    static class EventQueue
    {
        private const int EQ_STEP_IDLE = 0;
        private const int EQ_STEP_READY = 1;
        private const int EQ_STEP_ENQUEUING = 2;
        private const int MG_STEP_READY = 3;
        private const int MG_STEP_MIGRATING = 4;

        private static int amount;
        private static byte enqueueStep;

        //storage the event.
        internal const int MAX_EVENT_AMOUNT = 500;
        private static KMEvent[] events = new KMEvent[MAX_EVENT_AMOUNT];

        /// <summary>
        /// Can only be call by KMEventHook to storage the original keyboard and mouse event.
        /// </summary>
        internal static void enqueue(byte type, short eventCode, short keyCode, short x, short y)
        {
            int loopCounter = 0;
        BEGIN:
            if (enqueueStep == EQ_STEP_IDLE)
            {
                enqueueStep = EQ_STEP_READY;
                if (enqueueStep == EQ_STEP_READY)
                {
                    enqueueStep = EQ_STEP_ENQUEUING;

                    events[amount].type = type;
                    events[amount].eventCode = eventCode;
                    events[amount].keyCode = keyCode;
                    events[amount].x = x;
                    events[amount].y = y;
                    events[amount].time = DateTime.Now;

                    amount++;

                    enqueueStep = EQ_STEP_IDLE;
                    return;
                }
            }

            //wait for resource release.
            Logger.v("EventQueue", "waiting for record event, counter:" + loopCounter);
            if (loopCounter++ > 2)
                return;

            Thread.Sleep(10);
            goto BEGIN;
        }

        /// <summary>
        /// Can only be call by 'StatisticThread',
        /// copy all the event to another place to process.
        /// </summary>
        internal static void Migrate(ref KMEvent[] e, ref int amount)
        {
            if (enqueueStep == EQ_STEP_IDLE)
            {
                enqueueStep = MG_STEP_READY;
                if (enqueueStep == MG_STEP_READY)
                {
                    enqueueStep = MG_STEP_MIGRATING;

                    for (int i = 0; i < EventQueue.amount; i++)
                    {
                        e[i].type = events[i].type;
                        e[i].eventCode = events[i].eventCode;
                        e[i].keyCode = events[i].keyCode;
                        e[i].x = events[i].x;
                        e[i].y = events[i].y;
                        e[i].time = events[i].time;
                    }

                    amount = EventQueue.amount;
                    EventQueue.amount = 0;

                    enqueueStep = EQ_STEP_IDLE;
                }
            }
        }

        internal struct KMEvent
        {
            internal byte type; //keyboard event or mouse event
            internal short eventCode; //down or up
            internal short keyCode; //which key
            internal short x; //for mouse event
            internal short y; //for mouse event
            internal DateTime time;
        }
    }
}
