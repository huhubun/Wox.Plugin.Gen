using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Wox.Plugin.Gen.Const;
using Wox.Plugin.Gen.Functions;

namespace Wox.Plugin.Gen
{
    public class Main : IPlugin, IPluginI18n
    {
        private PluginInitContext _context;

        private Dictionary<string, FunctionBase> _functions = new Dictionary<string, FunctionBase>();

        public void Init(PluginInitContext context)
        {
            _context = context;

            // 重要
            // 不要在 Init() 方法里使用 context.API.GetTranslation()，会导致获取不到翻译内容，而且大大延长插件“加载耗时”

            var functionBaseType = typeof(FunctionBase);
            var functionTypes = functionBaseType.Assembly.GetTypes().Where(t => t.IsSubclassOf(functionBaseType));

            foreach (var functionType in functionTypes)
            {
                var function = Activator.CreateInstance(functionType, _context) as FunctionBase;

                foreach (var keyword in function.Keywords)
                {
                    _functions.Add(keyword, function);
                }
            }
        }

        public List<Result> Query(Query query)
        {
            // 重要
            // 不要使用 Wox.Plugin.Query 提供的 FirstSearch、SecondSearch 等获取查询内容，个人认为存在性能问题，请使用 GenQuery 替代
            var genQuery = new GenQuery(query.RawQuery);
            var search = genQuery.FirstSearch ?? String.Empty;

            var functionResults = new List<Result>();
            var autoCompleteResult = new List<Result>();

            foreach (var key in _functions.Keys)
            {
                var function = _functions[key];

                if (key.Equals(search, StringComparison.InvariantCultureIgnoreCase))
                {
                    functionResults.AddRange(function.GetResults(genQuery));
                    break;
                }

                if (key.StartsWith(search, true, CultureInfo.InvariantCulture))
                {
                    var infoResult = function.GetInfoResult();

                    if (!String.IsNullOrEmpty(search))
                    {
                        infoResult.Title = $"{search}: {infoResult.Title}";
                    }

                    infoResult.Action = actionContext =>
                    {
                        _context.API.ChangeQuery($"{query.ActionKeyword} {key} ", requery: true);
                        return false;
                    };

                    if (autoCompleteResult.All(r => r != infoResult))
                    {
                        autoCompleteResult.Add(infoResult);
                    }
                }
            }

            if (!functionResults.Any())
            {
                functionResults = autoCompleteResult;
            }

            functionResults.Add(new Result
            {
                Title = GetTranslatedGlobalTipTitle(),
                SubTitle = GetTranslatedGlobalTipSubTitle(),
                IcoPath = Icons.GEN_ICON_PATH,
                Action = e => false,
                Score = Scores.GLOBAL_TIP_SCORE
            });

#if DEBUG
            functionResults.Add(new Result
            {
                Title = $"ActionKeyword = [{query.ActionKeyword}], Search = [{query.Search}] | functionResults = {functionResults.Count}, autoCompleteResult = {autoCompleteResult.Count}",
                SubTitle = $"search = {search}, genQuery = {String.Join(",", genQuery.Queries)}",
                IcoPath = Icons.GEN_ICON_PATH,
                Action = e => false,
                Score = Scores.GLOBAL_TIP_SCORE
            });
#endif

            return functionResults;
        }

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

        protected string GetTranslatedGlobalTipTitle()
        {
            return _context.API.GetTranslation("wox_plugin_gen_global_tip_title");
        }

        private string _supportedKeys = null;
        protected string GetTranslatedGlobalTipSubTitle()
        {
            if (_supportedKeys == null)
            {
                _supportedKeys = String.Join(" ", _functions.Keys);
            }

            return String.Format(_context.API.GetTranslation("wox_plugin_gen_global_tip_sub_title"), _supportedKeys);
        }

        #endregion
    }
}
