using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Base
{
    internal class TraceConvertor<T> : ConvertorWrapper<T>, IConvertor<T>
    {
        public TraceConvertor(IConvertor<T> convertor) : base(convertor)
        {
        }

        public uint Priority => InnerConvertor.Priority;

        public ConvertResult<T> Convert(IConvertContext context, object input)
        {
            var trace = context.Settings.Trace;
            if (trace != null)
            {
                trace.WriteLine("调用： " + InnerConvertor.ToString());
                trace.IndentLevel++;
            }
            var result = InnerConvertor.Convert(context, input);
            if (trace != null)
            {
                trace.IndentLevel--;
            }
            var ex = result.Exception;
            if (ex is not null)
            {
                trace?.WriteLine("输入值： " + (input ?? "{null}"));
                trace?.WriteLine("输出类型： " + typeof(T).GetFriendlyName());
                trace?.WriteLine("异常： " + ex.ToString());
                ex.Data.Add("Convert", InnerConvertor);
                ex.Data.Add("InputValue", input);
                ex.Data.Add("OutputType", typeof(T));
            }
            else
            {
                trace?.WriteLine("返回:" + (result.Value?.ToString() ?? "{null}"));
            }
            return result;
        }

    }
}
