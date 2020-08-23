
using System.Diagnostics;

namespace zijian666.SuperConvert.Interface
{
    public interface IConvertSettings
    {
        TraceListener Trace { get; set; }

        IConvertor<T> GetConvertor<T>(IConvertContext context);
    }
}
