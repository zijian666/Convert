using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.Int64;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="long"/> 转换器
    /// </summary>
    public class Int64Convertor : BaseConvertor<long>
                                , IFromConvertible<long>
                                , IFrom<byte[], long>
    {
        public ConvertResult<long> From(IConvertContext context, bool input) => input ? (long)1 : (long)0;
        public ConvertResult<long> From(IConvertContext context, char input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, sbyte input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, byte input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, short input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, ushort input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, int input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, uint input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, long input) => (long)input;
        public ConvertResult<long> From(IConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (long)input;
        }
        public ConvertResult<long> From(IConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (long)input;
        }
        public ConvertResult<long> From(IConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (long)input;
        }
        public ConvertResult<long> From(IConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return decimal.ToInt64(input);
        }
        public ConvertResult<long> From(IConvertContext context, DateTime input) => context.ConvertFail(this, input);
        public ConvertResult<long> From(IConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            if (TryParse(s, NumberStyles.Any, context.Settings.NumberFormatInfo ?? NumberFormatInfo.CurrentInfo, out var result))
            {
                return result;
            }
            if (s.Length > 2)
            {
                if (s[0] == '0')
                {
                    switch (s[1])
                    {
                        case 'x':
                        case 'X':
                            if (TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
                            {
                                return result;
                            }
                            break;
                        case 'b':
                        case 'B':
                            try
                            {
                                return System.Convert.ToInt64(s.Substring(2), 2);
                            }
                            catch (Exception e)
                            {
                                return e;
                            }
                        default:
                            break;
                    }
                }
                else if (s[0] == '&' && (s[1] == 'H' || s[1] == 'h'))
                {
                    if (TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out result))
                    {
                        return result;
                    }
                }
            }
            return context.ConvertFail(this, input);
        }
        public ConvertResult<long> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(long))
            {
                return context.ConvertFail(this, input);
            }
            return BitConverter.ToInt64(input.Slice(sizeof(long)), 0);
        }
    }
}
