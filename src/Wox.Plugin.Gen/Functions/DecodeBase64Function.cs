using System;
using System.Collections.Generic;
using System.Text;
using Wox.Plugin.Gen.Const;

namespace Wox.Plugin.Gen.Functions
{
    public class DecodeBase64Function : FunctionBase
    {
        public override string[] Keywords => new string[] { "decodebase64" };

        public DecodeBase64Function(PluginInitContext context) : base(context) { }

        public override List<Result> GetResults(GenQuery query)
        {
            var results = new List<Result>();

            try
            {
                if (!String.IsNullOrEmpty(query.SecondSearch))
                {
                    var base64Bytes = Convert.FromBase64String(query.SecondSearch);
                    var str = Encoding.UTF8.GetString(base64Bytes);

                    results.Add(new Result
                    {
                        Title = str,
                        SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                        IcoPath = Icons.UNLOCK_ICON_PATH,
                        Action = e => _copyToClipboard(str),
                        Score = Scores.MAX_SCORE
                    });
                }
            }
            catch (Exception ex)
            {
                results.Add(new Result
                {
                    Title = ex.Message,
                    SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                    IcoPath = Icons.UNLOCK_ICON_PATH,
                    Action = e => true,
                    Score = Scores.MAX_SCORE
                });
            }

            results.Add(GetInfoResult());

            return results;
        }

        public override Result GetInfoResult()
        {
            return CreateInfo(GetTranslatedDecodeBase64Title(), GetTranslatedDecodeBase64SubTitle(), Icons.UNLOCK_ICON_PATH);
        }

        #region i18n

        private string GetTranslatedDecodeBase64SubTitle()
        {
            return GetTranslation("wox_plugin_gen_decode_base64_sub_title");
        }

        private string GetTranslatedDecodeBase64Title()
        {
            return GetTranslation("wox_plugin_gen_decode_base64_title");
        }

        #endregion
    }
}
