using System;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Factory
{
    public sealed class InstantiatedConvertorFactory : IConvertorFactory
    {
        private readonly object _convertor;
        private readonly Type _type;

        internal InstantiatedConvertorFactory(object convertor, Type type)
        {
            _convertor = convertor;
            _type = type;
        }

        public static IConvertorFactory BuildFactory<T>(IConvertor<T> convertor)
        {
            if (convertor is null)
            {
                throw new ArgumentNullException(nameof(convertor));
            }
            return new InstantiatedConvertorFactory(convertor, typeof(T));
        }

        public IEnumerable<MatchedConvertor<T>> Create<T>()
        {
            if (_convertor is IConvertor<T> conv)
            {
                yield return new MatchedConvertor<T>(conv, conv.Priority, MacthedLevel.Full);
            }
            else if (_type.IsAssignableFrom(typeof(T)))
            {
                var proxyType = typeof(ProxyConvertor<,>).MakeGenericType(_type, typeof(T));
                var proxy = (IConvertor<T>)Activator.CreateInstance(proxyType, _convertor);
                yield return new MatchedConvertor<T>(proxy, proxy.Priority, MacthedLevel.Subclass);
            }
        }

    }
}
