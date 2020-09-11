using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;
//using System.Runtime.Remoting;

namespace zijian666.SuperConvert.Dynamic
{
    /// <summary>
    /// 基于系统原始类型的动态类型
    /// </summary>
    internal class DynamicPrimitive : DynamicObjectBase<object>, IEquatable<object>, IComparable,
        IComparable<object>, IObjectReference, IConvertible, IFormattable
    {
        /// <summary>
        /// 表示一个null的动态类型
        /// </summary>
        public static readonly DynamicPrimitive Null = new DynamicPrimitive(null, Converts.Settings);

        /// <summary>
        /// 表示一个空的字符串数组
        /// </summary>
        private static readonly string[] _emptyStrings = new string[0];

        /// <summary>
        /// 使用指定对象初始化实例
        /// </summary>
        /// <param name="value"> </param>

        public DynamicPrimitive(object value, IConvertSettings convertSettings)
            : base(value, convertSettings)
        {
        }

        /// <summary>
        /// 将当前实例与同一类型的另一个对象进行比较，并返回一个整数，该整数指示当前实例在排序顺序中的位置是位于另一个对象之前、之后还是与其位置相同。
        /// </summary>
        /// <returns>
        /// 一个值，指示要比较的对象的相对顺序。返回值的含义如下：值含义小于零此实例在排序顺序中位于 <paramref name="obj" /> 之前。零此实例在排序顺序中的位置与
        /// <paramref name="obj" /> 相同。大于零此实例在排序顺序中位于 <paramref name="obj" /> 之后。
        /// </returns>
        /// <param name="obj"> 与此实例进行比较的对象。 </param>
        public int CompareTo(object obj) => Compare(this, obj);

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <returns> 如果指定的对象等于当前对象，则为 true，否则为 false。 </returns>
        /// <param name="obj"> 要与当前对象进行比较的对象。 </param>
        /// <filterpriority> 2 </filterpriority>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return Value == null;
            }
            if (ReferenceEquals(this, obj)) //相同实例
            {
                return true;
            }
            obj = (obj as IObjectReference)?.GetRealObject(new StreamingContext()) ?? obj; //获取被包装的类型
            if (Equals(Value, obj))
            {
                return true;
            }
            var result = Value.Convert(obj.GetType(), null); //尝试类型转换后比较
            return result.Success && Equals(result.Value, obj);
        }

        /// <summary>
        /// 打开该对象。
        /// </summary>
        /// <returns> 已打开的对象。 </returns>
        public virtual object Unwrap() => Value;

        /// <summary>
        /// 返回应进行反序列化的真实对象（而不是序列化流指定的对象）。
        /// </summary>
        /// <returns> 返回放入图形中的实际对象。 </returns>
        /// <param name="context"> 当前对象从其中进行反序列化的 <see cref="T:System.Runtime.Serialization.StreamingContext" />。 </param>
        public virtual object GetRealObject(StreamingContext context) => Value;

        /// <summary>
        /// 返回所有动态成员名称的枚举。
        /// </summary>
        /// <returns> 一个包含动态成员名称的序列。 </returns>
        public override IEnumerable<string> GetDynamicMemberNames() => _emptyStrings;

        /// <summary>
        /// 尝试转换类型
        /// </summary>
        public override bool TryConvert(ConvertBinder binder, out object result)
            => TryChangeType(Value, binder.ReturnType, out result);

        /// <summary>
        /// 尝试获取类型的成员对象
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var p = Value?.GetType().GetProperty(binder.Name, flags);
            if ((p == null) || (p.GetIndexParameters().Length > 0))
            {
                result = Null;
                return true;
            }
            result = WrapToDynamic(p.GetValue(Value));
            return true;
        }

        /// <summary>
        /// 尝试调用成员的方法
        /// </summary>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (Value == null)
            {
                result = Null;
                return false;
            }
            try
            {
                result = WrapToDynamic(Value.GetType().InvokeMember(
                                                binder.Name,
                                                BindingFlags.InvokeMethod,
                                                null, Value, args));
                return true;
            }
            catch
            {
                result = Null;
                return false;
            }
        }

        /// <summary>
        /// 二元操作
        /// </summary>
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            var a = Value.To<bool>();
            switch (binder.Operation)
            {
                case ExpressionType.AndAlso:
                    result = a && ((arg as DynamicPrimitive)?.Value ?? arg).To<bool>();
                    return true;
                case ExpressionType.OrElse:
                    result = a || ((arg as DynamicPrimitive)?.Value ?? arg).To<bool>();
                    return true;
                case ExpressionType.And:
                    result = a & ((arg as DynamicPrimitive)?.Value ?? arg).To<bool>();
                    return true;
                case ExpressionType.Or:
                    result = a | ((arg as DynamicPrimitive)?.Value ?? arg).To<bool>();
                    return true;
                default:
                    break;
            }
            return base.TryBinaryOperation(binder, arg, out result);
        }

        /// <summary>
        /// 一元操作
        /// </summary>
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            var value = Value.Convert<bool>(null);
            if (value.Success == false) //转型失败
            {
                result = null;
                return false;
            }
            switch (binder.Operation)
            {
                case ExpressionType.IsFalse:
                case ExpressionType.Not:
                    result = value.Value == false;
                    return true;
                case ExpressionType.IsTrue:
                    result = value.Value;
                    return true;
                default:
                    break;
            }
            return base.TryUnaryOperation(binder, out result);
        }

        /// <summary>
        /// 当前对象与string的隐式转换
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static implicit operator string(DynamicPrimitive value) => value.Value.To<string>();

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = Null;
            return true;
        }

        /// <summary>
        /// 返回表示当前对象的字符串。
        /// </summary>
        /// <returns> 表示当前对象的字符串。 </returns>
        /// <filterpriority> 2 </filterpriority>
        public override string ToString() => Value.To<string>();

        /// <summary>
        /// 作为默认哈希函数。
        /// </summary>
        /// <returns> 当前对象的哈希代码。 </returns>
        /// <filterpriority> 2 </filterpriority>
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        private static int Compare(DynamicPrimitive a, object b)
        {
            var b1 = b as DynamicPrimitive;
            if (ReferenceEquals(b1, null) == false)
            {
                b = b1.Value;
            }
            if (ReferenceEquals(b, null))
            {
                return ReferenceEquals(a, null) ? 0 : 1;
            }
            if (a.Value == null)
            {
                return -1;
            }

            var comparer = Comparer.DefaultInvariant;
            if (a.TryChangeType(a.Value, b.GetType(), out var c))
            {
                return comparer.Compare(c, b);
            }
            return -1;
        }

        #region 运算符重载
        public static bool operator >(DynamicPrimitive a, object b) => Compare(a, b) > 0;

        public static bool operator <(DynamicPrimitive a, object b) => Compare(a, b) < 0;

        public static bool operator ==(DynamicPrimitive a, object b) => a?.Equals(b) ?? b is null;

        public static bool operator !=(DynamicPrimitive a, object b) => !(a?.Equals(b) ?? b is null);

        public static bool operator >=(DynamicPrimitive a, object b) => Compare(a, b) >= 0;

        public static bool operator <=(DynamicPrimitive a, object b) => Compare(a, b) <= 0;
        #endregion

        T ConvertTo<T>(IFormatProvider provider)
        {
            if (provider == null)
            {
                return Value.Convert<T>(ConvertSettings).Value;
            }
            var convertSettings = new ConvertSettings()
            {
                FormatProviders = { [CustomType] = provider }
            };
            return Value.Convert<T>(convertSettings.Combin(ConvertSettings)).Value;
        }
        TypeCode IConvertible.GetTypeCode() =>
            (Value as IConvertible)?.GetTypeCode() ?? TypeCode.Object;
        bool IConvertible.ToBoolean(IFormatProvider provider) =>
            (Value as IConvertible)?.ToBoolean(provider) ?? ConvertTo<bool>(provider);
        byte IConvertible.ToByte(IFormatProvider provider) =>
            (Value as IConvertible)?.ToByte(provider) ?? ConvertTo<byte>(provider);
        char IConvertible.ToChar(IFormatProvider provider) =>
            (Value as IConvertible)?.ToChar(provider) ?? ConvertTo<char>(provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) =>
            (Value as IConvertible)?.ToDateTime(provider) ?? ConvertTo<DateTime>(provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider) =>
            (Value as IConvertible)?.ToDecimal(provider) ?? ConvertTo<decimal>(provider);
        double IConvertible.ToDouble(IFormatProvider provider) =>
            (Value as IConvertible)?.ToDouble(provider) ?? ConvertTo<double>(provider);
        short IConvertible.ToInt16(IFormatProvider provider) =>
            (Value as IConvertible)?.ToInt16(provider) ?? ConvertTo<short>(provider);
        int IConvertible.ToInt32(IFormatProvider provider) =>
            (Value as IConvertible)?.ToInt32(provider) ?? ConvertTo<int>(provider);
        long IConvertible.ToInt64(IFormatProvider provider) =>
            (Value as IConvertible)?.ToInt64(provider) ?? ConvertTo<long>(provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider) =>
            (Value as IConvertible)?.ToSByte(provider) ?? ConvertTo<sbyte>(provider);
        float IConvertible.ToSingle(IFormatProvider provider) =>
            (Value as IConvertible)?.ToSingle(provider) ?? ConvertTo<float>(provider);
        string IConvertible.ToString(IFormatProvider provider) =>
            (Value as IConvertible)?.ToString(provider) ?? ConvertTo<string>(provider);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (Value is IConvertible conv)
            {
                return conv.ToType(conversionType, provider);
            }
            if (provider == null)
            {
                return Value.Convert(conversionType, ConvertSettings).Value;
            }
            var convertSettings = new ConvertSettings()
            {
                FormatProviders = { [CustomType] = provider }
            };
            return Value.Convert(conversionType, convertSettings.Combin(ConvertSettings)).Value;
        }
        ushort IConvertible.ToUInt16(IFormatProvider provider) =>
            (Value as IConvertible)?.ToUInt16(provider) ?? ConvertTo<ushort>(provider);
        uint IConvertible.ToUInt32(IFormatProvider provider) =>
            (Value as IConvertible)?.ToUInt32(provider) ?? ConvertTo<uint>(provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider) =>
            (Value as IConvertible)?.ToUInt64(provider) ?? ConvertTo<ulong>(provider);
        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            if (Value is IFormattable formattable)
            {
                return formattable.ToString(format, provider);
            }
            if (provider == null && string.IsNullOrWhiteSpace(format))
            {
                return Value.Convert<string>(ConvertSettings).Value;
            }

            var settings = new ConvertSettings();
            if (provider != null)
            {
                settings.FormatProviders[CustomType] = provider;
            }
            if (!string.IsNullOrWhiteSpace(format))
            {
                settings.FormatStrings[CustomType] = format;
            }
            return Value.Convert(conversionType, convertSettings.Combin(ConvertSettings)).Value;
        }
    }
}
