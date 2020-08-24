
using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    class UIntPtrConvertor : BaseConvertor<UIntPtr>
                                 , IFrom<uint, UIntPtr>
                                 , IFrom<ulong, UIntPtr>
                                 , IFrom<object, UIntPtr>
    {
        public ConvertResult<UIntPtr> From(IConvertContext context, ulong input) => new UIntPtr(input);
        public ConvertResult<UIntPtr> From(IConvertContext context, uint input) => new UIntPtr(input);
        public ConvertResult<UIntPtr> From(IConvertContext context, object input)
        {
            var result = context.Convert<ulong>(input);
            if (!result.Success)
            {
                return result.Exception;
            }
            return new UIntPtr(result.Value);
        }
    }
}
