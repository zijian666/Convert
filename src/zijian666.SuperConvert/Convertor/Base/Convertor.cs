using System;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    public abstract class Convertor<T> : IConvertor<T>
    {
        protected Convertor()
        {
            OutputType = typeof(T);
            TypeName = OutputType.FullName;
            TypeFriendlyName = OutputType.GetFriendlyName();
        }
        /// <summary>
        /// 输出类型
        /// </summary>
        public virtual Type OutputType { get; }

        /// <summary>
        /// <seealso cref="OutputType"/>.FullName
        /// </summary>
        public virtual string TypeName { get; }

        /// <summary>
        /// <seealso cref="OutputType"/>的友好名称
        /// </summary>
        public virtual string TypeFriendlyName { get; }

        public virtual uint Priority { get; } = 1;

        public abstract ConvertResult<T> Convert(IConvertContext context, object input);
    }
}
