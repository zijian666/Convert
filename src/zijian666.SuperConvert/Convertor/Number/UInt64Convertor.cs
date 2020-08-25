using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.UInt64;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="ulong"/> 转换器
    /// </summary>
    public class UInt64Convertor : BaseConvertor<ulong>
                                , IFromConvertible<ulong>
                                , IFrom<object, ulong>
                                , IFrom<byte[], ulong>
    {
        public ConvertResult<ulong> From(IConvertContext context, bool input) => input ? (ulong)1 : (ulong)0;
        public ConvertResult<ulong> From(IConvertContext context, char input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, sbyte input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, byte input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, short input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, ushort input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, int input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, uint input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, long input)
        {
            if (input < 0)
            {
                return Exceptions.Overflow($"{input} < {MinValue}", context.Settings.CultureInfo);
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(IConvertContext context, ulong input) => (ulong)input;
        public ConvertResult<ulong> From(IConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(IConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (ulong)input;
        }
        public ConvertResult<ulong> From(IConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return decimal.ToUInt64(input);
        }
        public ConvertResult<ulong> From(IConvertContext context, DateTime input) => context.ConvertFail(this, input);
        public ConvertResult<ulong> From(IConvertContext context, string input)
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
                                return System.Convert.ToUInt64(s.Substring(2), 2);
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
        public ConvertResult<ulong> From(IConvertContext context, object input) => context.ConvertFail(this, input);
        public ConvertResult<ulong> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(ulong))
            {
                return context.ConvertFail(this, input);
            }
            return BitConverter.ToUInt64(input.Slice(sizeof(ulong)), 0);
        }
    }
}
