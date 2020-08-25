﻿using System;
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
            var convertor = Build<T>();
            if (convertor != null)
            {
                yield return new MatchedConvertor<T>(convertor, 1, MacthedLevel.Subclass);
            }
        }

        private bool CanBuild<T>()
        {
            if (typeof(T).IsMetaType() || typeof(T).IsValueType || typeof(T).IsEnum || typeof(T).IsArray)
            {
                return false;
            }
            if (typeof(T).Namespace.StartsWith("System."))
            {
                return false;
            }
            if (typeof(T).Namespace.StartsWith("Microsoft."))
            {
                return false;
            }

            if (typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Length == 0)
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
