using System;
using System.Globalization;
using zijian666.SuperConvert.Extensions;

namespace zijian666.SuperConvert.Core
{
    static class Exceptions
    {
        public static string ConvertFailMessage<T>(T input, string typeName, CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            var text = (input as IConvertible)?.ToString(null)
                      ?? (input as IFormattable)?.ToString(null, null);
            if (input == null)
            {
                return string.Format(rs.CANT_CONVERT, "null", "null", typeName);
            }
            if (text == null)
            {
                return string.Format(rs.CANT_CONVERT, "", typeof(T).GetFriendlyName(), typeName);
            }
            return string.Format(rs.CANT_CONVERT, text, typeof(T).GetFriendlyName(), typeName);
        }

        public static Exception GenericTypeDefinitionConvertor(string typeName, CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            return new NotSupportedException(string.Format(rs.CANT_BUILD_CONVERTOR_BECAUSE_GENERIC_DEFINITION_TYPE, typeName));
        }

        public static Exception StaticTypeConvertor(string typeName, CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            return new NotSupportedException(string.Format(rs.CANT_BUILD_CONVERTOR_BECAUSE_STATIC_TYPE, typeName));
        }

        public static Exception NotFountConvertor(string typeName, CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            return new EntryPointNotFoundException(string.Format(rs.CANT_BUILD_CONVERTOR_BECAUSE_NOTFOUND, typeName));
        }

        public static Exception ConvertFail<T>(T input, string typeName, CultureInfo cultureInfo, Exception e = null)
        {
            var message = ConvertFailMessage(input, typeName, cultureInfo);
            return new InvalidCastException(message, e);
        }
        public static Exception Overflow(string message, CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            return new OverflowException(string.Format(rs.VALUE_OVERFLOW, message));
        }
    }
}
