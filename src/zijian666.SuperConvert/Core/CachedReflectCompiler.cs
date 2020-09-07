using System;
using System.Collections.Concurrent;
using System.Reflection;
using zijian666.Core.Abstractions;

namespace zijian666.SuperConvert.Core
{
    internal class CachedReflectCompiler : IReflectCompiler
    {
        private readonly IReflectCompiler _reflectCompiler;

        public CachedReflectCompiler(IReflectCompiler reflectCompiler)
            => _reflectCompiler = reflectCompiler ?? throw new ArgumentNullException(nameof(reflectCompiler));

        private readonly ConcurrentDictionary<MemberInfo, Delegate> _cache = new ConcurrentDictionary<MemberInfo, Delegate>();

        public ObjectCreator<Object> CompileCreator<Object>(ConstructorInfo constructor)
            => (ObjectCreator<Object>)_cache.GetOrAdd(constructor, x => _reflectCompiler.CompileCreator<object>((ConstructorInfo)x));

        public MemberGetter<Value> CompileGetter<Value>(PropertyInfo property)
            => (MemberGetter<Value>)_cache.GetOrAdd(property, x => _reflectCompiler.CompileGetter<Value>((PropertyInfo)x));

        public MemberSetter<Value> CompileSetter<Value>(PropertyInfo property)
            => (MemberSetter<Value>)_cache.GetOrAdd(property, x => _reflectCompiler.CompileSetter<Value>((PropertyInfo)x));

        public MemberGetter<Value> CompileGetter<Value>(FieldInfo field)
            => (MemberGetter<Value>)_cache.GetOrAdd(field, x => _reflectCompiler.CompileGetter<Value>((FieldInfo)x));

        public MemberSetter<Value> CompileSetter<Value>(FieldInfo field)
            => (MemberSetter<Value>)_cache.GetOrAdd(field, x => _reflectCompiler.CompileSetter<Value>((FieldInfo)x));

        public MethodCaller<Result> CompileCaller<Result>(MethodInfo method)
            => (MethodCaller<Result>)_cache.GetOrAdd(method, x => _reflectCompiler.CompileCaller<Result>((MethodInfo)x));
        public static IReflectCompiler Build(IReflectCompiler reflectCompiler)
        {
            if (reflectCompiler is CachedReflectCompiler cached)
            {
                return cached;
            }
            return reflectCompiler == null ? null : new CachedReflectCompiler(reflectCompiler);
        }
    }
}
