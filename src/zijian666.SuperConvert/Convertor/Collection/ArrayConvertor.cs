using System;
using System.Collections;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{

    public class ArrayConvertor<T> : BaseConvertor<T[]>, IFrom<string, T[]>, IFrom<object, T[]>
    {
        public ConvertResult<T[]> From(IConvertContext context, string input)
        {
            if (input is null)
            {
                return Ok(null);
            }
            if (string.IsNullOrEmpty(input))
            {
                return Array.Empty<T>();
            }

            var arr = input.Split(context.Settings.StringSeparator, context.Settings.StringSplitOptions);
            if (arr is T[] result)
            {
                return result;
            }
            return From(context, arr.GetEnumerator());
        }

        public ConvertResult<T[]> From(IConvertContext context, IEnumerator input)
        {
            if (input is null)
            {
                return Ok(null);
            }
            var result = context.Convert<List<T>>(input);
            if (result.Success)
            {
                return result.Value.ToArray();
            }
            return context.ConvertFail(this, input, result.Exception);
        }

        public ConvertResult<T[]> From(IConvertContext context, object input)
        {
            if (input is null)
            {
                return Ok(default);
            }

            var enumerator = new KeyValueEnumerator<object, T>(context, input);

            if (enumerator.IsEmpty)
            {
                return context.ConvertFail(this, input);
            }

            var list = new List<T>();
            while (enumerator.MoveNext())
            {
                var result = enumerator.GetValue();
                if (!result.Success)
                {
                    var rs = ResourceStringManager.GetResource(context.Settings.CultureInfo);
                    var message = string.Format(rs.COLLECTION_ADD_FAIL, $"List<{TypeFriendlyName}>", list.Count, enumerator.OriginalValue);
                    return new InvalidCastException(message, result.Exception);
                }
                list.Add(result.Value);
            }
            return list.ToArray();
        }
    }
}
