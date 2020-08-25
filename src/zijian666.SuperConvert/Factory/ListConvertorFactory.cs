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
            if (typeof(T).IsArray)
            {
                var convertor = ByArray<T>();
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
                yield break;
            }
            var genericArgs = typeof(T).GetGenericArguments(typeof(IList<>));
            if (genericArgs != null)
            {
                var convertor = ByList<T>(genericArgs[0]);
                if (typeof(List<>).MakeGenericType(genericArgs[0]) == typeof(T))
                {
                    yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
                }
                else
                {
                    yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Subclass);
                }
                yield break;
            }

            if (typeof(T) == typeof(ArrayList))
            {
                var convertor = (IConvertor<T>)Activator.CreateInstance(typeof(ArrayListConvertor<>).MakeGenericType(typeof(T)));
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
                yield break;
            }

            if (typeof(T) == typeof(IList))
            {
                var convertor = (IConvertor<T>)Activator.CreateInstance(typeof(ArrayListConvertor<>).MakeGenericType(typeof(T)));
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Subclass);
                yield break;
            }
        }

        private IConvertor<T> ByArray<T>()
        {
            return (IConvertor<T>)Activator.CreateInstance(
                typeof(ArrayConvertor<>).MakeGenericType(typeof(T).GetElementType()));
        }

        private IConvertor<T> ByList<T>(Type type)
        {
            return (IConvertor<T>)Activator.CreateInstance(typeof(ListConvertor<,>).MakeGenericType(typeof(T), type));
        }

        private IConvertor<T> ByList<T>()
        {
            return (IConvertor<T>)Activator.CreateInstance(typeof(ArrayListConvertor<>).MakeGenericType(typeof(T)));
        }
    }
}
