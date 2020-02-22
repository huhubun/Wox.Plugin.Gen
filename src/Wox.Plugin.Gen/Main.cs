using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Wox.Plugin.Gen.Const;
using Wox.Plugin.Gen.Extensions;

namespace Wox.Plugin.Gen
{
    public class Main : IPlugin, IPluginI18n
    {
        private PluginInitContext _context;

        private Random _random = new Random();
        private Guid _guid;
        private bool _holdGuid = false;
        private Func<string, bool> _copyToClipboard = value =>
        {
            Clipboard.SetText(value);
            return true;
        };

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();


            if (IsHitGuidGen(query))
            {
                results.AddRange(GuidGen(query));
            }
            else
            {
                _holdGuid = false;
            }

            if (IsHitRandomGen(query))
            {
                results.AddRange(RandomGen(query));
            }

            if (IsHitaUnixTimestampGen(query))
            {
                results.AddRange(UnixTimestampGen());
            }

            if (IsHitEncodeBase64(query))
            {
                results.AddRange(EncodeBase64(query));
            }

            if (IsHitDecodeBase64(query))
            {
                results.AddRange(DecodeBase64(query));
            }

            results.Add(new Result
            {
                Title = GetTranslatedGlobalTipTitle(),
                SubTitle = GetTranslatedGlobalTipSubTitle(),
                IcoPath = Icons.GEN_ICON_PATH,
                Action = e => false,
                Score = Scores.GLOBAL_TIP_SCORE
            });

#if DEBUG
            results.Add(new Result
            {
                Title = $"ActionKeyword = [{query.ActionKeyword}], Search = [{query.Search}]",
                SubTitle = $"FirstSearch = [{query.FirstSearch}], SecondSearch = [{query.SecondSearch}], ThirdSearch = [{query.ThirdSearch}]",
                IcoPath = Icons.GEN_ICON_PATH,
                Action = e => false,
                Score = Scores.GLOBAL_TIP_SCORE
            });
#endif

            return results;
        }

        #region GuidGen

        private bool IsHitGuidGen(Query query)
        {
            return query.FirstSearch.EqualsAny("guid", "uuid");
        }

        /// <summary>
        /// guid|uuid [u] 生成 guid
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<Result> GuidGen(Query query)
        {
            var results = new List<Result>();

            if (!_holdGuid)
            {
                _guid = Guid.NewGuid();
                _holdGuid = true;
            }

            // ToString() 默认为小写
            var guidStrings = new string[]
            {
                _guid.ToString("N"),        // 00000000000000000000000000000000
                _guid.ToString(),           // 00000000-0000-0000-0000-000000000000
                _guid.ToString("B"),        // {00000000-0000-0000-0000-000000000000}
                _guid.ToString("X")         // {0x00000000，0x0000，0x0000，{0x00，0x00，0x00，0x00，0x00，0x00，0x00，0x00}}
            };

            // u 参数，转换大写
            if (query.SecondSearch.BetterEquals("u"))
            {
                guidStrings = guidStrings.Select(s => s.ToUpper()).ToArray();
            }

            results = guidStrings.Select((s, index) => new Result
            {
                Title = s,
                SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                IcoPath = Icons.GUID_ICON_PATH,
                Action = e => _copyToClipboard(s),
                Score = Scores.MAX_SCORE - index
            }).ToList();

            results.Add(CreateInfo(GetTranslatedGuidTitle(), GetTranslatedGuidSubTitle(), Icons.GUID_ICON_PATH));

            return results;
        }

        #endregion

        #region RandomGen

        private bool IsHitRandomGen(Query query)
        {
            return query.FirstSearch.EqualsAny("rand", "roll");
        }

        /// <summary>
        /// rand|roll [[max]|[min max]]生成 随机数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<Result> RandomGen(Query query)
        {
            var results = new List<Result>();

            try
            {
                int? value = null;

                // 2 个参数的情况，即 gen rand 
                if (query.Terms.Length == 2)
                {
                    value = _random.Next(0, 100);
                }

                // 3 个参数的情况，即 gen rand maxValue
                if (query.Terms.Length == 3)
                {
                    var maxValue = Int32.Parse(query.Terms[2]);
                    value = _random.Next(maxValue);
                }

                // 4 个参数的情况，即 gen rand minValue maxValue
                if (query.Terms.Length == 4)
                {
                    var minValue = Int32.Parse(query.Terms[2]);
                    var maxValue = Int32.Parse(query.Terms[3]);
                    value = _random.Next(minValue, maxValue);
                }

                if (value.HasValue)
                {
                    var valueString = value.ToString();

                    results.Add(new Result
                    {
                        Title = value.ToString(),
                        SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                        IcoPath = Icons.RAND_ICON_PATH,
                        Action = e => _copyToClipboard(valueString),
                        Score = Scores.MAX_SCORE
                    });
                }
            }
            catch (Exception ex)
            {
                results.Add(new Result
                {
                    Title = ex.Message.Replace("\r", String.Empty).Replace("\n", String.Empty),
                    SubTitle = GetTranslatedRandExceptionSubTitle(),
                    IcoPath = Icons.RAND_ICON_PATH,
                    Action = e => true,
                    Score = Scores.MAX_SCORE
                });
            }

            results.Add(CreateInfo(
                GetTranslatedRandTitle(),
                GetTranslatedRandSubTitle(),
                Icons.RAND_ICON_PATH));

            return results;
        }

        #endregion

        #region Unix timestamp

        private bool IsHitaUnixTimestampGen(Query query)
        {
            return query.FirstSearch.EqualsAny("unixtime", "timestamp");
        }

        private List<Result> UnixTimestampGen()
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

            results.Add(CreateInfo(GetTranslatedUnixTimestampTitle(), GetTranslatedUnixTimestampSubTitle(), Icons.TIME_ICON_PATH));

            return results;
        }

        #endregion

        #region Base64 加密

        private bool IsHitEncodeBase64(Query query)
        {
            return query.FirstSearch.EqualsAny("encodebase64");
        }

        private List<Result> EncodeBase64(Query query)
        {
            var results = new List<Result>();

            var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(query.SecondSearch));

            results.Add(new Result
            {
                Title = base64String,
                SubTitle = GetTranslatedGlobalTipCopyToClipboard(),
                IcoPath = Icons.LOCK_ICON_PATH,
                Action = e => _copyToClipboard(base64String),
                Score = Scores.MAX_SCORE
            });

            results.Add(CreateInfo(GetTranslatedEncodeBase64Title(), GetTranslatedEncodeBase64SubTitle(), Icons.LOCK_ICON_PATH));

            return results;
        }

        #endregion

        #region Base64 解密

        private bool IsHitDecodeBase64(Query query)
        {
            return query.FirstSearch.EqualsAny("decodebase64");
        }

        private List<Result> DecodeBase64(Query query)
        {
            var results = new List<Result>();

            try
            {
                var base64Bytes = Convert.FromBase64String(query.SecondSearch);

                if (base64Bytes.Length > 0)
                {
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

            results.Add(CreateInfo(GetTranslatedDecodeBase64Title(), GetTranslatedDecodeBase64SubTitle(), Icons.UNLOCK_ICON_PATH));

            return results;
        }

        #endregion

        #region Private

        /// <summary>
        /// 生成提示信息用的结果，位于到数第二位，指示当前指令如何使用。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        /// <param name="icoPath"></param>
        /// <returns></returns>
        private Result CreateInfo(string title, string subTitle, string icoPath)
        {
            return new Result
            {
                Title = title,
                SubTitle = subTitle,
                IcoPath = icoPath,
                Action = e => false,
                Score = Scores.COMMAND_SCORE
            };
        }

        #endregion

        #region I18n

        // 通用
        public string GetTranslatedPluginTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return _context.API.GetTranslation("wox_plugin_gen_plugin_description");
        }

        private string GetTranslatedGlobalTipTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_global_tip_title");
        }

        private string GetTranslatedGlobalTipSubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_global_tip_sub_title");
        }

        private string GetTranslatedGlobalTipCopyToClipboard()
        {
            return _context.API.GetTranslation("wox_plugin_gen_global_tip_copy_to_clipboard");
        }

        // GUID 生成
        private string GetTranslatedGuidSubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_guid_sub_title");
        }

        private string GetTranslatedGuidTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_guid_title");
        }

        // 随机数生成
        private string GetTranslatedRandSubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_rand_sub_title");
        }

        private string GetTranslatedRandTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_rand_title");
        }

        private string GetTranslatedRandExceptionSubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_rand_exception_sub_title");
        }

        // 时间戳生成
        private string GetTranslatedUnixTimestampSubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_unix_timestamp_sub_title");
        }

        private string GetTranslatedUnixTimestampTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_unix_timestamp_title");
        }

        private string GetTranslatedUnixTimestampUtcTipSubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_unix_timestamp_tip_utc");
        }

        private string GetTranslatedUnixTimestampLocalTipSubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_unix_timestamp_tip_local");
        }

        // Base64 加密
        private string GetTranslatedEncodeBase64SubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_encode_base64_sub_title");
        }

        private string GetTranslatedEncodeBase64Title()
        {
            return _context.API.GetTranslation("wox_plugin_gen_encode_base64_title");
        }

        // Base64 解密
        private string GetTranslatedDecodeBase64SubTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_decode_base64_sub_title");
        }

        private string GetTranslatedDecodeBase64Title()
        {
            return _context.API.GetTranslation("wox_plugin_gen_decode_base64_title");
        }
        #endregion
    }
}
