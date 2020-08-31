using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Translator
{
    public class EnumeratorTranslator : ITranslator
    {
        public bool CanTranslate(Type type)
            => type != typeof(string)
            && (typeof(IEnumerable).IsAssignableFrom(type)
            || typeof(DataRow).IsAssignableFrom(type)
            || typeof(DataRowView).IsAssignableFrom(type)
            || typeof(DataTable).IsAssignableFrom(type)
            || typeof(IListSource).IsAssignableFrom(type));

        public object Translate(IConvertContext context, object input)
            => input switch
            {
                string str => null,
                IEnumerable enumerable => enumerable.GetEnumerator(),
                DataRow dataRow => dataRow.ItemArray.GetEnumerator(),
                DataRowView dataRowView => dataRowView.Row?.ItemArray.GetEnumerator(),
                DataTable dataTable => dataTable.Rows.GetEnumerator(),
                IListSource listSource => listSource.GetList()?.GetEnumerator(),
                _ => null,
            };
    }
}
