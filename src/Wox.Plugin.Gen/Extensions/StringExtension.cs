using System;
using System.Linq;

namespace Wox.Plugin.Gen.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 忽略大小写并判断两个字符串内容是否一致
        /// </summary>
        /// <param name="source">要进行比较的值</param>
        /// <param name="value">目标</param>
        /// <returns></returns>
        public static bool BetterEquals(this string source, string value)
        {
            var isSourceNullOrEmpty = String.IsNullOrEmpty(source);
            var isValueNullOrEmpty = String.IsNullOrEmpty(value);

            if (!isSourceNullOrEmpty && !isValueNullOrEmpty)
            {
                return source.ToLowerInvariant() == value.ToLowerInvariant();
            }

            if (isSourceNullOrEmpty && isValueNullOrEmpty)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 忽略大小写并判断要比较的值与目标中任意字符串一致
        /// </summary>
        /// <param name="source">要进行比较的值</param>
        /// <param name="values">多个目标</param>
        /// <returns></returns>
        public static bool EqualsAny(this string source, params string[] values)
        {
            return values.Any(v => source.BetterEquals(v));
        }

        /// <summary>
        /// 移除换行符 \r 和 \n
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveLineWrapping(this string str)
        {
            return str.Replace("\r", String.Empty).Replace("\n", String.Empty);
        }
    }
}
