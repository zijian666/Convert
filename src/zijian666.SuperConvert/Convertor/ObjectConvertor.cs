using System;
using System.Collections.Generic;
using System.Reflection;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class ObjectConvertor<T> : AllowNullConvertor<T>, IFrom<object, T>
    {
        private readonly Dictionary<Type, Func<object, T>> _casters;
        public ObjectConvertor()
        {
            var binding = BindingFlags.Static | BindingFlags.Public;
            var methods = OutputType.GetMethods(binding);
            _casters = new Dictionary<Type, Func<object, T>>();
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 1)
                {
                    continue;
                }

                var parameterType = parameters[0].ParameterType;
                if (_casters.ContainsKey(parameterType))
                {
                    continue;
                }

                if (method.Name == "op_Explicit" || method.Name == "op_Implicit")
                {
                    _casters[parameterType] = o => (T)method.Invoke(null, new object[] { o });
                }
            }
            if (_casters.Count == 0)
            {
                _casters = null;
            }

        }
        public override uint Priority => 0;

        public ConvertResult<T> From(IConvertContext context, object input)
        {
            if (_casters is null && _casters.TryGetValue(input.GetType(), out var caster))
            {
                return caster(input);
            }
            var enumerator = new KeyValueEnumerator<string, object>(context, input);
            if (!enumerator.HasStringKey)
            {
                return context.ConvertFail(this, input);
            }
            var obj = Activator.CreateInstance<T>();
            var properties = PropertyHelper.GetByType(OutputType);
            var c1 = 0;
            var c2 = 0;
            var rs = context.Settings.GetResourceStrings();
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
