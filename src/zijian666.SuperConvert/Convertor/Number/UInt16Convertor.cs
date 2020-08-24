using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.UInt16;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="ushort"/> 转换器
    /// </summary>
    public class UInt16Convertor : BaseConvertor<ushort>
                                , IFromConvertible<ushort>
                                , IFrom<object, ushort>
                                , IFrom<byte[], ushort>
    {
        public ConvertResult<ushort> From(IConvertContext context, bool input) => input ? (ushort)1 : (ushort)0;
        public ConvertResult<ushort> From(IConvertContext context, char input) => (ushort)input;
        public ConvertResult<ushort> From(IConvertContext context, sbyte input) => (ushort)input;
        public ConvertResult<ushort> From(IConvertContext context, byte input) => (ushort)input;
        public ConvertResult<ushort> From(IConvertContext context, short input) => (ushort)input;
        public ConvertResult<ushort> From(IConvertContext context, ushort input) => (ushort)input;
        public ConvertResult<ushort> From(IConvertContext context, int input) => (ushort)input;
        public ConvertResult<ushort> From(IConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(IConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(IConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(IConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(IConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (ushort)input;
        }
        public ConvertResult<ushort> From(IConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return decimal.ToUInt16(input);
        }
        public ConvertResult<ushort> From(IConvertContext context, DateTime input) => Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
        public ConvertResult<ushort> From(IConvertContext context, string input)
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
                                return System.Convert.ToUInt16(s.Substring(2), 2);
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
            return Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
        }
        public ConvertResult<ushort> From(IConvertContext context, object input) => Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
        public ConvertResult<ushort> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(ushort))
            {
                return Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
            }
            return BitConverter.ToUInt16(input.Slice(sizeof(ushort)), 0);
        }
    }
}
