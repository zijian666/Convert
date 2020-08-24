using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Extensions
{
    public static class ConvertContextExtensions
    {
        public static ConvertResult<T> Convert<T>(this IConvertContext context, object value)
        {
            var convertor = context.Settings.GetConvertor<T>(context);
            return convertor.Convert(context, value);
        }
    }
}
