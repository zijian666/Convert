using System;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class ListConvertor<TList, TValue> : AllowNullConvertor<TList>, IFrom<string, TList>, IFrom<object, TList>, IFrom<object[], TList>
        where TList : IList<TValue>
    {
        public ConvertResult<TList> From(IConvertContext context, string input)
        {
            if (input is null)
            {
                return OutputType.IsValueType ? context.ConvertFail(this, input) : Ok(default);
            }

            if (string.IsNullOrEmpty(input))
            {
                return context.Settings.CreateInstance<TList>();
            }

            var arr = input.Split(context.Settings.StringSeparator, context.Settings.StringSplitOptions);
            if (arr is TList result)
            {
                return result;
            }
            return From(context, arr.GetEnumerator());
        }

        private TList CreateInstance(IConvertContext context) => OutputType.IsInterface
                ? (TList)context.Settings.CreateInstance(typeof(List<TValue>))
                : Activator.CreateInstance<TList>();

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
                    var list1 = CreateInstance(context);
                    list1.Add(result.Value);
                    return list1;
                }
                return context.ConvertFail(this, input);
            }

            var list = CreateInstance(context);
            while (enumerator.MoveNext())
            {
                var result = enumerator.GetValue();
                if (!result.Success)
                {
                    var rs = context.Settings.GetResourceStrings();
                    var message = string.Format(rs.COLLECTION_ADD_FAIL, TypeFriendlyName, list.Count, enumerator.OriginalValue);
                    return new InvalidCastException(message, result.Exception);
                }
                list.Add(result.Value);
            }
            return list;
        }

        public ConvertResult<TList> From(IConvertContext context, object[] input) => From(context, input.GetEnumerator());
    }
}
