using System;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class ListConvertor<TList, TValue> : BaseConvertor<TList>, IFrom<string, TList>, IFrom<object, TList>
        where TList : IList<TValue>
    {
        public ConvertResult<TList> From(IConvertContext context, string input)
        {
            if (input is null)
            {
                return typeof(TList).IsValueType ? context.ConvertFail(this, input) : Ok(default);
            }

            if (string.IsNullOrEmpty(input))
            {
                return (TList)Activator.CreateInstance(typeof(TList)); ;
            }

            var arr = input.Split(context.Settings.StringSeparator, context.Settings.StringSplitOptions);
            if (arr is TList result)
            {
                return result;
            }
            return From(context, arr.GetEnumerator());
        }

        public ConvertResult<TList> From(IConvertContext context, object input)
        {
            if (input is null)
            {
                return typeof(TList).IsValueType ? context.ConvertFail(this, input) : Ok(default);
            }

            var enumerator = new KeyValueEnumerator<object, TValue>(context, input);

            if (enumerator.IsEmpty)
            {
                var result = context.Convert<TValue>(input);
                if (result.Success)
                {
                    var list1 = (TList)Activator.CreateInstance(typeof(TList).IsInterface ? typeof(List<TValue>) : typeof(TList));
                    list1.Add(result.Value);
                    return list1;
                }
                return context.ConvertFail(this, input);
            }

            var list = (TList)Activator.CreateInstance(typeof(TList).IsInterface ? typeof(List<TValue>) : typeof(TList));
            while (enumerator.MoveNext())
            {
                var result = enumerator.GetValue();
                if (!result.Success)
                {
                    var rs = ResourceStringManager.GetResource(context.Settings.CultureInfo);
                    var message = string.Format(rs.COLLECTION_ADD_FAIL, TypeFriendlyName, list.Count, enumerator.OriginalValue);
                    return new InvalidCastException(message, result.Exception);
                }
                list.Add(result.Value);
            }
            return list;
        }
    }
}
