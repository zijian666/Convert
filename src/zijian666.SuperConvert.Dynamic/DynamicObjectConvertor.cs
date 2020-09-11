using System.Collections.Generic;
using System.Dynamic;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Dynamic
{
    public class DynamicObjectConvertor : AllowNullConvertor<DynamicObject>, IFrom<string, DynamicObject>, IFrom<object, DynamicObject>
    {
        public ConvertResult<DynamicObject> From(IConvertContext context, string input)
        {
            try
            {
                var array = context.Settings.StringSerializer?.ToObject(input, typeof(object[]));
                if (array != null)
                {
                    return From(context, array);
                }
            }
            catch
            {

            }

            var dict = context.Settings.StringSerializer?.ToObject(input, typeof(Dictionary<string, object>));
            if (dict == null)
            {
                return context.ConvertFail(this, input);
            }
            return From(context, dict);
        }

        public ConvertResult<DynamicObject> From(IConvertContext context, object input)
        {
            throw new System.NotImplementedException();
        }
    }
}
