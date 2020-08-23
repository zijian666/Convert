using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Core
{
    public abstract class ConvertorWrapper<T>
    {
        public IConvertor<T> InnerConvertor { get; }

        public ConvertorWrapper(IConvertor<T> convertor, bool unwrapper = true)
        {
            if (convertor is null)
            {
                throw new System.ArgumentNullException(nameof(convertor));
            }
            if (unwrapper && convertor is ConvertorWrapper<T> wrapper)
            {
                InnerConvertor = wrapper.InnerConvertor;
            }
            else
            {
                InnerConvertor = convertor;
            }
        }
    }
}
