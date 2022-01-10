using System;

namespace KMS.src.tool
{
    internal static class Logger
    {
        private static DateTime time;

        internal static void v(string tag, string msg)
        {
            /*
            time = DateTime.Now;
            System.Diagnostics.Debug.Write(time.Year + "-" + time.Month + "-" + time.Day + " " + time.Hour + ":" + time.Minute + ":" + time.Second + "." + time.Millisecond + "/");
            System.Diagnostics.Debug.WriteLine("{0}: {1}", tag, msg);
            */
        }
    }
}
