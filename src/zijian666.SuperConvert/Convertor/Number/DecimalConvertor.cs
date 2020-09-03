using System;
using System.Globalization;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.Decimal;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="decimal" /> 转换器
    /// </summary>
    public class DecimalConvertor : FromConvertor<decimal>, IFromConvertible<decimal>, IFrom<byte[], decimal>, IFrom<Guid, decimal>
    {
        public ConvertResult<decimal> From(IConvertContext context, bool input) => input ? One : Zero;
        public ConvertResult<decimal> From(IConvertContext context, char input) => input;
        public ConvertResult<decimal> From(IConvertContext context, sbyte input) => input;
        public ConvertResult<decimal> From(IConvertContext context, byte input) => input;
        public ConvertResult<decimal> From(IConvertContext context, short input) => input;
        public ConvertResult<decimal> From(IConvertContext context, ushort input) => input;
        public ConvertResult<decimal> From(IConvertContext context, int input) => input;
        public ConvertResult<decimal> From(IConvertContext context, uint input) => input;
        public ConvertResult<decimal> From(IConvertContext context, long input) => input;
        public ConvertResult<decimal> From(IConvertContext context, ulong input) => input;
        public ConvertResult<decimal> From(IConvertContext context, float input) => (decimal)input;
        public ConvertResult<decimal> From(IConvertContext context, double input) => (decimal)input;
        public ConvertResult<decimal> From(IConvertContext context, decimal input) => input;
        public ConvertResult<decimal> From(IConvertContext context, DateTime input)
            => context.ConvertFail(this, input);

        public ConvertResult<decimal> From(IConvertContext context, string input)
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
            return context.ConvertFail(this, input);
        }

        public ConvertResult<decimal> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(decimal))
            {
                return context.ConvertFail(this, input);
            }
            var bytes = input.Slice(sizeof(decimal));
            var arr2 = new int[4];
            Buffer.BlockCopy(bytes, 0, arr2, 0, sizeof(decimal));
            return new decimal(arr2);
        }
        public ConvertResult<decimal> From(IConvertContext context, Guid input)
        {
            var bytes = input.ToByteArray();
            var arr2 = new int[4];
            Buffer.BlockCopy(bytes, 0, arr2, 0, sizeof(decimal));
            return new decimal(arr2);
        }
    }
}
