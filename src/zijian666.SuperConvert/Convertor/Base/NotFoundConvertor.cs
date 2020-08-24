using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    class NotFoundConvertor<T> : IConvertor<T>
    {
        public uint Priority => 1;

        public ConvertResult<T> Convert(IConvertContext context, object input)
        {
            return Exceptions.NotFountConvertor(context.Settings.CultureInfo);
        }
    }
}
