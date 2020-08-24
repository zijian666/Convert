using System;
using System.Globalization;
using zijian666.SuperConvert.Extensions;

namespace zijian666.SuperConvert.Core
{
    static class Exceptions
    {
        public static string ConvertFailMessage<T>(T origin, string typeName, CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            var text = (origin as IConvertible)?.ToString(null)
                      ?? (origin as IFormattable)?.ToString(null, null);
            if (origin == null)
            {
                return string.Format(rs.CANT_CONVERT, "null", "null", typeName);
            }
            if (text == null)
            {
                return string.Format(rs.CANT_CONVERT, "", origin.GetType().GetFriendlyName(), typeName);
            }
            return string.Format(rs.CANT_CONVERT, text, origin.GetType().GetFriendlyName(), typeName);


        }

        public static Exception NotFountConvertor(CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            return new EntryPointNotFoundException(rs.NOT_FOUND_CONVERTOR);
        }

        public static Exception ConvertFail<T>(T origin, string typeName, CultureInfo cultureInfo)
        {
            var message = ConvertFailMessage(origin, typeName, cultureInfo);
            return new InvalidCastException(message);
        }
        public static Exception Overflow(string message, CultureInfo cultureInfo)
        {
            var rs = ResourceStringManager.GetResource(cultureInfo);
            return new OverflowException(string.Format(rs.VALUE_OVERFLOW, message));
        }
    }
}
