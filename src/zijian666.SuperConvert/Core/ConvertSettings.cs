using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
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
            Translators = new List<ITranslator>();
            FormatProviders = new Dictionary<Type, IFormatProvider>();
        }

        public TraceListener Trace { get; set; }

        public IConvertor<T> GetConvertor<T>(IConvertContext context)
            => (IConvertor<T>)_convertors.GetOrAdd(typeof(T), x => _builder.Build<T>());

        public List<ITranslator> Translators { get; }

        IEnumerable<ITranslator> IConvertSettings.Translators => Translators;

        public IStringSerializer StringSerializer { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public NumberFormatInfo NumberFormatInfo { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public Dictionary<Type, IFormatProvider> FormatProviders { get; }

        public StringSeparator StringSeparator { get; set; } = ",";
    }
}
