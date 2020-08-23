using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    /// <summary>
    /// 代理转换器
    /// </summary>
    /// <remarks>
    /// 转换器输出类型可兼容,但不可直接协变时, 可使用代理方式执行
    /// </remarks>
    public sealed class ProxyConvertor<TProxy, TOutput> : ConvertorWrapper<TProxy>, IConvertor<TOutput>
    {
        public ProxyConvertor(IConvertor<TProxy> convertor) : base(convertor)
        {
        }

        public uint Priority => InnerConvertor.Priority;

        public ConvertResult<TOutput> Convert(IConvertContext context, object input)
        {
            var result = InnerConvertor.Convert(context, input);
            if (!result.Success)
            {
                return result.Exception;
            }
            return new ConvertResult<TOutput>((TOutput)(object)result.Value);
        }
    }
}
