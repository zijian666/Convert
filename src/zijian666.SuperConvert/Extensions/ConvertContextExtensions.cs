using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Extensions
{
    public static class ConvertContextExtensions
    {
        public static ConvertResult<T> Convert<T>(this IConvertContext context, object value)
        {
            var convertor = context.GetConvertor<T>();
            return convertor.Convert(context, value);
        }

        public static ConvertResult<object> Convert(this IConvertContext context, Type type, object value)
        {
            var convertor = ProxyConvertor<object>(context, type);
            return convertor.Convert(context, value);
        }

        private static IConvertor<T> ProxyConvertor<T>(IConvertContext context, Type type)
        {
            var getConvertor = typeof(IConvertSettings).GetMethod("GetConvertor").MakeGenericMethod(type);
            var convertor = getConvertor.Invoke(context.Settings, new[] { context });
            var proxyConvertor = typeof(ProxyConvertor<,>).MakeGenericType(type, typeof(T));
            return (IConvertor<T>)Activator.CreateInstance(proxyConvertor, convertor);
        }

        public static IConvertor<T> GetConvertor<T>(this IConvertContext context)
            => context?.Settings.GetConvertor<T>(context);

        public static Exception ConvertFail<T1, T2>(this IConvertContext context, Convertor<T1> convertor, T2 input, Exception e = null)
            => Exceptions.ConvertFail(input, convertor.TypeFriendlyName, context.Settings.CultureInfo, e);
    }
}
