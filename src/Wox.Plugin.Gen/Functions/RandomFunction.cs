﻿using System;
using System.Collections.Generic;
using Wox.Plugin.Gen.Const;

namespace Wox.Plugin.Gen.Functions
{
    public class RandomFunction : FunctionBase
    {
        private Random _random = new Random();

        public override string[] Keywords => new string[] { "rand", "roll" };

        public RandomFunction(PluginInitContext context) : base(context)
        {
            _random = new Random();
        }

        public override List<Result> GetResults(GenQuery query)
        {
            var results = new List<Result>();

            try
            {
                int? value = null;

                // 1 个参数的情况，即 rand 
                if (query.Queries.Length == 1)
                {
                    value = _random.Next(0, 100);
                }

                // 2 个参数的情况，即 rand maxValue
                if (query.Queries.Length == 2)
                {
                    var maxValue = Int32.Parse(query.SecondSearch);
                    value = _random.Next(maxValue);
                }

                // 3 个参数的情况，即 rand minValue maxValue
                if (query.Queries.Length == 3)
                {
                    var minValue = Int32.Parse(query.SecondSearch);
                    var maxValue = Int32.Parse(query.ThirdSearch);
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

            results.Add(GetInfoResult());

            return results;
        }

        public override Result GetInfoResult()
        {
            return CreateInfo(GetTranslatedRandTitle(), GetTranslatedRandSubTitle(), Icons.RAND_ICON_PATH);
        }

        #region i18n

        private string GetTranslatedRandSubTitle()
        {
            return GetTranslation("wox_plugin_gen_rand_sub_title");
        }

        private string GetTranslatedRandTitle()
        {
            return GetTranslation("wox_plugin_gen_rand_title");
        }

        private string GetTranslatedRandExceptionSubTitle()
        {
            return GetTranslation("wox_plugin_gen_rand_exception_sub_title");
        }

        #endregion
    }
}