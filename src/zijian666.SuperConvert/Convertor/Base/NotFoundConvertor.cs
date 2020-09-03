using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    class NotFoundConvertor<T> : BaseConvertor<T>
    {

        public override ConvertResult<T> Convert(IConvertContext context, object input)
        {
            return Exceptions.NotFountConvertor(TypeFriendlyName, context.Settings.CultureInfo);
        }
    }
}
