using System;
using System.Collections.Generic;
using System.Linq;
using Wox.Plugin.Gen.Const;
using Wox.Plugin.Gen.Extensions;

namespace Wox.Plugin.Gen.Functions
{
    public class UnixTimestampFunction : FunctionBase
    {
        public override string[] Keywords => new string[] { "unixtime", "timestamp" };

        public UnixTimestampFunction(PluginInitContext context) : base(context) { }

        public override List<Result> GetResults(GenQuery query)
        {
            var results = new List<Result>();

            var utcDatetime = DateTimeOffset.UtcNow;

            var unixTimeStrings = new string[]
            {
                // UTC 时间戳
                utcDatetime.ToUnixTimeSeconds().ToString(),

                // 当前计算机时区时间戳
                utcDatetime.ToLocalTime().ToUnixTimeSeconds().ToString()
            };

            var tipStrings = new string[]
            {
                String.Format(GetTranslatedUnixTimestampUtcTipSubTitle(), utcDatetime.DateTime.ToString()),
                String.Format(GetTranslatedUnixTimestampLocalTipSubTitle(),TimeZoneInfo.Local.StandardName, utcDatetime.ToLocalTime().DateTime.ToString())
            };

            results.AddRange(unixTimeStrings.Select((s, index) => new Result
            {
                Title = utcDatetime.ToUnixTimeSeconds().ToString(),
                SubTitle = tipStrings[index] + GetTranslatedGlobalTipCopyToClipboard(),
                IcoPath = Icons.TIME_ICON_PATH,
                Action = e => _copyToClipboard(s),
                Score = Scores.MAX_SCORE - index
            }));

            results.Add(GetInfoResult());

            return results;
        }

        public override Result GetInfoResult()
        {
            return CreateInfo(GetTranslatedUnixTimestampTitle(), GetTranslatedUnixTimestampSubTitle(), Icons.TIME_ICON_PATH);
        }

        #region i18n

        private string GetTranslatedUnixTimestampSubTitle()
        {
            return GetTranslation("wox_plugin_gen_unix_timestamp_sub_title");
        }

        private string GetTranslatedUnixTimestampTitle()
        {
            return GetTranslation("wox_plugin_gen_unix_timestamp_title");
        }

        private string GetTranslatedUnixTimestampUtcTipSubTitle()
        {
            return GetTranslation("wox_plugin_gen_unix_timestamp_tip_utc");
        }

        private string GetTranslatedUnixTimestampLocalTipSubTitle()
        {
            return GetTranslation("wox_plugin_gen_unix_timestamp_tip_local");
        }

        #endregion
    }
}
