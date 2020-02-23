using System;
using System.Collections.Generic;
using Wox.Plugin.Gen.Const;
using Wox.Plugin.Gen.Extensions;

namespace Wox.Plugin.Gen.Functions
{
    public class UnixTimestampFunction : FunctionBase
    {
        public override string[] Keywords => new string[] { "unixtime", "timestamp" };

        public override string Usage => "timestamp|unixtime [unix_timestamp]";

        public UnixTimestampFunction(PluginInitContext context) : base(context) { }

        public override List<Result> GetResults(GenQuery query)
        {
            var results = new List<Result>();

            // 第二个参数，即需要转换的 unix timestamp
            if (String.IsNullOrEmpty(query.SecondSearch))
            {
                // 没传第二个参数，表示根据当前时间生成 unix timestamp

                var utcDatetime = DateTimeOffset.UtcNow;
                var timestamp = utcDatetime.ToUnixTimeSeconds().ToString();

                results.Add(new Result
                {
                    Title = timestamp,
                    SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                    IcoPath = Icons.TIME_ICON_PATH,
                    Action = e => _copyToClipboard(timestamp),
                    Score = Scores.MAX_SCORE
                });
            }
            else
            {
                // 传入第二个参数，表示要将传入的 timestamp 转换为可读的日期格式
                try
                {
                    var timestamp = Int64.Parse(query.SecondSearch);
                    var utcDatetime = DateTimeExtension.UtcZero.FromUnixTimeSeconds(timestamp);

                    // UTC 时间
                    var utcDateTimeString = utcDatetime.DateTime.ToString();    // .DateTime 是为了消掉 ToString() 后的 +08:00 
                    results.Add(new Result
                    {
                        Title = utcDateTimeString,
                        SubTitle = GetTranslatedUnixTimestampUtcTipSubTitle(),
                        IcoPath = Icons.TIME_ICON_PATH,
                        Action = e => _copyToClipboard(utcDateTimeString),
                        Score = Scores.MAX_SCORE
                    });

                    // 当前计算机时区时间
                    var localDateTimeString = utcDatetime.ToLocalTime().DateTime.ToString();
                    results.Add(new Result
                    {
                        Title = localDateTimeString,
                        SubTitle = GetTranslatedUnixTimestampLocalTipSubTitle(TimeZoneInfo.Local.StandardName, TimeZoneInfo.Local.DisplayName),
                        IcoPath = Icons.TIME_ICON_PATH,
                        Action = e => _copyToClipboard(localDateTimeString),
                        Score = Scores.MAX_SCORE - 1
                    });
                }
                catch (Exception ex)
                {
                    results.Add(new Result
                    {
                        Title = ex.Message.RemoveLineWrapping(),
                        SubTitle = GetTranslatedUnixTimestampExceptionSubTitle(),
                        IcoPath = Icons.TIME_ICON_PATH,
                        Action = e => true,
                        Score = Scores.MAX_SCORE - 1
                    });
                }
            }

            results.Add(GetInfoResult());

            return results;
        }

        public override Result GetInfoResult()
        {
            return CreateInfo(Usage, GetTranslatedUnixTimestampSubTitle(), Icons.TIME_ICON_PATH);
        }

        #region i18n

        private string GetTranslatedUnixTimestampSubTitle()
        {
            return GetTranslation("wox_plugin_gen_unix_timestamp_sub_title");
        }

        private string GetTranslatedUnixTimestampUtcTipSubTitle()
        {
            return GetTranslation("wox_plugin_gen_unix_timestamp_tip_utc");
        }

        private string GetTranslatedUnixTimestampLocalTipSubTitle(string standardName, string displayName)
        {
            return String.Format(GetTranslation("wox_plugin_gen_unix_timestamp_tip_local"), standardName, displayName);
        }

        private string GetTranslatedUnixTimestampExceptionSubTitle()
        {
            return GetTranslation("wox_plugin_gen_unix_timestamp_exception_sub_title");
        }

        #endregion
    }
}
