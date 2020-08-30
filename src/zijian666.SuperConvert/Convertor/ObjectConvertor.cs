using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class ObjectConvertor<T> : BaseConvertor<T>, IFrom<object, T>
    {
        public override uint Priority { get; } = 0;

        public ConvertResult<T> From(IConvertContext context, object input)
        {
            var enumerator = new KeyValueEnumerator<string, object>(context, input);
            if (!enumerator.HasStringKey)
            {
                return context.ConvertFail(this, input);
            }
            var obj = Activator.CreateInstance<T>();
            var properties = PropertyHelper.GetByType(OutputType);
            var c1 = 0;
            var c2 = 0;
            var rs = ResourceStringManager.GetResource(context.Settings.CultureInfo);
            while (enumerator.MoveNext())
            {
                c1++;
                var key = enumerator.GetKey();
                if (!key.Success)
                {
                    var message = string.Format(rs.PROPERTY_CAST_FAIL, TypeFriendlyName, enumerator.OriginalKey);
                    return new InvalidCastException(message, key.Exception);
                }

                if (!properties.TryGetValue(key.Value, out var prop))
                {
                    continue;
                }
                var value = context.Convert(prop.PropertyType, enumerator.OriginalValue);
                if (!value.Success)
                {
                    var message = string.Format(rs.PROPERTY_SET_FAIL, TypeFriendlyName, key.Value, enumerator.OriginalValue);
                    return new InvalidCastException(message, value.Exception);
                }
                c2++;
                prop.SetValue(obj, value.Value);
            }
            if (c1 > 0 && c2 == 0)
            {
                return context.ConvertFail(this, input);
            }
            return obj;

        }
    }
}
