using System;
using System.Collections;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Factory
{
    public class DictionaryConvertorFactory : IConvertorFactory
    {
        public IEnumerable<MatchedConvertor<T>> Create<T>()
        {
            var genericArgs = typeof(T).GetGenericArguments(typeof(IDictionary<,>));
            if (genericArgs != null)
            {
                var convertor = ByDictionary<T>(genericArgs);
                if (typeof(Dictionary<,>).MakeGenericType(genericArgs) == typeof(T))
                {
                    yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
                }
                else
                {
                    yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Subclass);
                }
                yield break;
            }

            if (typeof(T) == typeof(IDictionary))
            {
                var convertor = ByHashtable<T>();
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Subclass);
                yield break;
            }

            if (typeof(T) == typeof(Hashtable))
            {
                var convertor = ByHashtable<T>();
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
                yield break;
            }
        }

        private IConvertor<T> ByDictionary<T>(Type[] types)
        {
            return (IConvertor<T>)Activator.CreateInstance(
                typeof(DictionaryConvertor<,,>).MakeGenericType(typeof(T), types[0], types[1]));
        }

        private IConvertor<T> ByHashtable<T>()
        {
            return (IConvertor<T>)Activator.CreateInstance(typeof(HashtableConvertor<>).MakeGenericType(typeof(T)));
        }

        private IConvertor<T> Build<T>()
        {
            var genericArgs = typeof(T).GetGenericArguments(typeof(IDictionary<,>));
            if (genericArgs != null)
            {
                return (IConvertor<T>)Activator.CreateInstance(
                    typeof(DictionaryConvertor<,,>).MakeGenericType(typeof(T), genericArgs[0], genericArgs[1]));
            }
            if (typeof(T) == typeof(IDictionary) || typeof(T) == typeof(Hashtable))
            {
                return (IConvertor<T>)Activator.CreateInstance(typeof(HashtableConvertor<>).MakeGenericType(typeof(T)));
            }
            return null;
        }
    }
}
