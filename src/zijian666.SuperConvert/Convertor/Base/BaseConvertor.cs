using System;
using System.Linq;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    public abstract class BaseConvertor<T> : IConvertor<T>
    {
        protected BaseConvertor()
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

        ConvertResult<T> IConvertor<T>.Convert(IConvertContext context, object input)
        {
            if (input == null)
            {
                var result = Convert(context, input);
                if (result.Success && OutputType.IsEnum && context.Settings.StrictEnum)
                {
                    return CheckEnum(context, result.Value);
                }
                return result;
            }

            ExceptionCollection exceptions = null;

            {
                var result = Convert(context, input);
                if (result.Success)
                {
                    return OutputType.IsEnum && context.Settings.StrictEnum
                        ? CheckEnum(context, result.Value)
                        : result;
                }
                if (result.Exception != null)
                {
                    exceptions += result.Exception;
                }
            }

            var type = input.GetType();
            var values = context.Settings.Translators?.Where(x => x.CanTranslate(type)).Select(x => x.Translate(context, input));
            foreach (var value in values ?? Array.Empty<object>())
            {
                if (ReferenceEquals(value, input) || value == null)
                {
                    continue;
                }
                var result = Convert(context, value);
                if (result.Success)
                {
                    return OutputType.IsEnum && context.Settings.StrictEnum
                        ? CheckEnum(context, result.Value)
                        : result;
                }
                if (result.Exception != null)
                {
                    exceptions += result.Exception;
                }
            }


            var message = Exceptions.ConvertFailMessage(input, TypeFriendlyName, context.Settings.CultureInfo);
            if (exceptions == null)
            {
                return new InvalidCastException(message);
            }
            return exceptions.ToException(message);
        }


        public abstract ConvertResult<T> Convert(IConvertContext context, object input);

        protected ConvertResult<T> CheckEnum(IConvertContext context, T result)
        {
            if (!OutputType.IsEnum)
            {
                return result;
            }
            if (result.Equals(default(T)))
            {
                return result;
            }
            if (Enum.IsDefined(OutputType, result))
            {
                return result;
            }
            if (Attribute.IsDefined(OutputType, typeof(FlagsAttribute)))
            {
                if (result.ToString().IndexOf(',') >= 0)
                {
                    return result;
                }
            }

            var ex = new InvalidCastException("严格枚举模式下枚举值不能超过枚举定义范围");
            return context.ConvertFail(this, result, ex);
        }
    }
}
