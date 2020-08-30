using System;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    class GenericTypeDefinitionConvertor<T> : Convertor<T>
    {
        public override string TypeFriendlyName { get; }
        public GenericTypeDefinitionConvertor(Type type) => TypeFriendlyName = type.GetFriendlyName();

        public override ConvertResult<T> Convert(IConvertContext context, object input)
        {
            return Exceptions.GenericTypeDefinitionConvertor(TypeFriendlyName, context.Settings.CultureInfo);
        }
    }
}
