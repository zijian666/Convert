
using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class IntPtrConvertor : BaseConvertor<IntPtr>
                                 , IFrom<int, IntPtr>
                                 , IFrom<long, IntPtr>
                                 , IFrom<object, IntPtr>
    {
        public ConvertResult<IntPtr> From(IConvertContext context, long input) => new IntPtr(input);
        public ConvertResult<IntPtr> From(IConvertContext context, int input) => new IntPtr(input);
        public ConvertResult<IntPtr> From(IConvertContext context, object input)
        {
            var result = context.Convert<long>(input);
            if (!result.Success)
            {
                return result.Exception;
            }
            return new IntPtr(result.Value);
        }
    }
}
