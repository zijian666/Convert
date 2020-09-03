using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.SByte;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="sbyte"/> 转换器
    /// </summary>
    public class SBtyeConvertor : FromConvertor<sbyte>
                                , IFromConvertible<sbyte>
    {
        public ConvertResult<sbyte> From(IConvertContext context, bool input) => input ? (sbyte)1 : (sbyte)0;
        public ConvertResult<sbyte> From(IConvertContext context, char input) => (sbyte)input;
        public ConvertResult<sbyte> From(IConvertContext context, sbyte input) => (sbyte)input;
        public ConvertResult<sbyte> From(IConvertContext context, byte input) => (sbyte)input;
        public ConvertResult<sbyte> From(IConvertContext context, short input) => (sbyte)input;
        public ConvertResult<sbyte> From(IConvertContext context, ushort input) => (sbyte)input;
        public ConvertResult<sbyte> From(IConvertContext context, int input) => (sbyte)input;
        public ConvertResult<sbyte> From(IConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(IConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(IConvertContext context, ulong input)
        {
            if (input > (int)MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(IConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(IConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (sbyte)input;
        }
        public ConvertResult<sbyte> From(IConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return decimal.ToSByte(input);
        }
        public ConvertResult<sbyte> From(IConvertContext context, DateTime input) => context.ConvertFail(this, input);

        public ConvertResult<sbyte> From(IConvertContext context, string input)
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
                                return System.Convert.ToSByte(s.Substring(2), 2);
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
