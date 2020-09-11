using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using zijian666.SuperConvert.Interface;

//using System.Runtime.Remoting;

namespace zijian666.SuperConvert.Dynamic
{
    /// <summary>
    /// 基于 <seealso cref="IList" /> 的动态类型
    /// </summary>
    internal class DynamicList : DynamicObjectBase<IList>, IList, IObjectReference//, IObjectHandle, ICustomTypeProvider
    {
        private static readonly IEnumerable<string> _dynamicMemberNames = new List<string> { "Count", "Length" }.AsReadOnly();

        /// <summary>
        /// 初始化指定集合的动态类型包装
        /// </summary>
        /// <param name="list"> </param>
        public DynamicList(IList value, IConvertSettings convertSettings)
            : base(value, convertSettings)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前实例是否为只读。
        /// </summary>
        public bool IsReadOnly => Value.IsReadOnly;


        int IList.Add(object value) => Value.Add(value);

        void IList.Clear() => Value.Clear();

        bool IList.Contains(object value) => Value.Contains(value);

        int IList.IndexOf(object value) => Value.IndexOf(value);

        void IList.Insert(int index, object value) => Value.Insert(index, value);

        bool IList.IsFixedSize => Value.IsFixedSize;

        void IList.Remove(object value) => Value.Remove(value);

        void IList.RemoveAt(int index) => Value.RemoveAt(index);

        object IList.this[int index]
        {
            get
            {
                if ((index >= 0) && (index < Value.Count))
                {
                    return WrapToDynamic(Value[index]);
                }
                return DynamicPrimitive.Null;
            }
            set { Value[index] = value; }
        }

        void ICollection.CopyTo(Array array, int index) => Value.CopyTo(array, index);

        int ICollection.Count => Value.Count;

        bool ICollection.IsSynchronized => Value.IsSynchronized;

        object ICollection.SyncRoot => Value;

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in Value)
            {
                if (item == null)
                {
                    yield return DynamicPrimitive.Null;
                }
                yield return WrapToDynamic(item);
            }
        }


        /// <summary>
        /// 打开该对象。
        /// </summary>
        /// <returns> 已打开的对象。 </returns>
        public virtual object Unwrap() => Value;

        /// <summary>
        /// 返回应进行反序列化的真实对象（而不是序列化流指定的对象）。
        /// </summary>
        /// <returns> 返回放入图形中的实际对象。 </returns>
        /// <param name="context"> 当前对象从其中进行反序列化的 <see cref="T:System.Runtime.Serialization.StreamingContext" />。 </param>
        public virtual object GetRealObject(StreamingContext context) => Value;

        /// <summary>
        /// 返回所有动态成员名称的枚举。
        /// </summary>
        /// <returns> 一个包含动态成员名称的序列。 </returns>
        public override IEnumerable<string> GetDynamicMemberNames() => _dynamicMemberNames;

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
            if (typeof(IConvertible).IsAssignableFrom(binder.ReturnType))
            {
                if ((Value.Count == 1) && TryChangeType(Value[0], binder.ReturnType, out result))
                {
                    return true;
                }
            }
            return TryChangeType(Value, binder.ReturnType, out result);
        }

        /// <summary>
        /// 为获取成员值的操作提供实现。 从 <see cref="T:System.Dynamic.DynamicObject" /> 类派生的类可以重写此方法，以便为诸如获取属性值这样的操作指定动态行为。
        /// </summary>
        /// <returns> 如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发运行时异常。） </returns>
        /// <param name="binder">
        /// 提供有关调用了动态操作的对象的信息。 binder.Name 属性提供针对其执行动态操作的成员的名称。 例如，对于
        /// Console.WriteLine(sampleObject.SampleProperty) 语句（其中 sampleObject 是派生自 <see cref="T:System.Dynamic.DynamicObject" />
        /// 类的类的一个实例），binder.Name 将返回“SampleProperty”。 binder.IgnoreCase 属性指定成员名称是否区分大小写。
        /// </param>
        /// <param name="result"> 获取操作的结果。 例如，如果为某个属性调用该方法，则可以为 <paramref name="result" /> 指派该属性值。 </param>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if ("count".Equals(binder.Name, StringComparison.OrdinalIgnoreCase)
                || "length".Equals(binder.Name, StringComparison.OrdinalIgnoreCase))
            {
                result = Value.Count;
                return true;
            }
            result = null;
            return false;
        }

        private int Indexer(object[] indexes)
        {
            if ((indexes == null) || (indexes.Length != 1))
            {
                return -1;
            }
            var i = indexes[0].To(-1);
            if ((i < 0) || (i >= Value.Count))
            {
                return -1;
            }
            return i;
        }

        /// <summary>
        /// 为按索引获取值的操作提供实现。 从 <see cref="T:System.Dynamic.DynamicObject" /> 类派生的类可以重写此方法，以便为索引操作指定动态行为。
        /// </summary>
        /// <returns> 如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发运行时异常。） </returns>
        /// <param name="binder"> 提供有关该操作的信息。 </param>
        /// <param name="indexes">
        /// 该操作中使用的索引。 例如，对于 C# 中的 sampleObject[3] 操作（Visual Basic 中为 sampleObject(3)）（其中 sampleObject 派生自
        /// DynamicObject 类），<paramref name="indexes[0]" /> 等于 3。
        /// </param>
        /// <param name="result"> 索引操作的结果。 </param>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var i = Indexer(indexes);
            if ((i >= 0) && TryChangeType(Value[i], binder.ReturnType, out result))
            {
                result = WrapToDynamic(result);
                return true;
            }

            result = DynamicPrimitive.Null;
            return true;
        }

        /// <summary>
        /// 为按索引设置值的操作提供实现。 从 <see cref="T:System.Dynamic.DynamicObject" /> 类派生的类可以重写此方法，以便为按指定索引访问对象的操作指定动态行为。
        /// </summary>
        /// <returns> 如果此运算成功，则为 true；否则为 false。 如果此方法返回 false，则该语言的运行时联编程序将决定行为。（大多数情况下，将引发语言特定的运行时异常。） </returns>
        /// <param name="binder"> 提供有关该操作的信息。 </param>
        /// <param name="indexes">
        /// 该操作中使用的索引。 例如，对于 C# 中的 sampleObject[3] = 10 操作（Visual Basic 中为 sampleObject(3) = 10）（其中
        /// sampleObject 派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类），<paramref name="indexes[0]" /> 等于 3。
        /// </param>
        /// <param name="value">
        /// 要为具有指定索引的对象设置的值。 例如，对于 C# 中的 sampleObject[3] = 10 操作（Visual Basic 中为 sampleObject(3) = 10）（其中
        /// sampleObject 派生自 <see cref="T:System.Dynamic.DynamicObject" /> 类），<paramref name="value" /> 等于 10。
        /// </param>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (IsReadOnly)
            {
                return false;
            }

            var i = Indexer(indexes);
            if (i < 0)
            {
                return false;
            }
            Value[i] = value;
            return true;
        }
    }
}
