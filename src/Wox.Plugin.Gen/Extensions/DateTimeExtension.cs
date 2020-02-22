using System;

namespace Wox.Plugin.Gen.Extensions
{
    public static class DateTimeExtension
    {
        public static long ToUnixTimeSeconds(this DateTimeOffset dateTime)
        {
            return (long)(dateTime - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds;
        }
    }
}
