using System;
using System.Collections.Generic;
using System.Linq;
using Wox.Plugin.Gen.Const;

namespace Wox.Plugin.Gen.Functions
{
    public class GuidFunction : FunctionBase
    {
        public override string[] Keywords => new string[] { "guid", "uuid" };

        public GuidFunction(PluginInitContext context) : base(context) { }

        public override List<Result> GetResults(GenQuery query)
        {
            var results = new List<Result>();
            var guid = Guid.NewGuid();

            // ToString() 默认为小写
            var guidStrings = new string[]
            {
                guid.ToString("N"),        // 00000000000000000000000000000000
                guid.ToString(),           // 00000000-0000-0000-0000-000000000000
                guid.ToString("B"),        // {00000000-0000-0000-0000-000000000000}
                guid.ToString("X")         // {0x00000000，0x0000，0x0000，{0x00，0x00，0x00，0x00，0x00，0x00，0x00，0x00}}
            };

            // u 参数，转换大写
            if ("u" == query.SecondSearch?.ToLower())
            {
                guidStrings = guidStrings.Select(s => s.ToUpper()).ToArray();
            }

            for (var i = 0; i < guidStrings.Length; i++)
            {
                var s = guidStrings[i];

                results.Add(new Result
                {
                    Title = s,
                    SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                    IcoPath = Icons.GUID_ICON_PATH,
                    Action = e => _copyToClipboard(s),
                    Score = Scores.MAX_SCORE - i
                });
            }

            results.Add(GetInfoResult());

            return results;
        }

        public override Result GetInfoResult()
        {
            return CreateInfo(GetTranslatedGuidTitle(), GetTranslatedGuidSubTitle(), Icons.GUID_ICON_PATH);
        }

        #region i18n

        private string GetTranslatedGuidSubTitle()
        {
            return GetTranslation("wox_plugin_gen_guid_sub_title");
        }

        private string GetTranslatedGuidTitle()
        {
            return GetTranslation("wox_plugin_gen_guid_title");
        }

        #endregion
    }
}
