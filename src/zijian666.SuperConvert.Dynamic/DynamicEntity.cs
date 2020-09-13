using System;
using System.Collections.Generic;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

//using System.Runtime.Remoting;

namespace zijian666.SuperConvert.Dynamic
{
    /// <summary>
    /// 基于 <seealso cref="object"/> 的动态类型
    /// </summary>
    internal class DynamicEntity : DynamicObjectBase<object>
    {
        private readonly PropertiesCollection _properties;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="entity"></param>
        public DynamicEntity(object value, IConvertSettings convertSettings)
            : base(value, convertSettings)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _properties = PropertyHelper.GetByType(value.GetType());
        }


        /// <summary>
        /// 返回所有动态成员名称的枚举。</summary>
        /// <returns>一个包含动态成员名称的序列。</returns>
        public override IEnumerable<string> GetDynamicMemberNames() => _properties.Keys;

        protected override object this[string name]
        {
            get
            {
                var p = _properties[name];
                if (p == null)
                {
                    return null;
                }
                var getter = ConvertSettings.ReflectCompiler?.CompileGetter<object>(p.Property) ?? p.GetValue;
                if (getter == null)
                {
                    return null;
                }
                return getter(Value);
            }
            set
            {
                var p = _properties[name];
                if (p == null)
                {
                    return;
                }
                var setter = ConvertSettings.ReflectCompiler?.CompileSetter<object>(p.Property) ?? p?.SetValue;
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
