using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;
using static System.Char;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="char" /> 转换器
    /// </summary>
    public class CharConvertor : BaseConvertor<char>, IFromConvertible<char>, IFrom<byte[], char>
    {
        public ConvertResult<char> From(IConvertContext context, bool input) => input ? 'Y' : 'N';
        public ConvertResult<char> From(IConvertContext context, char input) => input;
        public ConvertResult<char> From(IConvertContext context, sbyte input)
        {
            if (input < MinValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, byte input) => (char)input;
        public ConvertResult<char> From(IConvertContext context, short input)
        {
            if (input < MinValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, ushort input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, int input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, uint input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, long input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, ulong input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, float input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, double input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, decimal input)
        {
            if (input < MinValue || input > MaxValue)
            {
                return context.ConvertFail(this, input);
            }
            return (char)input;
        }
        public ConvertResult<char> From(IConvertContext context, DateTime input)
            => context.ConvertFail(this, input);

        public ConvertResult<char> From(IConvertContext context, string input)
        {
            if (input?.Length == 1)
            {
                return input[0];
            }
            return context.ConvertFail(this, input);
        }

        public ConvertResult<char> From(IConvertContext context, byte[] input)
        {
            if (input == null || input.Length > sizeof(char))
            {
                return context.ConvertFail(this, input);
            }
            return BitConverter.ToChar(input.Slice(sizeof(char)), 0);
        }
    }
}
