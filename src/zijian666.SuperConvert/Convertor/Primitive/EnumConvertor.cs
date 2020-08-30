using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class EnumConvertor<T> : BaseConvertor<T>,
        IFrom<string, T>,
        IFrom<IConvertible, T>
    {
        public ConvertResult<T> From(IConvertContext context, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return default;
            }
            T result;
            try
            {
                result = (T)Enum.Parse(OutputType, input, true);
            }
            catch (Exception ex)
            {
                return ex;
            }

            if (result.Equals(default(T)))
            {
                return default;
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
            return context.ConvertFail(this, input);
        }

        public ConvertResult<T> From(IConvertContext context, IConvertible input)
        {
            T result;
            switch (input.GetTypeCode())
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                case TypeCode.DateTime:
                case TypeCode.Boolean:
                    return default;
                case TypeCode.Decimal:
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Double:
                case TypeCode.Single:
                    result = (T)Enum.ToObject(OutputType, input.ToInt64(null));
                    break;
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    result = (T)Enum.ToObject(OutputType, input.ToUInt64(null));
                    break;
                case TypeCode.String:
                    return From(context, input.ToString(null));
                case TypeCode.Object:
                default:
                    return default;
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
            return context.ConvertFail(this, input);
        }
    }
}
