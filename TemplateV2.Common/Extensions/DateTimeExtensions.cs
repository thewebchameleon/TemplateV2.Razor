using System;

namespace TemplateV2.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateTimeString(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy hh:mm tt");
        }

        public static string ToLongDateString(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy");
        }

        public static string ToLongDateTimeString(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy hh:mm tt");
        }
    }
}