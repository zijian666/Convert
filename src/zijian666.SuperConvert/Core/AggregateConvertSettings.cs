using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using zijian666.Core.Abstractions;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Core
{
    class AggregateConvertSettings : IConvertSettings
    {
        private readonly IConvertSettings _settings1;
        private readonly IConvertSettings _settings2;

        public AggregateConvertSettings(IConvertSettings settings1, IConvertSettings settings2)
        {
            _settings1 = settings1;
            _settings2 = settings2;
            Trace = _settings1?.Trace ?? _settings2?.Trace;
            if (_settings1?.Translators.IsEmpty() ?? true)
            {
                Translators = _settings2?.Translators;
            }
            else if (_settings2?.Translators.IsEmpty() ?? true)
            {
                Translators = _settings1?.Translators;
            }
            else
            {
                Translators = _settings1.Translators.Union(_settings2.Translators);
            }
        }

        public TraceListener Trace { get; set; }

        public IConvertor<T> GetConvertor<T>(IConvertContext context)
        {
            var conv1 = _settings1?.GetConvertor<T>(context);
            var conv2 = _settings2?.GetConvertor<T>(context);
            if (conv1 == null)
            {
                return conv2;
            }
            if (conv2 == null)
            {
                return conv1;
            }
            return new AggregateConvertor<T>(conv1, conv2);
        }

        public IEnumerable<ITranslator> Translators { get; }

        public IStringSerializer StringSerializer => _settings1?.StringSerializer ?? _settings2?.StringSerializer;

        public CultureInfo CultureInfo => _settings1?.CultureInfo ?? _settings2?.CultureInfo ?? CultureInfo.CurrentUICulture;

        public NumberFormatInfo NumberFormatInfo
            => _settings1?.NumberFormatInfo ?? _settings2?.NumberFormatInfo ?? NumberFormatInfo.CurrentInfo;

        public Encoding Encoding
            => _settings1?.Encoding ?? _settings2?.Encoding ?? Encoding.UTF8;

        public Dictionary<Type, IFormatProvider> FormatProviders
            => _settings1?.FormatProviders ?? _settings2?.FormatProviders;

        public StringSeparator StringSeparator
            => _settings1?.StringSeparator ?? _settings2?.StringSeparator;

        public StringSplitOptions StringSplitOptions
            => _settings1?.StringSplitOptions ?? _settings2?.StringSplitOptions ?? StringSplitOptions.RemoveEmptyEntries;

        public Dictionary<Type, string> FormatStrings
            => _settings1?.FormatStrings ?? _settings2?.FormatStrings;

        public IReflectCompiler ReflectCompiler => _settings1?.ReflectCompiler ?? _settings2.ReflectCompiler;
    }
}
