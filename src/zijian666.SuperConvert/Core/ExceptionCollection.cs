using System;
using System.Collections.Generic;

namespace zijian666.SuperConvert.Core
{
    public sealed class ExceptionCollection : List<Exception>
    {
        public static ExceptionCollection operator +(ExceptionCollection ex1, Exception ex2)
        {
            if (ex2 == null)
            {
                return ex1;
            }
            if (ex1 == null)
            {
                return new ExceptionCollection() { ex2 };
            }
            ex1.Add(ex2);
            return ex1;
        }

        public AggregateException ToException(string message) => new AggregateException(message, this);

    }
}
