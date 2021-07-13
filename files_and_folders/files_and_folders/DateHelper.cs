using System;
using System.Collections.Generic;
using System.Text;

namespace files_and_folders
{
    public static class DateHelper
    {
        public static DateTime? EpochToDate(this string unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0,0).AddSeconds(Convert.ToDouble(unixTimeStamp));
        }
        public static string DateToEpoch(this DateTime thsTime)
        {
            TimeSpan t = thsTime - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            return secondsSinceEpoch.ToString();
        }
    }
}
