using System;

namespace Cryptonite.Core.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime date)
        {
            var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            return utcDate;
        }
    }
}