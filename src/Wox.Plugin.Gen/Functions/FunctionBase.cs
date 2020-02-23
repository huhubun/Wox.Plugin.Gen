using System;
using System.Collections.Generic;
using System.Windows;
using Wox.Plugin.Gen.Const;

namespace Wox.Plugin.Gen.Functions
{
    public abstract class FunctionBase
    {
        protected Func<string, bool> _copyToClipboard = value =>
        {
            Clipboard.SetText(value);
            return true;
        };

        protected readonly PluginInitContext _context;

        public virtual string[] Keywords { get; }

        public FunctionBase(PluginInitContext context)
        {
            _context = context;
        }

        public abstract List<Result> GetResults(GenQuery query);

        public abstract Result GetInfoResult();

        /// <summary>
        /// 生成提示信息用的结果，位于到数第二位，指示当前指令如何使用。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        /// <param name="icoPath"></param>
        /// <returns></returns>
        protected Result CreateInfo(string title, string subTitle, string icoPath)
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

        #region i18n

        protected string GetTranslation(string key)
        {
            return _context.API.GetTranslation(key);
        }

        protected string GetTranslatedGlobalTipCopyToClipboard()
        {
            return GetTranslation("wox_plugin_gen_global_tip_copy_to_clipboard");
        }

        #endregion
    }
}
