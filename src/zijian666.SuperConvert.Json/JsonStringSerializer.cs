using System.Text.Json;
using zijian666.Core.Abstractions;

namespace zijian666.SuperConvert.Json
{
    public class JsonStringSerializer : IStringSerializer
    {
        public string ToString(object value) =>
            value is JsonElement ele ? ele.GetString() : JsonSerializer.Serialize(value);

        public object ToObject(string value, System.Type type) => System.Text.Json.JsonSerializer.Deserialize(value, type);

        public string Protocol => "json";
    }
}
