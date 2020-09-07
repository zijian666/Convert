using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using zijian666.Core.Abstractions;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal class TraceConvertor<T> : ConvertorWrapper<T>, IConvertor<T>
    {
        private IConvertContext _context = new InnerConvertContext();
        public TraceConvertor(IConvertor<T> convertor) : base(convertor)
        {
        }

        public uint Priority => InnerConvertor.Priority;

        public ConvertResult<T> Convert(IConvertContext context, object input)
        {
            var trace = context.Settings.Trace;
            if (trace != null)
            {
                trace.WriteLine("调用：" + InnerConvertor.ToString());
                trace.IndentLevel++;
            }
            var result = InnerConvertor.Convert(context, input);
            if (trace != null)
            {
                trace.IndentLevel--;
            }
            var ex = result.Exception;
            if (ex is null)
            {
                trace?.WriteLine("返回：" + (_context.Convert<string>(result.Value).Value ?? "{null}"));
            }
            else
            {
                trace?.WriteLine("输入值：" + (input ?? "{null}"));
                trace?.WriteLine("输出类型：" + typeof(T).GetFriendlyName());
                trace?.WriteLine("异常：" + ex.ToString());
                ex.Data.Add("Convert", InnerConvertor);
                ex.Data.Add("InputValue", input);
                ex.Data.Add("OutputType", typeof(T));
            }

            trace?.WriteLine("");
            return result;
        }

        class InnerConvertContext : Dictionary<string, object>, IConvertContext
        {
            public IConvertSettings Settings { get; } = new InnerConvertSettings();

            public void Dispose() => Clear();
        }

        class InnerConvertSettings : IConvertSettings
        {

            public TraceListener Trace => null;

            public IEnumerable<ITranslator> Translators => Converts.Settings.Translators;

            public IStringSerializer StringSerializer => Converts.Settings.StringSerializer;

            public CultureInfo CultureInfo => Converts.Settings.CultureInfo;

            public NumberFormatInfo NumberFormatInfo => Converts.Settings.NumberFormatInfo;

            public Encoding Encoding => Converts.Settings.Encoding;

            public IConvertor<T1> GetConvertor<T1>(IConvertContext context) => Converts.Settings.GetConvertor<T1>(context);

            public Dictionary<Type, IFormatProvider> FormatProviders => Converts.Settings.FormatProviders;

            public StringSeparator StringSeparator => Converts.Settings.StringSeparator;

            public StringSplitOptions StringSplitOptions => Converts.Settings.StringSplitOptions;

            public Dictionary<Type, string> FormatStrings => Converts.Settings.FormatStrings;

            public IReflectCompiler ReflectCompiler => Converts.Settings.ReflectCompiler;
        }

        private string GetDebuggerDisplay()
        {
            return $"Trace({InnerConvertor.GetType().GetFriendlyName()},out={typeof(T).GetFriendlyName()})";
        }
    }
}
