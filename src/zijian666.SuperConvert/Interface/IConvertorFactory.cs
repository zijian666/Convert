using System.Collections.Generic;

namespace zijian666.SuperConvert.Interface
{
    public interface IConvertorFactory
    {
        IEnumerable<MatchedConvertor<T>> Create<T>();
    }
}
