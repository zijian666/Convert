﻿using System;
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
        private Dictionary<Type, string> _formatStrings;
        private Dictionary<Type, IFormatProvider> _formatProviders;
        private List<ITranslator> _translators;

        public ConvertSettings(ConvertorBuilder builder)
        {
            _builder = builder;
            _convertors = _builder == null ? null : new ConcurrentDictionary<Type, object>();
        }

        public ConvertSettings()
        : this(null)
        {

        }

        public TraceListener Trace { get; set; }

        public IConvertor<T> GetConvertor<T>(IConvertContext context)
            => (IConvertor<T>)_convertors?.GetOrAdd(typeof(T), x => _builder.Build<T>());

        public List<ITranslator> Translators
        {
            get
            {
                if (_translators == null)
                {
                    _translators = new List<ITranslator>();
                }
                return _translators;
            }
        }

        public Dictionary<Type, IFormatProvider> FormatProviders
        {
            get
            {
                if (_formatProviders == null)
                {
                    _formatProviders = new Dictionary<Type, IFormatProvider>();
                }

                return _formatProviders;
            }
        }

        public Dictionary<Type, string> FormatStrings
        {
            get
            {
                if (_formatStrings == null)
                {
                    _formatStrings = new Dictionary<Type, string>();
                }
                return _formatStrings;
            }
        }

        public IStringSerializer StringSerializer { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public NumberFormatInfo NumberFormatInfo { get; set; }

        public Encoding Encoding { get; set; }


        public StringSeparator StringSeparator { get; set; }

        public StringSplitOptions StringSplitOptions { get; set; }

        Dictionary<Type, IFormatProvider> IConvertSettings.FormatProviders => _formatProviders;
        IEnumerable<ITranslator> IConvertSettings.Translators => _translators;

    }
}
