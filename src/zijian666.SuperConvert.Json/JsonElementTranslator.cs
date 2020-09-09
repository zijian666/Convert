using System;
using System.Collections.Generic;
using System.Text.Json;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Json
{
    public class JsonElementTranslator : ITranslator
    {
        public bool CanTranslate(Type type) => type == typeof(JsonElement);
        public object Translate(IConvertContext context, object input)
        {
            var element = (JsonElement)input;
            switch (element.ValueKind)
            {

                case JsonValueKind.Array:
                    var list = new List<object>();
                    foreach (var item in element.EnumerateArray())
                    {
                        list.Add(Translate(context, item));
                    }
                    return list;
                case JsonValueKind.Object:
                    var dictionary = new Dictionary<string, object>();
                    foreach (var item in element.EnumerateObject())
                    {
                        dictionary.Add(item.Name, Translate(context, item.Value));
                    }
                    return dictionary;
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    return element.GetDecimal();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    return DBNull.Value;
                default:
                    return null;
            }
        }
    }
}
