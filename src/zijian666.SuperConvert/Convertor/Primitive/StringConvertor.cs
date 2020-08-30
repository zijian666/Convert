
using System;
using System.Collections;
using System.Data;
using System.Net;
using System.Text;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    class StringConvertor : BaseConvertor<string>,
        IFrom<Type, string>,
        IFrom<byte[], string>,
        IFrom<bool, string>,
        IFrom<DataRow, string>,
        IFrom<DataTable, string>,
        IFrom<Uri, string>,
        IFrom<StringBuilder, string>,
        IFrom<IPAddress, string>,
        IFrom<object, string>,
        IFrom<IFormattable, string>,
        IFrom<IConvertible, string>,
        IFrom<IDataReader, string>,
        IFrom<IDataRecord, string>,
        IFrom<IEnumerator, string>,
        IFrom<IEnumerable, string>,
        IFromNull<string>
    {
        public ConvertResult<string> From(IConvertContext context, Type input) =>
            input.GetFriendlyName();
        public ConvertResult<string> FromNull(IConvertContext context) => default;

        public ConvertResult<string> From(IConvertContext context, DBNull input) => default;

        public ConvertResult<string> From(IConvertContext context, IConvertible input)
            => input?.ToString(context.Settings.GetFormatProvider(input.GetType()));

        public ConvertResult<string> From(IConvertContext context, byte[] input) =>
            (context.Settings.Encoding ?? Encoding.UTF8).GetString(input);
        public ConvertResult<string> From(IConvertContext context, bool input) => input ? "true" : "false";

        private string ToString(IConvertContext context, object input) =>
            context.Settings.StringSerializer?.ToString(input) ?? input?.ToString();

        public ConvertResult<string> From(IConvertContext context, IDataReader input) => ToString(context, input);

        public ConvertResult<string> From(IConvertContext context, IDataRecord input) => ToString(context, input);

        public ConvertResult<string> From(IConvertContext context, DataRow input) => ToString(context, input);

        public ConvertResult<string> From(IConvertContext context, DataTable input) => ToString(context, input);

        public ConvertResult<string> From(IConvertContext context, IEnumerable input) =>
            context.Settings.StringSerializer?.ToString(input) ?? From(context, input?.GetEnumerator());

        public ConvertResult<string> From(IConvertContext context, object input)
        {
            if (input.GetType().GetProperties().Length > 0)
            {
                return input?.ToString();
            }
            return input.ToString();
        }

        public ConvertResult<string> From(IConvertContext context, Uri input) => input?.ToString();
        public ConvertResult<string> From(IConvertContext context, StringBuilder input) => input?.ToString();
        public ConvertResult<string> From(IConvertContext context, IEnumerator input)
        {
            if (input == null || !input.MoveNext())
            {
                return default;
            }
            var s = context.Convert<string>(input.Current);
            if (!s.Success)
            {
                return s;
            }

            var sb = new StringBuilder(s.Value);
            while (input.MoveNext())
            {
                s = context.Convert<string>(input.Current);
                if (!s.Success)
                {
                    return s;
                }
                sb.Append(context.Settings.StringSeparator?.First ?? ",");
                sb.Append(s.Value);

            }
            return sb.ToString();
        }

        public ConvertResult<string> From(IConvertContext context, IPAddress input) => input.ToString();
        public ConvertResult<string> From(IConvertContext context, IFormattable input) => context.Settings.Format(input);
    }
}
