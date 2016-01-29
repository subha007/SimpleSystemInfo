using System;

namespace WinSysInfo.PEView.Converter
{
    public class DateTimeConverter
    {
        /// <summary>
        /// An additional conversion to local time was required
        /// </summary>
        /// <param name="time_tvalue32"></param>
        /// <returns></returns>
        public static DateTime ConvertFrom(uint time_tvalue32)
        {
            return new System.DateTime(1970, 1, 1).ToLocalTime().AddSeconds(time_tvalue32);
        }
    }
}
