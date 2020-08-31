using System.Collections.Generic;
using zijian666.SuperConvert.Core;

namespace zijian666.SuperConvert.Interface
{
    public interface IConvertorFactory
    {
        IEnumerable<MatchedConvertor<T>> Create<T>();
    }
}
