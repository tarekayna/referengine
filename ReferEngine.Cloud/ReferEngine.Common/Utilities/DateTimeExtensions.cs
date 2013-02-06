using System;

namespace ReferEngine.Common.Utilities
{
    public static class DateTimeExtensions
    {
        public static bool IsToday(this DateTime thisDateTime, bool isUtc = true)
        {
            return thisDateTime.Date.Equals((isUtc ? DateTime.UtcNow : DateTime.Now).Date);
        }

        public static bool IsSameDate(this DateTime thisDateTime, DateTime dateTime)
        {
            return thisDateTime.Date.Equals(dateTime.Date);
        }
    }
}
