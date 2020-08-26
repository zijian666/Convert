
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using zijian666.SuperConvert.Core;

namespace zijian666.SuperConvert.Interface
{
    public interface IConvertSettings
    {
        TraceListener Trace { get; }

        IEnumerable<ITranslator> Translators { get; }

        IStringSerializer StringSerializer { get; }

        CultureInfo CultureInfo { get; }

        NumberFormatInfo NumberFormatInfo { get; }
        Encoding Encoding { get; }

        IConvertor<T> GetConvertor<T>(IConvertContext context);

        Dictionary<Type, IFormatProvider> FormatProviders { get; }

        Dictionary<Type, string> FormatStrings { get; }

        StringSeparator StringSeparator { get; }

        StringSplitOptions StringSplitOptions { get; }
    }
}
