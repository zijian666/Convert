using System.Collections;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class ArrayListConvertor<T> : AllowNullConvertor<T>, IFrom<string, T>, IFrom<object, T>
        where T : IList
    {

        public ConvertResult<T> From(IConvertContext context, string input)
        {
            if (input is null)
            {
                return Ok(default);
            }

            if (string.IsNullOrEmpty(input))
            {
                return (T)(object)new ArrayList();
            }

            var arr = input.Split(context.Settings.StringSeparator, context.Settings.StringSplitOptions);
            return (T)(object)new ArrayList(arr);
        }

        public ConvertResult<T> From(IConvertContext context, object input)
        {
            if (input is null)
            {
                return Ok(default);
            }
            var enumerator = new KeyValueEnumerator<object, object>(context, input);

            if (enumerator.IsEmpty)
            {
                return (T)(object)new ArrayList { input };
            }

            var list = new ArrayList();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.OriginalValue);
            }
            return (T)(object)list;

        }
    }
}
