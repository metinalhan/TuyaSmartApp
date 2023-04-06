using System;

namespace TuyaApp.Infrastructure.Extensions
{
    // This static class provides an extension method for the DateTime class to convert a given datetime to a Unix timestamp in milliseconds.
    public static class MiliSecondsTimeStamp
    {
        public static string MillisecondsTimestamp(this DateTime date)
        {
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long l = (long)(date.ToUniversalTime() - baseDate).TotalMilliseconds;
            return l.ToString();
        }
    }
}
