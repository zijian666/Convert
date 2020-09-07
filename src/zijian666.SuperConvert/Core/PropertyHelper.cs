using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace zijian666.SuperConvert.Core
{
    /// <summary>
    /// 对象属性操作
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// 类型缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertiesCollection> _typeCache =
            new ConcurrentDictionary<Type, PropertiesCollection>();

        /// <summary>
        /// 根据类型获取操作属性的对象集合
        /// </summary>
        /// <param name="type"> 需要获取属性的类型 </param>
        /// <returns> </returns>
        public static PropertiesCollection GetByType(Type type) =>
            type == null ? null : _typeCache.GetOrAdd(type, Create);

        /// <summary>
        /// 根据类型创建一个操作属性的对象集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static PropertiesCollection Create(Type type)
        {
            var ps = type.GetProperties();
            var length = ps.Length;
            if (length == 0)
            {
                return PropertiesCollection.Empty;
            }
            var result = new List<PropertyHandler>(length);
            for (var i = 0; i < length; i++)
            {
                result.Add(PropertyHandler.Create(ps[i]));
            }
            return new PropertiesCollection(result);
        }

    }
}
