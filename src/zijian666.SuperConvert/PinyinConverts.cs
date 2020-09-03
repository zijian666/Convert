using System;
using System.Text;
using zijian666.SuperConvert.Core;

namespace zijian666.SuperConvert
{
    public static partial class Converts
    {
        // GB2312-80 标准规范中第一个汉字的机内码.即"啊"的机内码
        private const int FIRST_CH_CODE = -20319;
        // GB2312-80 标准规范中最后一个汉字的机内码.即"齄"的机内码
        private const int LAST_CH_CODE = -2050;
        // GB2312-80 标准规范中最后一个一级汉字的机内码.即"座"的机内码
        private const int LAST_OF_ONE_LEVEL_CH_CODE = -10247;
        //字符缓冲
        [ThreadStatic]
        private static StringBuilder _buffer;

        // 配置中文字符
        //static Regex regex = new Regex("[\u4e00-\u9fa5]$");
        private static readonly Encoding _gb2312 = Encoding.GetEncoding("gb2312");

        /// <summary>
        /// 把汉字转换成拼音
        /// </summary>
        /// <param name="str"> 汉字字符串 </param>
        /// <param name="mode"> 转换方式 </param>
        /// <returns> 转换后的拼音(全拼)字符串 </returns>
        public static string ToPinyin(string str, PinyinMode mode)
            => mode switch
            {
                PinyinMode.Full => ToPinyin(str, false),
                PinyinMode.First => ToPinyinFirst(str),
                PinyinMode.AllFirst => ToPinyinAllFirst(str),
                PinyinMode.FullWithSplit => ToPinyin(str, true),
                _ => throw new ArgumentException("mode"),
            };

        /// <summary>
        /// 把汉字转换成拼音(全拼)
        /// </summary>
        /// <param name="str"> 汉字字符串 </param>
        /// <param name="split"> </param>
        /// <returns> 转换后的拼音(全拼)字符串 </returns>
        public static string ToPinyin(string str, bool split)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            if (_buffer == null)
            {
                _buffer = new StringBuilder(1023);
            }
            var chs = str.ToCharArray();

            try
            {
                _buffer.Append(ToPinyin(chs[0]));
                for (var i = 1; i < chs.Length; i++)
                {
                    if (split)
                    {
                        _buffer.Append(' ');
                    }
                    _buffer.Append(ToPinyin(chs[i]));
                }
                return _buffer.ToString();
            }
            finally
            {
                _buffer?.Clear();
            }
        }

        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
        /// </summary>
        /// <param name="str"> 单个汉字 </param>
        /// <returns> 单个大写字母 </returns>
        private static string ToPinyinFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            var rs = ToPinyin(str[0]);
            return string.IsNullOrEmpty(rs) ? rs : rs.Substring(0, 1);
        }

        /// <summary>
        /// 得到一串汉字字符串的拼音第一个字母，如果是一串英文字母则直接返回大写字母
        /// </summary>
        /// <param name="str"> 要转换的汉字字符串 </param>
        /// <returns> 拼音缩写字符串 </returns>
        private static string ToPinyinAllFirst(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (_buffer == null)
            {
                _buffer = new StringBuilder();
            }
            var chs = str.ToCharArray();
            try
            {
                for (var i = 0; i < chs.Length; i++)
                {
                    var rs = ToPinyin(chs[i]);
                    _buffer.Append(string.IsNullOrEmpty(rs) ? rs : rs.Substring(0, 1));
                }

                return _buffer.ToString();
            }
            finally
            {
                _buffer?.Clear();
            }
        }

        /// <summary>
        /// 获取单字拼音
        /// </summary>
        /// <param name="word"> </param>
        /// <returns> </returns>
        private static string ToPinyin(char word)
        {
            if (char.IsPunctuation(word) || char.IsSeparator(word) // 标点符号、分隔符       
                || (word < '\x4E00') || (word > '\x9FA5')) // 非中文字符  
            {
                return word.ToString();
            }

            var arr = _gb2312.GetBytes(word.ToString());
            var chr = arr[0] * 256 + arr[1] - 65536;

            //***// 单字符--英文或半角字符  
            if ((chr > 0) && (chr < 160))
            {
                return word.ToString();
            }

            #region 中文字符处理

            // 判断是否超过GB2312-80标准中的汉字范围
            if ((chr > LAST_CH_CODE) || (chr < FIRST_CH_CODE))
            {
                return word.ToString();
            }
            // 如果是在一级汉字中
            if (chr <= LAST_OF_ONE_LEVEL_CH_CODE)
            {
                // 将一级汉字分为12块,每块33个汉字.
                for (var aPos = 11; aPos >= 0; aPos--)
                {
                    var aboutPos = aPos * 33;
                    // 从最后的块开始扫描,如果机内码大于块的第一个机内码,说明在此块中
                    if (chr >= PinYin.Chars[aboutPos])
                    {
                        // Console.WriteLine("存在于第 " + aPos.ToString() + " 块,此块的第一个机内码是: " + _PyValue[aPos * 33].ToString());
                        // 遍历块中的每个音节机内码,从最后的音节机内码开始扫描,
                        // 如果音节内码小于机内码,则取此音节
                        for (var i = aboutPos + 32; i >= aboutPos; i--)
                        {
                            if (PinYin.Chars[i] <= chr)
                            {
                                // Console.WriteLine("找到第一个小于要查找机内码的机内码: " + _PyValue[i].ToString());
                                return PinYin.CharNames[i];
                            }
                        }
                        break;
                    }
                }
            }
            else // 如果是在二级汉字中
            {
                var yinpin = PinYin.Map(word);
                if (yinpin != null)
                {
                    return yinpin;
                }
            }

            #endregion 中文字符处理

            return "?";
        }


    }
}
