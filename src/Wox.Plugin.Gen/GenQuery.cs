using System;
using System.Linq;

namespace Wox.Plugin.Gen
{
    public class GenQuery
    {
        public string RawQuery { get; private set; }

        public string FirstSearch { get; private set; }

        public string SecondSearch { get; private set; }

        public string ThirdSearch { get; private set; }

        /// <summary>
        /// 查询关键字集合。不含插件的关键字。
        /// </summary>
        public string[] Queries { get; private set; }

        public GenQuery(string rawQuery)
        {
            // 个人感觉 Wox.Plugin.Query 的 SecondQuery 有一定的性能问题，这里进行重新封装

            RawQuery = rawQuery;

            Queries = RawQuery.Split(' ').Skip(1).Where(s => !String.IsNullOrEmpty(s)).ToArray();

            if (Queries.Length > 0)
            {
                FirstSearch = Queries[0];
            }

            if (Queries.Length > 1)
            {
                SecondSearch = Queries[1];
            }

            if (Queries.Length > 2)
            {
                ThirdSearch = Queries[2];
            }
        }
    }
}
