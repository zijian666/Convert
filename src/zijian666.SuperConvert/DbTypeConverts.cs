using System;
using System.Data;
using System.Xml;

namespace zijian666.SuperConvert
{
    public static partial class Converts
    {
        /// <summary>
        /// 将 <seealso cref="Type" /> 对象转为对应 <seealso cref="DbType" /> 对象
        /// </summary>
        public static DbType TypeToDbType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return DbType.Boolean;
                case TypeCode.Byte:
                    return DbType.Byte;
                case TypeCode.Char:
                    return DbType.Boolean;
                case TypeCode.DBNull:
                    return DbType.Object;
                case TypeCode.DateTime:
                    return DbType.DateTime;
                case TypeCode.Decimal:
                    return DbType.Decimal;
                case TypeCode.Double:
                    return DbType.Double;
                case TypeCode.Empty:
                    return DbType.Object;
                case TypeCode.Int16:
                    return DbType.Int16;
                case TypeCode.Int32:
                    return DbType.Int32;
                case TypeCode.Int64:
                    return DbType.Int64;
                case TypeCode.SByte:
                    return DbType.SByte;
                case TypeCode.Single:
                    return DbType.Single;
                case TypeCode.String:
                    return DbType.String;
                case TypeCode.UInt16:
                    return DbType.UInt16;
                case TypeCode.UInt32:
                    return DbType.UInt32;
                case TypeCode.UInt64:
                    return DbType.UInt64;
                case TypeCode.Object:
                default:
                    break;
            }
            if (type == typeof(Guid))
            {
                return DbType.Guid;
            }
            if (type == typeof(byte[]))
            {
                return DbType.Binary;
            }
            if (type == typeof(XmlDocument))
            {
                return DbType.Xml;
            }
            return DbType.Object;
        }

        /// <summary>
        /// 将 <seealso cref="DbType" /> 对象转为对应 <seealso cref="Type" /> 对象
        /// </summary>
        public static Type DbTypeToType(DbType dbtype)
            => dbtype switch
            {
                DbType.AnsiString => typeof(string),
                DbType.AnsiStringFixedLength => typeof(string),
                DbType.String => typeof(string),
                DbType.StringFixedLength => typeof(string),
                DbType.Binary => typeof(byte[]),
                DbType.Boolean => typeof(bool),
                DbType.Byte => typeof(byte),
                DbType.Date => typeof(DateTime),
                DbType.DateTime => typeof(DateTime),
                DbType.DateTime2 => typeof(DateTime),
                DbType.DateTimeOffset => typeof(DateTime),
                DbType.Time => typeof(DateTime),
                DbType.Decimal => typeof(decimal),
                DbType.VarNumeric => typeof(decimal),
                DbType.Currency => typeof(decimal),
                DbType.Double => typeof(double),
                DbType.Guid => typeof(Guid),
                DbType.Int16 => typeof(short),
                DbType.Int32 => typeof(int),
                DbType.Int64 => typeof(long),
                DbType.Object => typeof(object),
                DbType.SByte => typeof(sbyte),
                DbType.Single => typeof(float),
                DbType.UInt16 => typeof(ushort),
                DbType.UInt32 => typeof(uint),
                DbType.UInt64 => typeof(ulong),
                DbType.Xml => typeof(XmlDocument),
                _ => throw new InvalidCastException("无效的DbType值:" + dbtype),
            };
    }
}
