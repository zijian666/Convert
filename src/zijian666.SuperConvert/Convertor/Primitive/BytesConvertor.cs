using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    /// <summary>
    /// <seealso cref="byte" /> 数组转换器
    /// </summary>
    public class BytesConvertor : BaseConvertor<byte[]>, IFromConvertible<byte[]>, IFrom<Guid, byte[]>
    {
        public ConvertResult<byte[]> From(IConvertContext context, string input) => input == null ? null : input.Length == 0 ? Array.Empty<byte>() : context.Settings.Encoding.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, bool input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, char input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, sbyte input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, byte input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, short input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, ushort input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, int input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, uint input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, long input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, ulong input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, float input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, double input) => BitConverter.GetBytes(input);
        public ConvertResult<byte[]> From(IConvertContext context, decimal input)
        {
            var arr = decimal.GetBits(input);
            var bytes = new byte[arr.Length << 2];
            Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public ConvertResult<byte[]> From(IConvertContext context, DateTime input)
            => Exceptions.ConvertFail(input, TypeFriendlyName, context.Settings.CultureInfo);

        public ConvertResult<byte[]> From(IConvertContext context, Guid input) => input.ToByteArray();


    }
}
