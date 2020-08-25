using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.Byte;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="byte" /> 转换器
    /// </summary>
    public class ByteConvertor : BaseConvertor<byte>, IFromConvertible<byte>
    {
        public ConvertResult<byte> From(IConvertContext context, bool input) => input ? (byte)1 : (byte)0;
        public ConvertResult<byte> From(IConvertContext context, char input) => (byte)input;
        public ConvertResult<byte> From(IConvertContext context, sbyte input)
        {
            if (input < MinValue)
            {
                return Exceptions.Overflow($"{input} < {MinValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, byte input) => input;
        public ConvertResult<byte> From(IConvertContext context, short input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, ushort input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, int input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, uint input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, long input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, ulong input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, float input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, double input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, decimal input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (byte)input;
        }
        public ConvertResult<byte> From(IConvertContext context, DateTime input)
        {
            return context.ConvertFail(this, input);
        }
        public ConvertResult<byte> From(IConvertContext context, string input)
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
                                return System.Convert.ToByte(s.Substring(2), 2);
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
    }
}
