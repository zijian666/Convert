using System;
using System.Collections.Generic;
using System.Reflection;
using zijian666.SuperConvert.Convertor;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Factory
{
    public class ObjectConvertorFactory : IConvertorFactory
    {
        public IEnumerable<MatchedConvertor<T>> Create<T>()
        {
            if (CanBuild<T>())
            {
                var convertor = new ObjectConvertor<T>();
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Subclass);
            }
            else if (typeof(object) == typeof(T))
            {
                var convertor = new ObjectConvertor<T>();
                yield return new MatchedConvertor<T>(convertor, convertor.Priority, MacthedLevel.Full);
            }
        }

        private bool CanBuild<T>()
        {
            var type = typeof(T);
            if (type.IsMetaType() || type.IsValueType || type.IsEnum || type.IsArray || type.Namespace == null)
            {
                return false;
            }
            if (type.Namespace.StartsWith("System."))
            {
                return false;
            }
            if (type.Namespace.StartsWith("Microsoft."))
            {
                return false;
            }

            if (type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Length == 0)
            {
                return false;
            }

            try
            {
                return Activator.CreateInstance<T>() != null;
            }
            catch
            {
                return false;
            }
        }

        private IConvertor<T> Build<T>() => CanBuild<T>() ? new ObjectConvertor<T>() : null;
    }
}
