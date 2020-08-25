using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace zijian666.SuperConvert.Core
{
    /// <summary>
    /// 对象属性操作
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// 属性缓存
        /// </summary>
        private static readonly ConcurrentDictionary<PropertyInfo, PropertyHandler> _propertyCache =
            new ConcurrentDictionary<PropertyInfo, PropertyHandler>();

        /// <summary>
        /// 类型缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyHandler>> _typeCache =
            new ConcurrentDictionary<Type, Dictionary<string, PropertyHandler>>();

        /// <summary>
        /// 表示一个空属性集合
        /// </summary>
        private static readonly Dictionary<string, PropertyHandler> _empty = new Dictionary<string, PropertyHandler>();

        /// <summary>
        /// 根据类型获取操作属性的对象集合
        /// </summary>
        /// <param name="type"> 需要获取属性的类型 </param>
        /// <returns> </returns>
        public static Dictionary<string, PropertyHandler> GetByType(Type type) =>
            type == null ? null : _typeCache.GetOrAdd(type, Create);

        /// <summary>
        /// 根据类型创建一个操作属性的对象集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Dictionary<string, PropertyHandler> Create(Type type)
        {
            var ps = type.GetProperties();
            var length = ps.Length;
            if (length == 0)
            {
                _typeCache.TryAdd(type, _empty);
                return _empty;
            }
            var result = new Dictionary<string, PropertyHandler>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < length; i++)
            {
                var handler = GetPropertyHandler(ps[i]);
                result.Add(handler.Name, handler);
            }
            return result;
        }

        /// <summary>
        /// 根据属性值获取属性操作对象
        /// </summary>
        /// <param name="property"> 属性 </param>
        /// <returns></returns>
        internal static PropertyHandler GetPropertyHandler(this PropertyInfo property) =>
            property == null ? null : _propertyCache.GetOrAdd(property, PropertyHandler.Create);
    }
}
