using System;
using zijian666.Core.Abstractions;
namespace zijian666.SuperConvert
{
    internal class FormatterConverter : IFormatterConverterExtra
    {
        public static FormatterConverter Instance { get; } = new FormatterConverter();

        public bool TryConvert<T>(object value, out T result)
        {
            var res = Converts.Convert<T>(value);
            result = res.GetValueOrDefalut(default);
            return res.Success;
        }

        public bool TryConvert(object value, Type type, out object result)
        {
            var res = Converts.Convert(value, type);
            result = res.GetValueOrDefalut(default);
            return res.Success;
        }

        public object Convert(object value, Type type) => value.Convert(type).Value;

        public object Convert(object value, TypeCode typeCode) => typeCode switch
        {
            TypeCode.Boolean => ToBoolean(value),
            TypeCode.Byte => ToByte(value),
            TypeCode.Char => ToChar(value),
            TypeCode.DateTime => ToDateTime(value),
            TypeCode.DBNull => Convert(value, typeof(DBNull)),
            TypeCode.Decimal => ToDecimal(value),
            TypeCode.Double => ToDouble(value),
            TypeCode.Int16 => ToInt16(value),
            TypeCode.Int32 => ToInt32(value),
            TypeCode.Int64 => ToInt64(value),
            TypeCode.Object => value,
            TypeCode.SByte => ToSByte(value),
            TypeCode.Single => ToSingle(value),
            TypeCode.String => ToString(value),
            TypeCode.UInt16 => ToUInt16(value),
            TypeCode.UInt32 => ToUInt32(value),
            TypeCode.UInt64 => ToUInt64(value),
            TypeCode.Empty => throw new NotSupportedException(),
            _ => throw new NotSupportedException(),
        };

        public bool ToBoolean(object value) => value.To<bool>();
        public byte ToByte(object value) => value.To<byte>();
        public char ToChar(object value) => value.To<char>();
        public DateTime ToDateTime(object value) => value.To<DateTime>();
        public decimal ToDecimal(object value) => value.To<decimal>();
        public double ToDouble(object value) => value.To<double>();
        public short ToInt16(object value) => value.To<short>();
        public int ToInt32(object value) => value.To<int>();
        public long ToInt64(object value) => value.To<long>();
        public sbyte ToSByte(object value) => value.To<sbyte>();
        public float ToSingle(object value) => value.To<float>();
        public string ToString(object value) => value.To<string>();
        public ushort ToUInt16(object value) => value.To<ushort>();
        public uint ToUInt32(object value) => value.To<uint>();
        public ulong ToUInt64(object value) => value.To<ulong>();
    }
}
