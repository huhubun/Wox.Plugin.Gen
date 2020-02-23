using System;

namespace Wox.Plugin.Gen.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTimeOffset UtcZero { get; } = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static long ToUnixTimeSeconds(this DateTimeOffset dateTime)
        {
            return (long)(dateTime - UtcZero).TotalSeconds;
        }

        public static DateTimeOffset FromUnixTimeSeconds(this DateTimeOffset dateTime, long seconds)
        {
            return UtcZero.AddSeconds(seconds);
        }
    }
}
