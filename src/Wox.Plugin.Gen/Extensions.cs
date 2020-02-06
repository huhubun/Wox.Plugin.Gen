using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.Gen
{
    public static class StringExtensions
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
    }
}
