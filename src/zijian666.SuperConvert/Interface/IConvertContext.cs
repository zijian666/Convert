using System;

namespace zijian666.SuperConvert.Interface
{
    public interface IConvertContext : IDisposable
    {

        IConvertSettings Settings { get; }

        object this[string key] { get; set; }

    }
}
