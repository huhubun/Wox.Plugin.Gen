using System;
using System.Collections.Generic;
using System.Text;
using Wox.Plugin.Gen.Const;

namespace Wox.Plugin.Gen.Functions
{
    public class EncodeBase64Function : FunctionBase
    {
        public override string[] Keywords => new string[] { "encodebase64" };

        public override string Usage => "encodebase64 text";

        public EncodeBase64Function(PluginInitContext context) : base(context) { }

        public override List<Result> GetResults(GenQuery query)
        {
            var results = new List<Result>();

            if (!String.IsNullOrEmpty(query.SecondSearch))
            {
                var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(query.SecondSearch));

                results.Add(new Result
                {
                    Title = base64String,
                    SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                    IcoPath = Icons.LOCK_ICON_PATH,
                    Action = e => _copyToClipboard(base64String),
                    Score = Scores.MAX_SCORE
                });
            }

            results.Add(GetInfoResult());

            return results;
        }

        public override Result GetInfoResult()
        {
            return CreateInfo(Usage, GetTranslatedEncodeBase64SubTitle(), Icons.LOCK_ICON_PATH);
        }


        #region i18n

        private string GetTranslatedEncodeBase64SubTitle()
        {
            return GetTranslation("wox_plugin_gen_encode_base64_sub_title");
        }

        #endregion
    }
}
