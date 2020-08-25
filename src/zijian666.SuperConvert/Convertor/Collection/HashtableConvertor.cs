using System.Collections;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class HashtableConvertor<T> : BaseConvertor<T>, IFrom<object, T>
        where T : IDictionary
    {
        public ConvertResult<T> From(IConvertContext context, object input)
        {
            var enumerator = new KeyValueEnumerator<object, object>(context, input);
            var hashtable = new Hashtable();
            while (enumerator.MoveNext())
            {
                hashtable.Add(enumerator.OriginalKey, enumerator.OriginalValue);
            }
            return (T)(object)hashtable;
        }
    }
}
