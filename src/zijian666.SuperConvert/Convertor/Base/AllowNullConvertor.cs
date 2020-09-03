using System;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    public abstract class AllowNullConvertor<T> : FromConvertor<T>, IFromNull<T>
    {
        public ConvertResult<T> FromNull(IConvertContext context) => default;

        public ConvertResult<T> From(IConvertContext context, DBNull input) => default;
    }
}
