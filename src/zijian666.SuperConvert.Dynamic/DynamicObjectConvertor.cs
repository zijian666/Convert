using System.Collections.Generic;
using System.Dynamic;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Dynamic
{
    public class DynamicObjectConvertor : AllowNullConvertor<DynamicObject>
        , IFrom<object, DynamicObject>
        , IFrom<object[], DynamicObject>
        , IFrom<Dictionary<string, object>, DynamicObject>
    {
        public ConvertResult<DynamicObject> From(IConvertContext context, object input)
            => DynamicFactory.Create(input, context.Settings);

        public ConvertResult<DynamicObject> From(IConvertContext context, Dictionary<string, object> input)
            => DynamicFactory.Create(input, context.Settings);

        public ConvertResult<DynamicObject> From(IConvertContext context, object[] input)
            => DynamicFactory.Create(input, context.Settings);
    }
}
