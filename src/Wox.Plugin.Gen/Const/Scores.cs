namespace Wox.Plugin.Gen.Const
{
    public static class Scores
    {
        /// <summary>
        /// 排在结果第一行
        /// </summary>
        public const int MAX_SCORE = 999;

        /// <summary>
        /// 当前 command 的 helper 确保显示在倒数第二行
        /// </summary>
        public const int COMMAND_SCORE = 2;

        /// <summary>
        /// global tip 确保显示在最后一行
        /// </summary>
        public const int GLOBAL_TIP_SCORE = -1;
    }
}
