using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Core
{
    public class ConvertSettings : IConvertSettings
    {
        private readonly ConvertorBuilder _builder;
        private readonly ConcurrentDictionary<Type, object> _convertors;

        public ConvertSettings(ConvertorBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _convertors = new ConcurrentDictionary<Type, object>();
        }

        public TraceListener Trace { get; set; }

        public IConvertor<T> GetConvertor<T>(IConvertContext context)
            => (IConvertor<T>)_convertors.GetOrAdd(typeof(T), x => _builder.Build<T>());
    }
}
