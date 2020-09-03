namespace zijian666.SuperConvert
{
    /// <summary>
    /// 拼音转换方式
    /// </summary>
    public enum PinyinMode
    {
        /// <summary>
        /// 全拼
        /// </summary>
        Full = 1,

        /// <summary>
        /// 首声母
        /// </summary>
        First = 2,

        /// <summary>
        /// 全声母
        /// </summary>
        AllFirst = 3,

        /// <summary>
        /// 全拼但中间有空格
        /// </summary>
        FullWithSplit = 4
    }
}
