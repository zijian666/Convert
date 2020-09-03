using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="DateTime" /> 转换器
    /// </summary>
    public class DateTimeConvertor : FromConvertor<DateTime>, IFrom<string, DateTime>
    {
        /// <summary>
        /// 日期格式化字符
        /// </summary>
        private static readonly string[] _formats = {
            "HH:mm",
            "HHmmss",
            "HH:mm:ss",
            "yyyyMMdd",
            "yyyy/MM/dd",
            "yyyy-MM-dd",
            "yyyy.MM.dd",
            "yyyyMMddHHmmss",
            "yyyyMMddHHmmssfff",
            "yyyy-MM-dd HH:mm:ss",
            "yyyyMMddHHmmssffffff" ,
            "yyyy-MM-dd HH:mm:ss:fff",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-dd HH:mm:ss:ffffff" ,
            "yyyy-MM-dd HH:mm:ss.ffffff" , };


        public ConvertResult<DateTime> From(IConvertContext context, string input)
        {
            var s = input?.ToString(context.Settings.GetFormatProvider(typeof(string)))?.Trim() ?? "";
            if (DateTime.TryParse(s, out var result))
            {
                return result;
            }
            foreach (var format in _formats)
            {
                if (s.Length == format.Length)
                {
                    if (DateTime.TryParseExact(s, format, null, DateTimeStyles.None, out result))
                    {
                        return result;
                    }
                    break;
                }
            }
            return context.ConvertFail(this, input);
        }
    }
}
