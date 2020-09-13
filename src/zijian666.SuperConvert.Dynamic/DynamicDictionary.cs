using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
//using System.Runtime.Remoting;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Dynamic
{
    /// <summary>
    /// 基于 <seealso cref="IDictionary" /> 的动态类型
    /// </summary>
    internal class DynamicDictionary : DynamicObjectBase<IDictionary>, IDictionary
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dict"> </param>
        public DynamicDictionary(IDictionary value, IConvertSettings convertSettings)
            : base(value, convertSettings)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        public override bool IsReadOnly => Value.IsReadOnly;


        /// <summary>
        /// 返回所有动态成员名称的枚举。
        /// </summary>
        /// <returns> 一个包含动态成员名称的序列。 </returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var key in Value.Keys)
            {
                if (key is string str)
                {
                    yield return str;
                }
            }
        }

        /// <summary>
        /// 提供类型转换运算的实现。 从 <see cref="T:System.Dynamic.DynamicObject" /> 类派生的类可以重写此方法，以便为将某个对象从一种类型转换为另一种类型的运算指定动态行为。
        /// </summary>
        /// <returns> 如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发语言特定的运行时异常。） </returns>
        /// <param name="binder">
        /// 提供有关转换运算的信息。 binder.Type 属性提供必须将对象转换为的类型。 例如，对于 C# 中的语句 (String)sampleObject（Visual Basic 中为
        /// CType(sampleObject, Type)）（其中 sampleObject 是派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类的类的一个实例），binder.Type 将返回
        /// <see cref="T:System.String" /> 类型。 binder.Explicit 属性提供有关所发生转换的类型的信息。 对于显式转换，它返回 true；对于隐式转换，它返回 false。
        /// </param>
        /// <param name="result"> 类型转换运算的结果。 </param>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (Value.Count == 1)
            {
                foreach (var item in Value.Values)
                {
                    if (TryChangeType(item, binder.ReturnType, out result))
                    {
                        return true;
                    }
                    break;
                }
            }
            return TryChangeType(Value, binder.ReturnType, out result);
        }

        protected override object this[object key]
        {
            get => Value[key];
            set => Value[key] = value;
        }

        #region 显示实现接口

        IEnumerator IEnumerable.GetEnumerator() => Value.GetEnumerator();
        void IDictionary.Add(object key, object value) => Value.Add(key, value);
        void IDictionary.Clear() => Value.Clear();
        bool IDictionary.Contains(object key) => Value.Contains(key);
        IDictionaryEnumerator IDictionary.GetEnumerator() => Value.GetEnumerator();
        bool IDictionary.IsFixedSize => false;
        bool IDictionary.IsReadOnly => false;
        ICollection IDictionary.Keys => Value.Keys;
        void IDictionary.Remove(object key) => Value.Remove(key);
        ICollection IDictionary.Values => Value.Values;

        object IDictionary.this[object key]
        {
            get { return Value[key]; }
            set { Value[key] = value; }
        }

        void ICollection.CopyTo(Array array, int index) => Value.CopyTo(array, index);
        int ICollection.Count => Value.Count;
        bool ICollection.IsSynchronized { get; } = false;
        object ICollection.SyncRoot => Value;

        #endregion
    }
}
