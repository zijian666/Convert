using System;
using System.Collections.Concurrent;
using zijian666.Core.Abstractions;

namespace zijian666.SuperConvert.Core
{
    public class StringSerializerCollection
    {
        private readonly ConcurrentDictionary<string, IStringSerializer> _items
            = new ConcurrentDictionary<string, IStringSerializer>(StringComparer.OrdinalIgnoreCase);

        public static string DefaultProtocol { get; set; } = "json";

        public void Register(IStringSerializer serializer)
            => _items[serializer.Protocol] = serializer;

        public IStringSerializer this[string protocol]
            => _items.TryGetValue(protocol, out var value) ? value : null;

        public bool IsExists(string protocol)
            => _items.ContainsKey(protocol);

        public IStringSerializer Default
            => this[DefaultProtocol];
    }
}
