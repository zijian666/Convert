using System;
using System.Collections.Generic;
using System.Reflection;
using zijian666.SuperConvert.Interface;
//using System.Runtime.Remoting;

namespace zijian666.SuperConvert.Dynamic
{
    /// <summary>
    /// 基于系统原始类型的动态类型
    /// </summary>
    internal class DynamicPrimitive : DynamicObjectBase<object>
    {
        /// <summary>
        /// 表示一个null的动态类型
        /// </summary>
        public static readonly DynamicPrimitive Null = new DynamicPrimitive(null, Converts.Settings);

        /// <summary>
        /// 使用指定对象初始化实例
        /// </summary>
        /// <param name="value"> </param>

        public DynamicPrimitive(object value, IConvertSettings convertSettings)
            : base(value, convertSettings)
        {
        }

        /// <summary>
        /// 返回所有动态成员名称的枚举。
        /// </summary>
        /// <returns> 一个包含动态成员名称的序列。 </returns>
        public override IEnumerable<string> GetDynamicMemberNames() => Array.Empty<string>();

        protected override object this[string name]
        {
            get
            {
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var p = Value?.GetType().GetProperty(name, flags);
                if (p == null)
                {
                    return null;
                }
                var getter = ConvertSettings.ReflectCompiler?.CompileGetter<object>(p) ?? p.GetValue;
                if (getter == null)
                {
                    return null;
                }
                return getter(Value);
            }
            set
            {
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var p = Value?.GetType().GetProperty(name, flags);
                if (p == null)
                {
                    return;
                }
                var setter = ConvertSettings.ReflectCompiler?.CompileSetter<object>(p) ?? p.SetValue;
                if (setter == null)
                {
                    return;
                }
                if (!p.PropertyType.IsInstanceOfType(value))
                {
                    value = value.Convert(p.PropertyType, ConvertSettings).Value;
                }
                setter(Value, value);
            }
        }
    }
}
