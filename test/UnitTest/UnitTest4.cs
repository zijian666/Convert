using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.Json;
using zijian666.SuperConvert;
using zijian666.SuperConvert.Json;

namespace UnitTest
{
    [TestClass]
    public class UnitTest4
    {
        [TestInitialize]
        public void Before()
        {
            if (!Debugger.IsAttached)
            {
                Converts.Settings.Trace = new TextWriterTraceListener(Console.Out);
            }
        }

        [TestMethod]
        public void FormatterConverter()
        {
            var converter = Converts.GetFormatterConverter();
            var result = converter.ToInt32("123");
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void JsonConvertor()
        {
            Converts.Settings.Protocol = "json";
            Converts.SetStringSerializer(new JsonStringSerializer());

            {
                var data = new
                {
                    id = 1,
                    name = "zijian666",
                };
                var json = data.To<string>();
                Console.WriteLine(json);
                Assert.AreEqual(JsonSerializer.Serialize(data), json);

                var result = json.Convert(data.GetType());
                Assert.IsTrue(result.Success);
                dynamic data2 = result.Value;
                Assert.AreEqual(data.id, data2.id);
                Assert.AreEqual(data.name, data2.name);
            }

            {
                var data = new[]
                {
                    new {id = 1, name = "zijian666"},
                    new {id = 2, name = "blqw"}
                };
                var json = data.To<string>();
                Console.WriteLine(json);
                Assert.AreEqual(JsonSerializer.Serialize(data), json);

                var result = json.Convert(data.GetType());
                Assert.IsTrue(result.Success);
                dynamic data2 = result.Value;
                Assert.AreEqual(data[0].id, data2[0].id);
                Assert.AreEqual(data[0].name, data2[0].name);
                Assert.AreEqual(data[1].id, data2[1].id);
                Assert.AreEqual(data[1].name, data2[1].name);
            }

            {
                var data = new Dictionary<string, dynamic>
                {
                    ["1"] = new { id = 1, name = "zijian666" },
                    ["2"] = new { id = 2, name = "blqw" }
                };
                var json = data.To<string>();
                Console.WriteLine(json);
                Assert.AreEqual(JsonSerializer.Serialize(data), json);

                var type = typeof(Dictionary<,>).MakeGenericType(typeof(string), ((object)data["1"]).GetType());
                var result = json.Convert(type);
                Assert.IsTrue(result.Success);
                dynamic data2 = result.Value;
                Assert.AreEqual(data["1"].id, data2["1"].id);
                Assert.AreEqual(data["1"].name, data2["1"].name);
                Assert.AreEqual(data["2"].id, data2["2"].id);
                Assert.AreEqual(data["2"].name, data2["2"].name);
            }

            {
                var data = new List<dynamic>
                {
                    new {id = 1, name = "zijian666"},
                    new {id = 2, name = "blqw"}
                };
                var json = data.To<string>();
                Console.WriteLine(json);
                Assert.AreEqual(JsonSerializer.Serialize(data), json);

                var type = typeof(List<>).MakeGenericType(((object)data[0]).GetType());
                var result = json.Convert(type);
                Assert.IsTrue(result.Success);
                dynamic data2 = result.Value;
                Assert.AreEqual(data[0].id, data2[0].id);
                Assert.AreEqual(data[0].name, data2[0].name);
                Assert.AreEqual(data[1].id, data2[1].id);
                Assert.AreEqual(data[1].name, data2[1].name);
            }

            {
                var data = new Dictionary<string, dynamic>
                {
                    ["1"] = new { id = 1, name = "zijian666" },
                    ["2"] = new { id = 2, name = "blqw" }
                };
                var json = data.To<string>();
                Console.WriteLine(json);
                Assert.AreEqual(JsonSerializer.Serialize(data), json);

                var result = json.Convert<NameValueCollection>();
                Assert.IsTrue(result.Success);
                var data2 = result.Value;

                Assert.AreEqual(JsonSerializer.Serialize(data["1"]), data2["1"]);
                Assert.AreEqual(JsonSerializer.Serialize(data["2"]), data2["2"]);
            }


        }

    }
}
