using System;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Extensions;
using zijian666.SuperConvert.Interface;

namespace zijian666.SuperConvert.Convertor
{
    public class UriConvertor : AllowNullConvertor<Uri>, IFrom<string, Uri>
    {
        public ConvertResult<Uri> From(IConvertContext context, string input)
        {
            if (input == null)
            {
                return default;
            }
            Uri result;
            input = input.TrimStart();
            if ((input.Length > 10) && (input[6] != '/'))
            {
                if (Uri.TryCreate("http://" + input, UriKind.Absolute, out result))
                {
                    return result;
                }
            }

            if (Uri.TryCreate(input, UriKind.Absolute, out result))
            {
                return result;
            }
            return context.ConvertFail(this, input);
        }
    }
}
