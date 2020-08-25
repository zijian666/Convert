using System;
using System.Collections;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Factory
{
    sealed class ListConvertorFactory : IConvertorFactory
    {
        public IEnumerable<MatchedConvertor<T>> Create<T>()
        {
            var convertor = Build<T>();
            if (convertor != null)
            {
                yield return new MatchedConvertor<T>(convertor, 1, MacthedLevel.Full);
            }
        }

        private IConvertor<T> Build<T>()
        {
            if (typeof(T).IsArray)
            {
                return (IConvertor<T>)Activator.CreateInstance(typeof(ArrayConvertor<>).MakeGenericType(typeof(T).GetElementType()));
            }
            var genericArgs = typeof(T).GetGenericArguments(typeof(IList<>));

            if (genericArgs != null)
            {
                return (IConvertor<T>)Activator.CreateInstance(typeof(ListConvertor<,>).MakeGenericType(typeof(T), genericArgs[0]));
            }

            // 如果无法得道泛型参数, 判断output是否与 List<object> 兼容, 如果是返回 List<object> 的转换器
            if (typeof(T) == typeof(ArrayList) || typeof(T) == typeof(IList))
            {
                return (IConvertor<T>)Activator.CreateInstance(typeof(ArrayListConvertor<>).MakeGenericType(typeof(T)));
            }
            return null;
        }
    }
}
