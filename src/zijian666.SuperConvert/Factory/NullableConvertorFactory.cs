using System;
using System.Collections.Generic;
using zijian666.SuperConvert.Convertor;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Factory
{
    public class NullableConvertorFactory : IConvertorFactory
    {

        public IEnumerable<MatchedConvertor<T>> Create<T>()
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            if (underlyingType == null)
            {
                yield break;
            }

            var instance = (IConvertor<T>)Activator.CreateInstance(typeof(NullableConvertor<>).MakeGenericType(underlyingType));
            yield return new MatchedConvertor<T>(instance, 1, MacthedLevel.Full);
        }
    }
}
