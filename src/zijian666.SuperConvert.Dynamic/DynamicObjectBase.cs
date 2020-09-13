using System;
using System.Collections;
using System.Dynamic;
using System.Reflection;
using System.Runtime.Serialization;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Dynamic
{
    internal abstract class DynamicObjectBase<T> : DynamicObject, IObjectReference, IEquatable<object>, IComparable,
        IComparable<object>, IConvertible, IFormattable
    {
        public DynamicObjectBase(T value, IConvertSettings convertSettings)
        {
            Value = value;
            ConvertSettings = convertSettings ?? Converts.Settings;
            CustomType = value?.GetType() ?? typeof(T);
        }

        public T Value { get; }
        public IConvertSettings ConvertSettings { get; }

        public object GetRealObject(StreamingContext context) => Value;

        /// <summary>
        /// 打开该对象。
        /// </summary>
        /// <returns> 已打开的对象。 </returns>
        public object Unwrap() => Value;

        /// <summary>
        /// 获取由此对象提供的自定义类型。
        /// </summary>
        /// <returns> 自定义类型。 </returns>
        public Type CustomType { get; }


        protected bool TryChangeType(object input, Type outputType, out object result)
        {
            var res = Converts.Convert(input, outputType, ConvertSettings);
            result = res.GetValueOrDefalut(default);
            return res.Success;
        }

        protected dynamic WrapToDynamic(object value) => DynamicFactory.Create(value, ConvertSettings);

        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            return base.TryUnaryOperation(binder, out result);
        }

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        public virtual bool IsReadOnly => false;

        protected string GetIndexer0(object[] indexes)
        {
            if ((indexes == null) || (indexes.Length != 1))
            {
                return null;
            }
            return indexes[0] as string;
        }

        protected virtual object this[int index]
        {
            get => null;
            set { }
        }

        protected virtual object this[string name]
        {
            get => null;
            set { }
        }

        protected virtual object this[object key]
        {
            get => key switch
            {
                string name => this[name],
                int i => this[i],
                _ => this[key.Convert<int>(ConvertSettings).GetValueOrDefalut(-1)]
            };

            set => _ = key switch
            {
                string name => this[name] = value,
                int i => this[i] = value,
                _ => this[key.Convert<int>(ConvertSettings).GetValueOrDefalut(-1)] = value
            };
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if ((indexes == null) || (indexes.Length != 1))
            {
                result = DynamicPrimitive.Null;
                return true;
            }
            var index = indexes[0];
            var value = this[index];

            if (value == null)
            {
                result = DynamicPrimitive.Null;
                return true;
            }
            if (binder.ReturnType == typeof(object))
            {
                result = WrapToDynamic(value);
                return true;
            }
            var r = value.Convert(binder.ReturnType, ConvertSettings);
            result = r.Success ? WrapToDynamic(r.Value) : DynamicPrimitive.Null;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var value = this[(object)binder.Name];
            if (value == null)
            {
                result = DynamicPrimitive.Null;
                return true;
            }
            if (binder.ReturnType == typeof(object))
            {
                result = WrapToDynamic(value);
                return true;
            }
            var r = value.Convert(binder.ReturnType, ConvertSettings);
            result = r.Success ? WrapToDynamic(r.Value) : DynamicPrimitive.Null;
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (IsReadOnly || indexes == null || indexes.Length != 1)
            {
                return false;
            }
            var index = indexes[0];
            this[index] = value;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (IsReadOnly)
            {
                return false;
            }
            this[(object)binder.Name] = value;
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result) => TryChangeType(Value, binder.ReturnType, out result);


        /// <summary>
        /// 尝试调用成员的方法
        /// </summary>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (Value == null)
            {
                result = DynamicPrimitive.Null;
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
                result = DynamicPrimitive.Null;
                return false;
            }
        }




        public int CompareTo(object obj) => Compare(this, obj);

        public static implicit operator string(DynamicObjectBase<T> value) => value.Value.To<string>();


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


        public override string ToString() => Value.To<string>();


        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        private static int Compare(DynamicObjectBase<T> a, object b)
        {
            var b1 = b as DynamicObjectBase<T>;
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
        public static bool operator >(DynamicObjectBase<T> a, object b) => Compare(a, b) > 0;

        public static bool operator <(DynamicObjectBase<T> a, object b) => Compare(a, b) < 0;

        public static bool operator ==(DynamicObjectBase<T> a, object b) => a?.Equals(b) ?? b is null;

        public static bool operator !=(DynamicObjectBase<T> a, object b) => !(a?.Equals(b) ?? b is null);

        public static bool operator >=(DynamicObjectBase<T> a, object b) => Compare(a, b) >= 0;

        public static bool operator <=(DynamicObjectBase<T> a, object b) => Compare(a, b) <= 0;
        #endregion

        Result ConvertTo<Result>(IFormatProvider provider)
        {
            if (provider == null)
            {
                return Value.Convert<Result>(ConvertSettings).Value;
            }
            var convertSettings = new ConvertSettings()
            {
                FormatProviders = { [CustomType] = provider }
            };
            return Value.Convert<Result>(convertSettings.Combin(ConvertSettings)).Value;
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
            return Value.Convert<string>(settings.Combin(ConvertSettings)).Value;
        }
    }
}
