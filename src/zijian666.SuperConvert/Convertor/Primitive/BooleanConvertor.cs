using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// 布尔值转换器
    /// </summary>
    public class BooleanConvertor : FromConvertor<bool>, IFromConvertible<bool>, IFrom<object, bool>
    {
        public ConvertResult<bool> From(IConvertContext context, bool input) => input;
        public ConvertResult<bool> From(IConvertContext context, char input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, sbyte input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, byte input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, short input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, ushort input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, int input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, uint input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, long input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, ulong input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, float input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, double input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, decimal input) => input != 0;
        public ConvertResult<bool> From(IConvertContext context, string input)
        {
            var s = input?.Trim() ?? "";
            switch (s.Length)
            {
                case 1:
                    switch (s[0])
                    {
                        case '1':
                        case 'T':
                        case 't':
                        case '对':
                        case '對':
                        case '真':
                        case '是':
                        case '男':
                            return true;
                        case '0':
                        case 'F':
                        case 'f':
                        case '错':
                        case '錯':
                        case '假':
                        case '否':
                        case '女':
                            return false;
                        default:
                            break;
                    }
                    break;
                case 2:
                    if ((s[0] == '-') || (s[0] == '+'))
                    {
                        return s[1] != '0';
                    }
                    break;
                case 4:
                    if (s.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    break;
                case 5:
                    if (s.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return context.ConvertFail(this, input);
        }
        public ConvertResult<bool> From(IConvertContext context, object input)
            => context.ConvertFail(this, input);
        public ConvertResult<bool> From(IConvertContext context, DateTime input)
            => context.ConvertFail(this, input);
    }
}
