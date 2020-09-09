using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using zijian666.SuperConvert;
using zijian666.SuperConvert.Json;

namespace UnitTest
{
    [TestClass]
    public class UnitTest4
    {
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

            var data = new
            {
                id = 1,
                name = "zijian666",
            };
            var json = data.To<string>();
            Console.WriteLine(json);
            Assert.AreEqual("{\"id\":1,\"name\":\"zijian666\"}", json);

            var result = json.Convert(data.GetType());
            Assert.IsTrue(result.Success);
            dynamic data2 = result.Value;
            Assert.AreEqual(data.id, data2.id);
            Assert.AreEqual(data.name, data2.name);
        }
    }
}
