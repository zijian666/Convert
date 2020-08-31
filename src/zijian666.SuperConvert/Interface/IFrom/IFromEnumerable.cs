using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;

namespace zijian666.SuperConvert.Interface
{
    /// <summary>
    /// 处理枚举类型的转换接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromEnumerable<T>
        : IFrom<IList, T>
        , IFrom<DataRow, T>
        , IFrom<DataRowView, T>
        , IFrom<DataTable, T>
        , IFrom<IDictionary, T>
        , IFrom<NameObjectCollectionBase, T>
        , IFrom<StringDictionary, T>
        , IFrom<Array, T>
        , IFrom<IListSource, T>
    {

    }
}
