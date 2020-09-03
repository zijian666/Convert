using System;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class DictionaryConvertor<TDictionary, TKey, TValue> : AllowNullConvertor<TDictionary>, IFrom<object, TDictionary>
        where TDictionary : class, IDictionary<TKey, TValue>
    {
        public ConvertResult<TDictionary> From(IConvertContext context, object input)
        {
            var enumerator = new KeyValueEnumerator<TKey, TValue>(context, input);
            var dict = (TDictionary)Activator.CreateInstance(
                typeof(TDictionary).IsInterface ? typeof(Dictionary<TKey, TValue>) : typeof(TDictionary));
            var rs = context.Settings.GetResourceStrings();
            while (enumerator.MoveNext())
            {
                var key = enumerator.GetKey();
                if (!key.Success)
                {
                    var message = string.Format(rs.COLLECTION_KEY_FAIL, TypeFriendlyName, enumerator.OriginalKey);
                    return new InvalidCastException(message, key.Exception);
                }

                var value = enumerator.GetValue();
                if (!value.Success)
                {
                    var message = string.Format(rs.COLLECTION_ADD_FAIL, TypeFriendlyName, key.Value, enumerator.OriginalValue);
                    return new InvalidCastException(message, key.Exception);
                }

                dict.Add(key.Value, value.Value);
            }
            return dict;
        }
    }
}
