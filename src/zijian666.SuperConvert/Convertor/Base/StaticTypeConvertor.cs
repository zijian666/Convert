using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    class StaticTypeConvertor<T> : Convertor<T>
    {
        public override ConvertResult<T> Convert(IConvertContext context, object input)
        {
            return Exceptions.StaticTypeConvertor(TypeFriendlyName, context.Settings.CultureInfo);
        }
    }
}
