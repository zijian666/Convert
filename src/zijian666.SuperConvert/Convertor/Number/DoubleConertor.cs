using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.Double;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="double" /> 转换器
    /// </summary>
    public class DoubleConertor : BaseConvertor<double>, IFromConvertible<double>, IFrom<byte[], double>
    {
        public ConvertResult<double> From(IConvertContext context, bool input) => input ? 1 : 0;
        public ConvertResult<double> From(IConvertContext context, char input) => input;
        public ConvertResult<double> From(IConvertContext context, sbyte input) => input;
        public ConvertResult<double> From(IConvertContext context, byte input) => input;
        public ConvertResult<double> From(IConvertContext context, short input) => input;
        public ConvertResult<double> From(IConvertContext context, ushort input) => input;
        public ConvertResult<double> From(IConvertContext context, int input) => input;
        public ConvertResult<double> From(IConvertContext context, uint input) => input;
        public ConvertResult<double> From(IConvertContext context, long input) => input;
        public ConvertResult<double> From(IConvertContext context, ulong input) => input;
        public ConvertResult<double> From(IConvertContext context, float input) => input;
        public ConvertResult<double> From(IConvertContext context, double input) => input;
        public ConvertResult<double> From(IConvertContext context, decimal input) => decimal.ToDouble(input);
        public ConvertResult<double> From(IConvertContext context, DateTime input)
            => Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
        public ConvertResult<double> From(IConvertContext context, string input)
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
        public ConvertResult<double> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(double))
            {
                return Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);
            }
            return BitConverter.ToDouble(input.Slice(sizeof(double)), 0);
        }
    }
}
