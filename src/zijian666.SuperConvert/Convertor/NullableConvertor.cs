using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class NullableConvertor<TValue> : FromConvertor<TValue?>, IConvertor<TValue?>
        where TValue : struct
    {
        public override ConvertResult<TValue?> Convert(IConvertContext context, object input)
        {
            if (input is null || input is DBNull || (input is string s && string.IsNullOrWhiteSpace(s)))
            {
                return new ConvertResult<TValue?>(null);
            }
            var result = context.Convert<TValue>(input);
            return result.Success ? (ConvertResult<TValue?>)result.Value : result.Exception;
        }

    }
}
