using System;
using zijian666.SuperConvert.Extensions;

namespace zijian666.SuperConvert.Core
{
    class Exceptions
    {
        public static string ConvertFail(object origin, Type type)
        {
            var outputTypeName = type.GetFriendlyName();
            var rs = ResourceStringManager.GetResource();
            var text = (origin as IConvertible)?.ToString(null)
                      ?? (origin as IFormattable)?.ToString(null, null);
            if (origin == null)
            {
                return string.Format(rs.CANT_CONVERT, "null", "null", outputTypeName);
            }
            if (text == null)
            {
                return string.Format(rs.CANT_CONVERT, "", origin.GetType().GetFriendlyName(), outputTypeName);
            }
            return string.Format(rs.CANT_CONVERT, text, origin.GetType().GetFriendlyName(), outputTypeName);


        }
    }
}
