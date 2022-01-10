using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KMS.src.tool
{
    /// <summary>
    /// 为避免跨日时间重置带来的影响，将整个程序的时间统一标准。
    /// 所有需要参照系统时间来访问数据库的情况都从这里取时间值。
    /// 2021-01-07 10:38
    /// </summary>
    static class TimeManager
    {
        internal static DateTime TimeUsing
        {
            get;
            set;
        }
    }
}
