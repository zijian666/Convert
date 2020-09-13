using System;
using System.Runtime.Serialization;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Translator
{
    public class ObjectReferenceTranslator : ITranslator
    {
        public bool CanTranslate(Type type)
            => typeof(IObjectReference).IsAssignableFrom(type);

        public object Translate(IConvertContext context, object input)
            => ((IObjectReference)input).GetRealObject(default);
    }
}
