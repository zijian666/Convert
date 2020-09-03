using System;
using System.Collections;
using System.Data;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class DataTableConvertor : AllowNullConvertor<DataTable>
        , IFrom<DataView, DataTable>
        , IFrom<IDataReader, DataTable>
        , IFrom<IEnumerator, DataTable>
    {
        public ConvertResult<DataTable> From(IConvertContext context, DataView input)
            => input?.ToTable();

        public ConvertResult<DataTable> From(IConvertContext context, IDataReader input)
        {
            if (input == null)
            {
                return default;
            }
            var table = new DataTable();
            table.Load(input);
            return table;
        }


        public ConvertResult<DataTable> From(IConvertContext context, IEnumerator input)
        {
            var table = new DataTable();
            var rs = context.Settings.GetResourceStrings();
            while (input.MoveNext())
            {
                var enumerator = new KeyValueEnumerator<string, object>(context, input.Current);
                if (!enumerator.HasStringKey)
                {
                    return context.ConvertFail(this, input);
                }

                var row = table.NewRow();
                while (enumerator.MoveNext())
                {
                    var key = enumerator.GetKey();
                    if (!key.Success)
                    {
                        var message = string.Format(rs.COLLECTION_KEY_FAIL, TypeFriendlyName, enumerator.OriginalKey);
                        return new InvalidCastException(message, key.Exception);
                    }

                    var value = enumerator.GetValue();
                    if (!value.Success)
                    {
                        var message = string.Format(rs.COLLECTION_ADD_FAIL, TypeFriendlyName, key.Value, enumerator.OriginalValue);
                        return new InvalidCastException(message, key.Exception);
                    }

                    var column = table.Columns[key.Value] ?? table.Columns.Add(key.Value);
                    row[column] = value.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
