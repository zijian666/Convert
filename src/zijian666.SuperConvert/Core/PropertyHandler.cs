using System;
using System.Reflection;
using zijian666.Core.Abstractions;

namespace zijian666.SuperConvert.Core
{
    /// <summary>
    /// 用于操作属性的Get和Set
    /// </summary>
    public class PropertyHandler
    {
        /// <summary>
        /// 初始化 <see cref="PropertyHandler"/>
        /// </summary>
        /// <param name="property">属性</param>
        private PropertyHandler(PropertyInfo property)
        {
            Property = property;
            PropertyType = property.PropertyType;
            Name = property.Name;
            GetValue = property.GetValue;
            SetValue = property.SetValue;
        }

        /// <summary>
        /// 调用构造函数
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static PropertyHandler Create(PropertyInfo property) => new PropertyHandler(property);

        /// <summary>
        /// 属性类型
        /// </summary>
        public Type PropertyType { get; }
        /// <summary>
        /// 属性的Get方法委托
        /// </summary>
        public MemberGetter<object> GetValue { get; }
        /// <summary>
        /// 属性的Set方法委托
        /// </summary>
        public MemberSetter<object> SetValue { get; }
        /// <summary>
        /// 属性
        /// </summary>
        public PropertyInfo Property { get; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; }
    }
}
