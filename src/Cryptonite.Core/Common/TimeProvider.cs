using System;

namespace Cryptonite.Core.Common
{
    public abstract class TimeProvider
    {
        public static DateTime Now
            => DateTimeProviderContext.Current == null
                ? DateTime.Now
                : DateTimeProviderContext.Current.ContextDateTimeNow;

        public static DateTime UtcNow => Now.ToUniversalTime();

        public static DateTime Today => Now.Date;
    }
}