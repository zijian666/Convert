using System.Collections.Generic;
using zijian666.SuperConvert.Convertor;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Factory
{
    public class AnonymousTypeConvertorFactory : IConvertorFactory
    {
        public IEnumerable<MatchedConvertor<T>> Create<T>()
        {
            if (!typeof(T).IsArray && typeof(T).Name.StartsWith("<>f__AnonymousType"))
            {
                var convertor = new AnonymousTypeConvertor<T>();
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
            }
        }
    }
}
