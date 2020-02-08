﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Wox.Plugin.Gen
{
    public class Main : IPlugin
    {
        #region Const

        private const string TEXT_COPY_TO_CLIPBOARD = "复制到剪贴板";

        /// <summary>
        /// 排在结果第一行
        /// </summary>
        private const int MAX_SCORE = 999;

        /// <summary>
        /// 当前 command 的 helper 确保显示在到数第二行
        /// </summary>
        private const int COMMAND_SCORE = 1;

        /// <summary>
        /// helper 确保显示在最后一行
        /// </summary>
        private const int HELPER_SCORE = 0;

        private const string GEN_ICON_PATH = "Images/gen.png";
        private const string GUID_ICON_PATH = "Images/key.png";
        private const string RAND_ICON_PATH = "Images/dice.png";

        #endregion

        private Random _random = new Random();
        private Guid _guid;
        private bool _holdGuid = false;
        private Func<string, bool> _setToClipboard = value =>
        {
            Clipboard.SetText(value);
            return true;
        };

        public void Init(PluginInitContext context)
        {
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

            results.Add(new Result
            {
                Title = $"生成一切……",
                SubTitle = "指令 guid uuid rand roll",
                IcoPath = GEN_ICON_PATH,
                Action = e => true,
                Score = HELPER_SCORE
            });

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

            results = guidStrings.Select(s => new Result
            {
                Title = s,
                SubTitle = TEXT_COPY_TO_CLIPBOARD,
                IcoPath = GUID_ICON_PATH,
                Action = e => _setToClipboard(s),
                Score = MAX_SCORE
            }).ToList();

            results.Add(CreateInfo("guid|uuid [u]", "生成 GUID。默认为小写，u 转换为大写。", GUID_ICON_PATH));

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
                        SubTitle = TEXT_COPY_TO_CLIPBOARD,
                        IcoPath = RAND_ICON_PATH,
                        Action = e => _setToClipboard(valueString),
                        Score = MAX_SCORE
                    });
                }
            }
            catch (Exception ex)
            {
                results.Add(new Result
                {
                    Title = ex.Message.Replace("\r", String.Empty).Replace("\n", String.Empty),
                    SubTitle = "请输入正确的数字",
                    IcoPath = RAND_ICON_PATH,
                    Action = e => true,
                    Score = MAX_SCORE
                });
            }

            results.Add(CreateInfo(
                "rand|roll [[max]|[min max]]",
                "生成随机整数。默认>=0 并且 <100，min 指定最小值，max 指定最大值。",
                RAND_ICON_PATH));

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
                IcoPath = GUID_ICON_PATH,
                Action = e => true,
                Score = COMMAND_SCORE
            };
        }

        #endregion
    }
}
