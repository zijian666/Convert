using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="TimeSpan" /> 转换器
    /// </summary>
    class TimeSpanConvertor : BaseConvertor<TimeSpan>, IFrom<string, TimeSpan>, IFrom<IConvertible, TimeSpan>
    {
        /// <summary>
        /// 日期格式化字符
        /// </summary>
        private static readonly string[] _formats = {
            "hhmmss",
            "hhmmssfff",
            "hhmmssffffff",
            "hh:mm",
            "hh:mm:ss",
            "hh:mm:ss.fff",
            "hh:mm:ss.ffffff",
        };

        public ConvertResult<TimeSpan> From(IConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            if (TimeSpan.TryParse(s, out var result))
            {
                return result;
            }
            foreach (var format in _formats)
            {
                if (s.Length == format.Length)
                {
                    if (TimeSpan.TryParseExact(input, format, null, System.Globalization.TimeSpanStyles.None, out result))
                    {
                        return result;
                    }
                    break;
                }
            }
            return context.ConvertFail(this, input);
        }

        public ConvertResult<TimeSpan> From(IConvertContext context, IConvertible input)
        {
            if (input?.GetTypeCode() == TypeCode.DateTime)
            {
                var time = input.ToDateTime(context.Settings.GetFormatProvider(typeof(DateTime)));
                return new TimeSpan(time.Hour, time.Minute, time.Second, time.Millisecond);
            }
            return context.ConvertFail(this, input);
        }
    }
}
