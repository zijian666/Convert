using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using zijian666.SuperConvert.Convertor;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Factory
{
    public class NameValueCollectionConvertorFactory : IConvertorFactory
    {
        public IEnumerable<MatchedConvertor<T>> Create<T>()
        {
            if (typeof(NameValueCollection).IsAssignableFrom(typeof(T)))
            {
                var type = typeof(NameValueCollectionConvertor<>).MakeGenericType(typeof(T));
                var convertor = (IConvertor<T>)Activator.CreateInstance(type);
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
            }
        }
    }
}
