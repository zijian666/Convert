using System;
using System.Dynamic;
using System.Runtime.Serialization;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Dynamic
{
    internal abstract class DynamicObjectBase<T> : DynamicObject, IObjectReference
    {
        public DynamicObjectBase(T value, IConvertSettings convertSettings)
        {
            Value = value;
            ConvertSettings = convertSettings;
            CustomType = value?.GetType() ?? typeof(T);
        }

        public T Value { get; }
        public IConvertSettings ConvertSettings { get; }

        public object GetRealObject(StreamingContext context) => Value;

        /// <summary>
        /// 打开该对象。
        /// </summary>
        /// <returns> 已打开的对象。 </returns>
        public object Unwrap() => Value;

        /// <summary>
        /// 获取由此对象提供的自定义类型。
        /// </summary>
        /// <returns> 自定义类型。 </returns>
        public Type CustomType { get; }


        protected bool TryChangeType(object input, Type outputType, out object result)
        {
            var res = Converts.Convert(input, outputType, ConvertSettings);
            result = res.GetValueOrDefalut(default);
            return res.Success;
        }

        protected dynamic WrapToDynamic(object value) => DynamicFactory.Create(value, ConvertSettings);
    }
}
