using System;
using System.Net;
using System.Text;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Translator
{
    public class StringTranslator : ITranslator
    {
        public bool CanTranslate(Type type) =>
              typeof(IFormattable).IsAssignableFrom(type)
              || type == typeof(StringBuilder)
              || type == typeof(Uri)
              || type == typeof(IPAddress);

        public object Translate(IConvertContext context, object input)
            => input switch
            {
                StringBuilder stringBuilder => stringBuilder.ToString(),
                Uri uri => uri.ToString(),
                IPAddress ipAddress => ipAddress.ToString(),
                IFormattable formattable => context.Settings.Format(formattable),
                _ => null,
            };
    }
}
