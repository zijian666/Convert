using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using zijian666.SuperConvert;

namespace UnitTest
{
    [TestClass]
    public class UnitTest5
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
        public void 动态类型与匿名类型转换测试()
        {
            var a = new { id = 1, name = "zijian666" };
            var b = a.ToDynamic();
            Assert.IsTrue((int)b.ID == a.id);
            Assert.IsTrue((string)b.NaMe == a.name);

            var c = ((object)b).ToDynamic();
            Assert.IsTrue((string)c.NamE == a.name);
            Assert.IsTrue(ReferenceEquals(b, c));
        }

        [TestMethod]
        public void 动态类型转换测试()
        {
            var a1 = Guid.NewGuid().ToString();
            var b = a1.ToDynamic();
            var a2 = Converts.To<string>(b, null);
            Assert.AreEqual(a1, a2);


            var c1 = "1";
            var d = c1.ToDynamic();
            var c2 = Converts.To<string>(d, null);
            Assert.AreEqual(c1, c2);
            var c3 = Converts.To<int>(d, 0);
            Assert.AreEqual(int.Parse(c1), c3);
        }

        [TestMethod]
        public void 测试动态类型属性测试()
        {
            var a = "aaa".ToDynamic();
            Assert.IsTrue((int)a.Length == 3);
            Assert.IsTrue((int)a.Length > 2);
            Assert.IsTrue((int)a.Length < 4);

            var b = 1.ToDynamic();
            Assert.IsTrue((int?)b.Length == null);

            var c = true.ToDynamic();
            Assert.IsTrue((bool)c);
            Assert.IsFalse(!(bool)c);

            var d = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = 999
            }.ToDynamic();
            Assert.IsTrue((int)d.id == 999);
            Assert.IsTrue((int)d.ID == 999);
            Assert.IsTrue((int)d["Id"] == 999);
            Assert.IsTrue((int?)d.Id.a.b.c.d.e.f.g == null);
            var now = DateTime.Parse(DateTime.Now.ToString());
            var e = new List<object>
            {
                666,
                new {id=1,name="zijian666"},
                new {obj = new {id=1,name="zijian666",time = now.ToString()}}
            }.ToDynamic();
            Assert.IsTrue((int)e[0] == 666);
            Assert.IsTrue((int)e[1].id == 1);
            Assert.IsTrue((string)e[1].NAME == "zijian666");

            Assert.IsTrue((string)e[2]["obj"]["iD"] == "1");
            Assert.IsTrue((string)e[2].obj["Name"] == "zijian666");
            Assert.AreEqual((DateTime)e[2]["obj"].time, now);



        }
    }
}
