using System;
using System.Collections.Generic;
using System.Linq;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    /// <summary>
    /// 转换器基类
    /// </summary>
    /// <typeparam name="T">输出类型</typeparam>
    public abstract class FromConvertor<T> : BaseConvertor<T>, IConvertor<T>
    {
        /// <summary>
        /// 调用器字典
        /// </summary>
        private readonly Dictionary<Type, InvokeIFormHandler> _invokers;
        private readonly Type[] _invokerTypes;

        /// <summary>
        /// 构造函数, 根据实际类型的 <seealso cref="IFrom{TOutput, TInput}"/> 接口情况, 按照 TInput 类型缓存调用器
        /// </summary>
        //public BaseConvertor()
        //    : this(typeof(T))
        //{
        //}

        protected FromConvertor()
        {
            _invokers = InitInvokers();
            _invokerTypes = _invokers.Keys.ToArray();
            Array.Sort(_invokerTypes, TypeComparer.Instance);
        }




        private ConvertResult<T> TryFrom(IConvertContext context, object input, bool translation, ref ExceptionCollection exceptions)
        {
            //空值转换
            if (input == null)
            {
                try
                {
                    if (this is IFromNull<T> conv)
                    {
                        var result = conv.FromNull(context);
                        if (result.Success)
                        {
                            return result;
                        }
                        exceptions += result.Exception;
                    }
                }
                catch (Exception e)
                {
                    exceptions += e;
                }

                return ConvertResult<T>.NULL;
            }

            // 精确匹配

            var invoker0 = GetInvoker(input.GetType());
            if (invoker0 != null)
            {
                var result = invoker0(this, context, input);
                if (result.Success)
                {
                    return result;
                }
                exceptions += result.Exception;
            }


            //获取指定输入类型的转换方法调用器
            var invokers = GetInvokers(input.GetType());
            foreach (var invoker in invokers)
            {
                if (invoker == invoker0)
                {
                    continue;
                }
                var result = invoker(this, context, input);
                if (result.Success)
                {
                    return result;
                }
                //如果异常,下面还可以尝试其他方案
                exceptions += result.Exception;
            }

            if (translation)
            {
                var type = input.GetType();
                var values = context.Settings.Translators?.Where(x => x.CanTranslate(type)).Select(x => x.Translate(context, input));
                foreach (var value in values ?? Array.Empty<object>())
                {
                    if (input != value)
                    {
                        if (value is T t)
                        {
                            return Ok(t);
                        }
                        var result = TryFrom(context, value, false, ref exceptions);
                        if (result.Success)
                        {
                            return result;
                        }
                        //如果异常,下面还可以尝试其他方案
                        exceptions += result.Exception;
                    }
                }
            }


            return ConvertResult<T>.NULL;

        }


        private ConvertResult<T> TryStringSerializer(IConvertContext context, object input, ref ExceptionCollection exceptions)
        {
            //字符串类型的序列化器
            if (input is string str)
            {
                var serializer = context.Settings.StringSerializer;
                if (serializer != null)
                {
                    try
                    {
                        return (T)serializer.ToObject(str, typeof(T));
                    }
                    catch (Exception ex)
                    {
                        exceptions += ex;
                    }
                }
            }
            else if (typeof(T) == typeof(string))
            {
                var serializer = context.Settings.StringSerializer;
                if (serializer != null)
                {
                    try
                    {
                        return (T)(object)serializer.ToString(input);
                    }
                    catch (Exception ex)
                    {
                        exceptions += ex;
                    }
                }
            }
            return ConvertResult<T>.NULL;
        }
        /// <summary>
        /// 转换转换方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public override ConvertResult<T> Convert(IConvertContext context, object input)
        {
            if (input is T t)
            {
                return Ok(t);
            }

            ExceptionCollection exceptions = null;
            var result = TryStringSerializer(context, input, ref exceptions);
            if (result.Success)
            {
                return result;
            }
            result = TryFrom(context, input, true, ref exceptions);
            if (result.Success)
            {
                return result;
            }

            result = TryConvertible(context, input, ref exceptions);
            if (result.Success)
            {
                return result;
            }

            var message = Exceptions.ConvertFailMessage(input, TypeFriendlyName, context.Settings.CultureInfo);
            if (exceptions == null)
            {
                return new InvalidCastException(message);
            }
            return exceptions.ToException(message);
        }

        private ConvertResult<T> TryConvertible(IConvertContext context, object input, ref ExceptionCollection exceptions)
        {
            var fail = ConvertResult<T>.NULL;
            // 转为各种基本类型进行转换, 这里也可以使用翻译器来做, 但是考虑值类型装箱拆箱引起的性能问题, 决定写死
            if (input is IConvertible v0)
            {
                switch (v0.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return v0 is bool ? fail : TryFromGeneric(context, v0.ToBoolean(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Byte:
                        return v0 is byte ? fail : TryFromGeneric(context, v0.ToByte(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Char:
                        return v0 is char ? fail : TryFromGeneric(context, v0.ToChar(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.DateTime:
                        return v0 is DateTime ? fail : TryFromGeneric(context, v0.ToDateTime(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Decimal:
                        return v0 is decimal ? fail : TryFromGeneric(context, v0.ToDecimal(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Double:
                        return v0 is double ? fail : TryFromGeneric(context, v0.ToDouble(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Int16:
                        return v0 is short ? fail : TryFromGeneric(context, v0.ToInt16(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Int32:
                        return v0 is int ? fail : TryFromGeneric(context, v0.ToInt32(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Int64:
                        return v0 is long ? fail : TryFromGeneric(context, v0.ToInt64(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.SByte:
                        return v0 is sbyte ? fail : TryFromGeneric(context, v0.ToSByte(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.Single:
                        return v0 is float ? fail : TryFromGeneric(context, v0.ToSingle(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.UInt16:
                        return v0 is ushort ? fail : TryFromGeneric(context, v0.ToUInt16(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.UInt32:
                        return v0 is uint ? fail : TryFromGeneric(context, v0.ToUInt32(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.UInt64:
                        return v0 is ulong ? fail : TryFromGeneric(context, v0.ToUInt64(context.Settings.CultureInfo), ref exceptions);
                    case TypeCode.DBNull:
                        return InvokeIForm(context, DBNull.Value, exceptions); ;
                    case TypeCode.String:
                        var s = v0.ToString(context.Settings.CultureInfo);
                        if (s == input as string)
                        {
                            return fail;
                        }
                        return InvokeIForm(context, s, exceptions);
                    default:
                        return fail;
                }
            }
            return fail;
        }


        private ConvertResult<T> TryFromGeneric<TInput>(IConvertContext context, TInput input, ref ExceptionCollection exceptions)
        {
            try
            {
                if (this is IFrom<TInput, T> from)
                {
                    var result = from.From(context, input);
                    if (result.Success)
                    {
                        return result;
                    }
                    exceptions += result.Exception;
                }
            }
            catch (Exception e)
            {
                exceptions += e;
            }
            return ConvertResult<T>.NULL;
        }

        private readonly uint priority = 1;

        /// <summary>
        /// 优先级 默认0
        /// </summary>
        public override uint Priority => priority;

        protected ConvertResult<T> Ok(T value) => new ConvertResult<T>(value);

        private Dictionary<Type, InvokeIFormHandler> InitInvokers()
        {
            var invokers = new Dictionary<Type, InvokeIFormHandler>();
            var method = ((Func<InvokeIFormHandler>)CreateInvoker<object>).Method.GetGenericMethodDefinition();
            foreach (var intf in GetType().GetInterfaces())
            {
                if (intf.IsConstructedGenericType && intf.GetGenericTypeDefinition() == typeof(IFrom<,>))
                {
                    var args = intf.GetGenericArguments();
                    if (args[1] == typeof(T))
                    {
                        var invoker = (InvokeIFormHandler)method.MakeGenericMethod(args[0]).Invoke(null, null);
                        invokers.Add(args[0], invoker);
                    }
                }
            }
            return invokers;
        }

        /// <summary>
        /// 精确匹配调度器
        /// </summary>
        protected InvokeIFormHandler GetInvoker(Type inputType)
        {
            if (_invokers.TryGetValue(inputType, out var invoker0))
            {
                return invoker0;
            }
            return null;
        }

        /// <summary>
        /// 获取指定输入类型的调用器
        /// </summary>
        /// <param name="inputType">指定输入类型</param>
        /// <returns></returns>
        protected IEnumerable<InvokeIFormHandler> GetInvokers(Type inputType)
        {
            if (_invokers.Count == 0)
            {
                yield break;
            }
            var invokers = _invokers;

            foreach (var type in _invokerTypes)
            {
                if (type.IsAssignableFrom(inputType))
                {
                    yield return invokers[type];
                }
            }
        }


        protected delegate ConvertResult<T> InvokeIFormHandler(FromConvertor<T> convertor, IConvertContext context, object input);

        private static InvokeIFormHandler CreateInvoker<TInput>()
            => (convertor, context, input) =>
            {
                try
                {
                    return ((IFrom<TInput, T>)convertor).From(context, (TInput)input);
                }
                catch (Exception e)
                {
                    return e;
                }
            };

        /// <summary>
        /// 调用转换方法
        /// </summary>
        /// <typeparam name="TInput">输入类型</typeparam>
        /// <param name="conv">转换器</param>
        /// <param name="context">转换上下文</param>
        /// <param name="input">输入值</param>
        /// <returns></returns>
        protected ConvertResult<T> InvokeIForm<TInput>(IConvertContext context, TInput input, ExceptionCollection exceptions)
        {
            try
            {
                if (this is IFrom<TInput, T> from)
                {
                    var result = from.From(context, input);
                    if (result.Success)
                    {
                        return result;
                    }
                    exceptions += result.Exception;
                }
            }
            catch (Exception e)
            {
                exceptions += e;
            }

            var message = Exceptions.ConvertFailMessage(input, TypeFriendlyName, context.Settings.CultureInfo);
            return exceptions.ToException(message);
        }
    }
}
