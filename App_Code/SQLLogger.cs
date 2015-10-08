using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SQLLogger
/// </summary>
namespace BTS.Util
{
    public class SQLLogger : Logger
    {

        public const uint ONLY_CHANGE = INFO;
        public const uint ALL = 100;


        public static string GetSeverityString(uint severity)
        {
            if (severity >= ALL) return "[ALL]";
            if (severity >= ONLY_CHANGE) return "[ONLY_CHANGE]";
            return "[ONLY_CHANGE]";
        }

        public static uint GetSeverityByString(string level)
        {
            if (level == null) return ALL;
            if (level.Equals("ONLY_CHANGE")) return ONLY_CHANGE;
            if (level.Equals("ALL")) return ALL;
            return ALL;
        }

    }
}
