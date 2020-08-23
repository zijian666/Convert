using System.Diagnostics;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Core
{
    class CombinConvertSettings : IConvertSettings
    {
        private readonly IConvertSettings _settings1;
        private readonly IConvertSettings _settings2;

        public CombinConvertSettings(IConvertSettings settings1, IConvertSettings settings2)
        {
            _settings1 = settings1;
            _settings2 = settings2;
            Trace = _settings1?.Trace ?? _settings2?.Trace;
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
    }
}
