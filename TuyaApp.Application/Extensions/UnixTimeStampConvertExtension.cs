using System;

namespace TuyaApp.Application.Extensions
{
    public static class UnixTimeStampConvertExtension
    {
        public static DateTime FromUnixTimestamp(this long unixTimestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTimestamp).ToLocalTime();
        }
    }
}
