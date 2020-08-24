using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.Single;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="float"/> 转换器
    /// </summary>
    public class SingleConvertor : BaseConvertor<float>
                                , IFromConvertible<float>
                                , IFrom<object, float>
                                , IFrom<byte[], float>
    {
        public ConvertResult<float> From(IConvertContext context, bool input) => input ? (float)1 : (float)0;
        public ConvertResult<float> From(IConvertContext context, char input) => (float)input;
        public ConvertResult<float> From(IConvertContext context, sbyte input) => (float)input;
        public ConvertResult<float> From(IConvertContext context, byte input) => (float)input;
        public ConvertResult<float> From(IConvertContext context, short input) => (float)input;
        public ConvertResult<float> From(IConvertContext context, ushort input) => (float)input;
        public ConvertResult<float> From(IConvertContext context, int input) => (float)input;

        public ConvertResult<float> From(IConvertContext context, uint input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(IConvertContext context, long input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(IConvertContext context, ulong input)
        {
            if (input > MaxValue)
            {
                return Exceptions.Overflow($"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(IConvertContext context, float input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(IConvertContext context, double input)
        {
            if ((input < MinValue) || (input > MaxValue))
            {
                return Exceptions.Overflow(input < MinValue ? $"{input} < {MinValue}" : $"{input} > {MaxValue}", context.Settings.CultureInfo);
            }
            return (float)input;
        }
        public ConvertResult<float> From(IConvertContext context, decimal input) => decimal.ToSingle(input);
        public ConvertResult<float> From(IConvertContext context, DateTime input) => Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
        public ConvertResult<float> From(IConvertContext context, string input)
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
                            if (long.TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out var result0))
                            {
                                return result0;
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
            return Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
        }

        public ConvertResult<float> From(IConvertContext context, object input) => Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);

        public ConvertResult<float> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(float))
            {
                return Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
            }
            return BitConverter.ToSingle(input.Slice(sizeof(float)), 0);
        }
    }
}
