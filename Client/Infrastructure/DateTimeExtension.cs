using System;

namespace Couple.Client.Infrastructure
{
    public static class DateTimeExtension
    {
        public static string ToCalendarUrl(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");
    }
}
