using System;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor.Number
{
    class Int32Convertor : IConvertor<int>
    {
        public uint Priority => 1;

        public ConvertResult<int> Convert(IConvertContext context, object input)
        {
            try
            {
                return System.Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
