using System;
using System.Linq;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class AnonymousTypeConvertor<T> :
        AllowNullConvertor<T>, IFrom<object, T>
    {
        public ConvertResult<T> From(IConvertContext context, object input)
        {
            var enumerator = new KeyValueEnumerator<string, object>(context, input);
            var constructor = OutputType.GetConstructors()[0];
            var parameters = constructor.GetParameters();
            var arguments = new object[parameters.Length];


            //FormatterServices.GetUninitializedObject()
            var rs = context.Settings.GetResourceStrings();
            while (enumerator.MoveNext())
            {
                var key = enumerator.GetKey();
                if (!key.Success)
                {
                    var message = string.Format(rs.COLLECTION_KEY_FAIL, TypeFriendlyName, enumerator.OriginalKey);
                    return new InvalidCastException(message, key.Exception);
                }

                var parameter = parameters.FirstOrDefault(x => x.Name.Equals(key.Value, StringComparison.OrdinalIgnoreCase));
                if (parameter == null)
                {
                    continue;
                }

                var value = context.Convert(parameter.ParameterType, enumerator.OriginalValue);
                if (!value.Success)
                {
                    var message = string.Format(rs.COLLECTION_ADD_FAIL, TypeFriendlyName, key.Value, enumerator.OriginalValue);
                    return new InvalidCastException(message, key.Exception);
                }

                arguments[parameter.Position] = value.Value;
            }

            for (var i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == null)
                {
                    arguments[i] = parameters[i].ParameterType.GetDefault();
                }
            }

            return (T)constructor.Invoke(arguments);
        }
    }
}
