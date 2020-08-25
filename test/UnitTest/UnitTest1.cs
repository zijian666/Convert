using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using zijian666.SuperConvert;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime Birthday { get; set; }
            public bool Sex { get; set; }
        }
        class MyClass
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        [TestMethod]
        public void 一般转换测试()
        {
            Assert.AreEqual(1, "1".To<int>());
            Assert.AreEqual(true, "true".To<bool>());
            Assert.AreEqual(false, 0.To<bool>());
            Assert.AreEqual(true, 1.To<bool>());
            Assert.AreEqual(DateTime.Parse("2018-02-20 16:50:02"), "2018-02-20 16:50:02".To<DateTime>());
            Assert.AreEqual(DateTime.Parse("2018.02.20"), "2018.02.20".To<DateTime>());
            Assert.AreEqual(DateTime.Parse("16:50:09"), "16:50:09".To<DateTime>());
            Assert.AreEqual(1, "1".To<int>());
            Assert.AreEqual(null, "".To<int?>());
            Assert.AreEqual(1, "1".To<int?>());
        }

        [TestMethod]
        public void 泛型转换测试()
        {
            var list = "1,2,3,4".To<List<int>>();
            Assert.AreEqual(list?.Count, 4);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list[2], 3);
            Assert.AreEqual(list[3], 4);

            var table = new DataTable("b");
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("birthday", typeof(string));
            table.Rows.Add(1, "2016-04-20");
            table.Rows.Add(2, "2016-04-21");
            table.Rows.Add(3, "2016-04-22");
            table.Rows.Add(4, "2016-04-23");
            var dataset = new DataSet();
            dataset.Tables.Add(table);
            var dict = dataset.To<Dictionary<string, List<User>>>();

            Assert.AreEqual(dict?.Count, 1);
            Assert.AreEqual(dict["b"].Count, 4);
            Assert.AreEqual(dict["b"][0].ID, 1L);
            Assert.AreEqual(dict["b"][1].ID, 2L);
            Assert.AreEqual(dict["b"][2].ID, 3L);
            Assert.AreEqual(dict["b"][3].ID, 4L);
            Assert.AreEqual(dict["b"][0].Birthday, DateTime.Parse("2016-04-20"));
            Assert.AreEqual(dict["b"][1].Birthday, DateTime.Parse("2016-04-21"));
            Assert.AreEqual(dict["b"][2].Birthday, DateTime.Parse("2016-04-22"));
            Assert.AreEqual(dict["b"][3].Birthday, DateTime.Parse("2016-04-23"));
        }

    }
}
