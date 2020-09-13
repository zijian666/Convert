using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Dynamic
{
    public static class DynamicFactory
    {
        public static DynamicObject Create(object value, IConvertSettings convertSettings)
        {
            switch (value)
            {
                case DBNull dbNull:
                    return DynamicPrimitive.Null;
                case null:
                    return DynamicPrimitive.Null;
                case DynamicObject dyc:
                    return dyc;
                case string str:
                    return new DynamicPrimitive(str, convertSettings);
                case DataRow row:
                    return new DynamicDataRow(row, convertSettings);
                case DataRowView view:
                    return new DynamicDataRow(view.Row, convertSettings);
                case NameValueCollection nv:
                    return new DynamicNameValueCollection(nv, convertSettings);
                case IDataReader reader:
                    return new DynamicDictionary(reader.To<IDictionary>(), convertSettings);
                case IDictionary dict:
                    return new DynamicDictionary(dict, convertSettings);
                case IList list:
                    return new DynamicList(list, convertSettings);
                case IEnumerator e1:
                    return new DynamicEnumerator(e1, convertSettings);
                case IEnumerable e2:
                    return new DynamicEnumerator(e2.GetEnumerator(), convertSettings);
            }
            if (value.GetType().IsMetaType())
            {
                return new DynamicPrimitive(value, convertSettings);
            }
            return new DynamicEntity(value, convertSettings);
        }
    }
}
