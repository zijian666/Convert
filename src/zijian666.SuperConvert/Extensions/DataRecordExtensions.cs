using System;
using System.Data;
using zijian666.SuperConvert.Core;

namespace zijian666.SuperConvert.Extensions
{
    static class DataRecordExtensions
    {
        public static ConvertResult<T> GetValue<T>(this IDataRecord record, int index)
        {
            var handler = Getter<T>.GetValue;
            if (handler != null)
            {
                return handler(record, index);
            }

            return ConvertResult<T>.NULL;
        }

        class Getter<T>
        {
            public static readonly Func<IDataRecord, int, T> GetValue = CreateHandler();

            private static Func<IDataRecord, int, T> CreateHandler()
            {
                if (typeof(T).IsMetaType())
                {
                    var name = "Get" + typeof(T).GetFriendlyName();
                    var method = typeof(T).GetMethod(name);
                    if (method != null)
                    {
                        return (Func<IDataRecord, int, T>)method.CreateDelegate(typeof(Func<IDataRecord, int, T>));
                    }
                }
                return null;
            }
        }

    }
}
