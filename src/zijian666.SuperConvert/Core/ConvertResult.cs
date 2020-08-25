using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace zijian666.SuperConvert.Core
{
    /// <summary>
    /// 转换结果
    /// </summary>
    [DebuggerDisplay("{Success} : {Success ? (object)Value : Exception.Message}")]
    public readonly struct ConvertResult<T> : IObjectReference
    {
        public readonly static ConvertResult<T> NULL = new ConvertResult<T>(false, default, null);
        /// <summary>
        /// 转换成功
        /// </summary>
        /// <param name="value">返回值</param>
        public ConvertResult(T value) : this(true, value, null) { }

        /// <summary>
        /// 转换成功
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="value">返回值</param>
        /// <param name="ex">错误对象</param>
        private ConvertResult(bool success, T value, Exception ex)
        {
            _fail = !success;
            _value = value;
            Exception = ex;
        }


        private readonly bool _fail;
        private readonly T _value;

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool Success => !_fail;
        /// <summary>
        /// 转换后的输出结果
        /// </summary>
        /// <returns></returns>
        public T Value
        {
            get
            {
                if (Exception != null)
                {
                    throw Exception;
                }
                return _value;
            }
        }

        public T GetValueOrDefalut(T defaultValue) => Success ? _value : defaultValue;

        /// <summary>
        /// 如果失败,则返回异常
        /// </summary>
        /// <returns></returns>
        public Exception Exception { get; }

        object IObjectReference.GetRealObject(StreamingContext context) => Value;

        public override bool Equals(object obj) => obj is ConvertResult<T> result && Equals(result.Value, Value);

        public override int GetHashCode() => ((object)Value ?? Exception).GetHashCode();

        public static bool operator ==(ConvertResult<T> left, ConvertResult<T> right) => left.Equals(right);

        public static bool operator !=(ConvertResult<T> left, ConvertResult<T> right) => !(left == right);


        #region 隐式转换

        public static implicit operator ConvertResult<T>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            return new ConvertResult<T>(false, default, exception);
        }

        public static implicit operator ConvertResult<T>(T value) => new ConvertResult<T>(true, value, null);

        #endregion
    }
}
