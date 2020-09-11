using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class NameValueCollectionConvertor<T> : AllowNullConvertor<T>, IFrom<object, T>, IFrom<Dictionary<string, object>, T>
        where T : NameValueCollection
    {
        public ConvertResult<T> From(IConvertContext context, object input)
        {
            var enumerator = new KeyValueEnumerator<string, string>(context, input);
            if (!enumerator.HasStringKey)
            {
                return context.ConvertFail(this, input);
            }
            var collection = context.Settings.CreateInstance<T>();
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

                collection.Add(key.Value, value.Value);
            }
            return collection;
        }

        public ConvertResult<T> From(IConvertContext context, Dictionary<string, object> input) => From(context, (object)input);
    }
}
