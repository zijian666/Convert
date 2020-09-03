using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.Int32;

namespace zijian666.SuperConvert.Convertor
{
    class Int32Convertor : FromConvertor<int>, IFromConvertible<int>, IFrom<byte[], int>
    {
        public ConvertResult<int> From(IConvertContext context, bool input) => input ? 1 : 0;
        public ConvertResult<int> From(IConvertContext context, char input) => input;
        public ConvertResult<int> From(IConvertContext context, sbyte input) => input;
        public ConvertResult<int> From(IConvertContext context, byte input) => input;
        public ConvertResult<int> From(IConvertContext context, short input) => input;
        public ConvertResult<int> From(IConvertContext context, ushort input) => input;
        public ConvertResult<int> From(IConvertContext context, int input) => input;
        public ConvertResult<int> From(IConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (int)input;
        }

        public ConvertResult<int> From(IConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (int)input;
        }

        public ConvertResult<int> From(IConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (int)input;
        }
        public ConvertResult<int> From(IConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (int)input;
        }
        public ConvertResult<int> From(IConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (int)input;
        }
        public ConvertResult<int> From(IConvertContext context, decimal input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return decimal.ToInt32(input);
        }
        public ConvertResult<int> From(IConvertContext context, DateTime input) => context.ConvertFail(this, input);
        public ConvertResult<int> From(IConvertContext context, string input)
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
                                return System.Convert.ToInt32(s.Substring(2), 2);
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
        public ConvertResult<int> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(int))
            {
                return context.ConvertFail(this, input);
            }
            return BitConverter.ToInt32(input.Slice(sizeof(int)), 0);
        }
    }
}
