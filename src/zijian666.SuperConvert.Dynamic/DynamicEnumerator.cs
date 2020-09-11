using System.Collections;
using System.Runtime.Serialization;
using zijian666.SuperConvert.Interface;

//using System.Runtime.Remoting;

namespace zijian666.SuperConvert.Dynamic
{
    internal class DynamicEnumerator : DynamicEntity, IObjectReference, IEnumerator//, IObjectHandle, ICustomTypeProvider
    {
        private readonly IEnumerator _enumerator;

        public DynamicEnumerator(IEnumerator enumerator, IConvertSettings convertSettings)
            : base(enumerator, convertSettings)
        {
            _enumerator = enumerator;
        }

        public object Current => WrapToDynamic(_enumerator.Current);

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();

    }
}
