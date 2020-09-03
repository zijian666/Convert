using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.UInt32;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="uint"/> 转换器
    /// </summary>
    public class UInt32Convertor : FromConvertor<uint>
                                , IFromConvertible<uint>
                                , IFrom<byte[], uint>
    {
        public ConvertResult<uint> From(IConvertContext context, bool input) => input ? (uint)1 : (uint)0;
        public ConvertResult<uint> From(IConvertContext context, char input) => (uint)input;
        public ConvertResult<uint> From(IConvertContext context, sbyte input) => (uint)input;
        public ConvertResult<uint> From(IConvertContext context, byte input) => (uint)input;
        public ConvertResult<uint> From(IConvertContext context, short input) => (uint)input;
        public ConvertResult<uint> From(IConvertContext context, ushort input) => (uint)input;
        public ConvertResult<uint> From(IConvertContext context, int input) => (uint)input;
        public ConvertResult<uint> From(IConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(IConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(IConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(IConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(IConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (uint)input;
        }
        public ConvertResult<uint> From(IConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return decimal.ToUInt32(input);
        }
        public ConvertResult<uint> From(IConvertContext context, DateTime input) => context.ConvertFail(this, input);
        public ConvertResult<uint> From(IConvertContext context, string input)
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
                                return System.Convert.ToUInt32(s.Substring(2), 2);
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
        public ConvertResult<uint> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(uint))
            {
                return context.ConvertFail(this, input);
            }
            return BitConverter.ToUInt32(input.Slice(sizeof(uint)), 0);
        }
    }
}
