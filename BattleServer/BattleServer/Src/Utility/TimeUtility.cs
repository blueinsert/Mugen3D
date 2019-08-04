using System;
using System.Collections;
using System.Collections.Generic;

namespace BattleServer
{
    public class TimeUtility
    {
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}