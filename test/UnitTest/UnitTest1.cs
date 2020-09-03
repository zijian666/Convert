using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using zijian666.SuperConvert;
using zijian666.SuperConvert.Core;

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


        [TestMethod]
        public void 接口转换测试()
        {
            var list1 = "1,2,3,4".To<IList<int>>();
            Assert.AreEqual(list1?.Count, 4);
            Assert.AreEqual(list1[0], 1);
            Assert.AreEqual(list1[1], 2);
            Assert.AreEqual(list1[2], 3);
            Assert.AreEqual(list1[3], 4);

            var list11 = new object[] { "1", 2, 3, "4" }.To<IList<int>>();
            Assert.AreEqual(list11?.Count, 4);
            Assert.AreEqual(list11[0], 1);
            Assert.AreEqual(list11[1], 2);
            Assert.AreEqual(list11[2], 3);
            Assert.AreEqual(list11[3], 4);

            var list3 = "1,2,3,4".To<IEnumerable>().Cast<object>().ToList();
            Assert.AreEqual(list3?.Count, 4);
            Assert.AreEqual(list3[0], "1");
            Assert.AreEqual(list3[1], "2");
            Assert.AreEqual(list3[2], "3");
            Assert.AreEqual(list3[3], "4");

            var list2 = "1,2,3,4".To<IList>();
            Assert.AreEqual(list2?.Count, 4);
            Assert.AreEqual(list2[0], "1");
            Assert.AreEqual(list2[1], "2");
            Assert.AreEqual(list2[2], "3");
            Assert.AreEqual(list2[3], "4");
        }

        [TestMethod]
        public void 自定义转换参数()
        {
            var result = "1;2;3;;4".Convert<List<int>>(new ConvertSettings()
            {
                StringSeparator = ";",
                StringSplitOptions = StringSplitOptions.None,
            });
            Assert.AreEqual(result.Success, false);
            var list = "1;2;3;;4".Convert<List<int>>(new ConvertSettings()
            {
                StringSeparator = ";",
                StringSplitOptions = StringSplitOptions.RemoveEmptyEntries,
            }).Value;
            Assert.AreEqual(list?.Count, 4);
            Assert.AreEqual(list[0], 1);
            Assert.AreEqual(list[1], 2);
            Assert.AreEqual(list[2], 3);
            Assert.AreEqual(list[3], 4);
        }

        [TestMethod]
        public void 自定义格式化参数()
        {
            var format = "yyyyMMddHHmmssffffff";
            var enUS = CultureInfo.CreateSpecificCulture("en-US");
            var urPK = CultureInfo.CreateSpecificCulture("ur-PK");
            var time = DateTime.Now;

            var formatResult = time.ToString(format);
            var enUSResult = time.ToString(enUS);
            var urPKResult = time.ToString(urPK);

            var formatTest = time.Convert<string>(new ConvertSettings() { FormatStrings = { [typeof(DateTime)] = format } }).Value;
            var enUSTest = time.Convert<string>(new ConvertSettings() { FormatProviders = { [typeof(DateTime)] = enUS } }).Value;
            var urPKTest = time.Convert<string>(new ConvertSettings() { FormatProviders = { [typeof(DateTime)] = urPK } }).Value;

            Assert.AreEqual(formatResult, formatTest);
            Assert.AreEqual(enUSResult, enUSTest);
            Assert.AreEqual(urPKResult, urPKTest);
        }

        [TestMethod]
        public void 测试友好类名()
        {
            Assert.AreEqual(typeof(int).To<string>(), "int");
            Assert.AreEqual(typeof(char?).To<string>(), "char?");
            Assert.AreEqual(typeof(List<ICollection>).To<string>(), "List<ICollection>");
            Assert.AreEqual(typeof((int, long)).To<string>(), "(int, long)");
            Assert.AreEqual(typeof(List<>).To<string>(), "List<>");
            Assert.AreEqual(typeof(void).To<string>(), "void");
            Assert.AreEqual(typeof(Dictionary<List<long?>, Tuple<string, DateTime, Guid, (TimeSpan, int)>>).To<string>(),
                "Dictionary<List<long?>, Tuple<string, DateTime, Guid, (TimeSpan, int)>>");
            Assert.AreEqual(typeof(int*).To<string>(), "int*");
            Assert.AreEqual(typeof(int).MakeByRefType().To<string>(), "int&");
            Assert.AreEqual(typeof(int[]).To<string>(), "int[]");
        }

        enum MyEnum
        {
            A = 1,
            B = 2,
        }
        class MyClass1
        {
            public MyEnum E { get; set; }
            public MyEnum? E2 { get; set; }
        }

        [TestMethod]
        public void 测试可空枚举()
        {
            var a = 0;
            var b = a.To<MyEnum?>();
            Assert.AreEqual((MyEnum?)0, b);
            object c = null;
            var d = c.To<MyEnum?>();
            Assert.AreEqual(null, d);

        }

        [TestMethod]
        public void 测试可空枚举属性()
        {
            var a = new { E2 = (MyEnum?)null };
            var b = a.To<MyClass1>();
            Assert.AreEqual(a.E2, b.E2);


            var c = new { E2 = 0 };
            var d = c.To<MyClass1>();
            Assert.AreEqual((MyEnum)0, d.E2);
        }

        [TestMethod]
        public void 测试枚举()
        {
            {
                var a = 0;
                var b = a.To<MyEnum>();
                Assert.AreEqual((MyEnum)0, b);
            }
            {
                var a = 1;
                var b = a.To<MyEnum>();
                Assert.AreEqual(MyEnum.A, b);
            }
            {
                var a = 3;
                var b = a.To<MyEnum>(0);
                Assert.AreEqual((MyEnum)0, b);
            }
            {
                var a = BindingFlags.Static | BindingFlags.Public;
                var b = a.ToString().To<BindingFlags>(0);
                Assert.AreEqual(a, b);
            }
        }

        [TestMethod]
        public void 测试枚举属性()
        {
            {
                var a = new { E = 0 };
                var b = a.To<MyClass1>();
                Assert.AreEqual((MyEnum)a.E, b.E);
            }
            {
                var a = new { E = 1 };
                var b = a.To<MyClass1>();
                Assert.AreEqual(MyEnum.A, b.E);
            }
            {
                var a = new { E = 3 };
                var b = a.To<MyClass1>(null);
                Assert.AreEqual(null, b);
            }
        }


        [TestMethod]
        public void 测试数组转型()
        {
            var arr = new[] { "1", "2", "3" };
            var arr2 = arr.To<int[]>();
            Assert.AreEqual(3, arr2?.Length);
            Assert.AreEqual(1, arr2[0]);
            Assert.AreEqual(2, arr2[1]);
            Assert.AreEqual(3, arr2[2]);
        }

        [TestMethod]
        public void 键值对接口转换测试()
        {
            var dict = new Dictionary<string, string[]>()
            {
                [" 2015-1-1"] = new[] { "1", "2", "3", "4" }
            };

            var dict2 = dict.To<IDictionary<DateTime, List<int>>>();
            Assert.IsNotNull(dict2);
            Assert.AreEqual(1, dict2.Count);
            Assert.AreEqual(new DateTime(2015, 1, 1), dict2.Keys.First());
            Assert.AreEqual(4, dict2.Values.First().Count);
            Assert.AreEqual(1, dict2.Values.First()[0]);
            Assert.AreEqual(2, dict2.Values.First()[1]);
            Assert.AreEqual(3, dict2.Values.First()[2]);
            Assert.AreEqual(4, dict2.Values.First()[3]);

            var nv = new NameValueCollection()
            {
                [" 2015-1-1"] = "1,2,3,4"
            };

            var dict3 = nv.To<IDictionary>();
            Assert.IsNotNull(dict3);
            Assert.AreEqual(1, dict3.Count);
            Assert.AreEqual(" 2015-1-1", dict3.Keys.Cast<object>().First());
            Assert.AreEqual("1,2,3,4", dict3.Values.Cast<object>().First());

        }

        [TestMethod]
        public void 进阶功能()
        {
            var uri = "www.baidu.com".To<Uri>();
            Assert.IsNotNull(uri);
            Assert.AreEqual("http://www.baidu.com/", uri.AbsoluteUri);

            var arr = "1,2,3,4,5,6".To<int[]>();
            Assert.IsNotNull(arr);
            Assert.AreEqual(6, arr.Length);
            Assert.AreEqual(1, arr[0]);
            Assert.AreEqual(2, arr[1]);
            Assert.AreEqual(3, arr[2]);
            Assert.AreEqual(4, arr[3]);
            Assert.AreEqual(5, arr[4]);
            Assert.AreEqual(6, arr[5]);

            var arr2 = "1,2,3,4,5,6".To<ArrayList>();
            Assert.IsNotNull(arr2);
            Assert.AreEqual(6, arr2.Count);
            Assert.AreEqual("1", arr2[0]);
            Assert.AreEqual("2", arr2[1]);
            Assert.AreEqual("3", arr2[2]);
            Assert.AreEqual("4", arr2[3]);
            Assert.AreEqual("5", arr2[4]);
            Assert.AreEqual("6", arr2[5]);




            var user = new { id = "1", name = 123 }.To<User>();
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.ID);
            Assert.AreEqual("123", user.Name);

            var dict = new Dictionary<string, string[]>()
            {
                [" 2015-1-1"] = new[] { "1", "2", "3", "4" }
            };
            var dict2 = dict.To<Dictionary<DateTime, List<int>>>();
            Assert.IsNotNull(dict2);
            Assert.AreEqual(1, dict2.Count);
            Assert.AreEqual(new DateTime(2015, 1, 1), dict2.Keys.First());
            Assert.AreEqual(4, dict2.Values.First().Count);
            Assert.AreEqual(1, dict2.Values.First()[0]);
            Assert.AreEqual(2, dict2.Values.First()[1]);
            Assert.AreEqual(3, dict2.Values.First()[2]);
            Assert.AreEqual(4, dict2.Values.First()[3]);


            Console.WriteLine("1,2,3,4,5".To<ArrayList>());
            var table = new DataTable("x");
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Sex", typeof(bool));
            table.Rows.Add(1, "blqw1", true);
            table.Rows.Add(2, "blqw2", false);
            table.Rows.Add(3, "blqw3", true);
            table.Rows.Add(4, "blqw4", false);

            var list1 = table.To<List<NameValueCollection>>();
            Assert.IsNotNull(list1);
            Assert.AreEqual(4, list1.Count);
            Action<NameValueCollection, string, string, string> assert1 =
                (actual, id, name, sex) =>
                {
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(id, actual["id"]);
                    Assert.AreEqual(name, actual["Name"]);
                    Assert.AreEqual(sex, actual["Sex"]);
                };
            assert1(list1[0], "1", "blqw1", "true");
            assert1(list1[1], "2", "blqw2", "false");
            assert1(list1[2], "3", "blqw3", "true");
            assert1(list1[3], "4", "blqw4", "false");

            var list2 = table.To<List<User>>();
            Assert.IsNotNull(list2);
            Assert.AreEqual(4, list2.Count);
            Action<User, int, string, bool> assert2 =
                (actual, id, name, sex) =>
                {
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(id, actual.ID);
                    Assert.AreEqual(name, actual.Name);
                    Assert.AreEqual(sex, actual.Sex);
                };
            assert2(list2[0], 1, "blqw1", true);
            assert2(list2[1], 2, "blqw2", false);
            assert2(list2[2], 3, "blqw3", true);
            assert2(list2[3], 4, "blqw4", false);


            var list3 = table.To<List<Dictionary<string, object>>>();
            Assert.IsNotNull(list3);
            Assert.AreEqual(4, list3.Count);
            Action<Dictionary<string, object>, object, object, object> assert3 =
                (actual, id, name, sex) =>
                {
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(id, actual["id"]);
                    Assert.AreEqual(name, actual["Name"]);
                    Assert.AreEqual(sex, actual["Sex"]);
                };
            assert3(list3[0], 1, "blqw1", true);
            assert3(list3[1], 2, "blqw2", false);
            assert3(list3[2], 3, "blqw3", true);
            assert3(list3[3], 4, "blqw4", false);

            var list4 = table.To<List<Hashtable>>();
            Assert.IsNotNull(list4);
            Assert.AreEqual(4, list4.Count);
            Action<Hashtable, object, object, object> assert4 =
                (actual, id, name, sex) =>
                {
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(id, actual["id"]);
                    Assert.AreEqual(name, actual["Name"]);
                    Assert.AreEqual(sex, actual["Sex"]);
                };
            assert4(list4[0], 1, "blqw1", true);
            assert4(list4[1], 2, "blqw2", false);
            assert4(list4[2], 3, "blqw3", true);
            assert4(list4[3], 4, "blqw4", false);

            Action<DataRow, string, string, string> assert5 =
                (actual, id, name, sex) =>
                {
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(id, actual["id"] + "");
                    Assert.AreEqual(name, actual["Name"] + "");
                    Assert.AreEqual(sex, (actual["Sex"] + "").ToLowerInvariant());
                };
            Action<DataTable> assert6 =
                (actual) =>
                {
                    Assert.IsNotNull(actual);
                    Assert.AreEqual(4, actual.Rows.Count);
                    assert5(actual.Rows[0], "1", "blqw1", "true");
                    assert5(actual.Rows[1], "2", "blqw2", "false");
                    assert5(actual.Rows[2], "3", "blqw3", "true");
                    assert5(actual.Rows[3], "4", "blqw4", "false");
                };
            var tb1 = list1.To<DataTable>();
            assert6(tb1);
            var tb2 = list2.To<DataTable>();
            assert6(tb2);
            var tb3 = list3.To<DataTable>();
            assert6(tb3);
            var tb4 = list4.To<DataTable>();
            assert6(tb4);
        }

        [TestMethod]
        public void 空字符串转数组()
        {
            var str = "";
            var arr = str.To<string[]>();
            Assert.IsNotNull(arr);
            Assert.AreEqual(0, arr.Length);
        }

        class user
        {
            public long id { get; set; }
            public string name { get; set; }
            public DateTime date { get; set; }
        }

        [TestMethod]
        public void 异常栈展示()
        {
            try
            {
                var table1 = new DataTable("a");
                table1.Columns.Add("id", typeof(int));
                table1.Columns.Add("name", typeof(string));
                table1.Rows.Add(1, "赵");
                table1.Rows.Add(2, "钱");
                table1.Rows.Add(3, "孙");
                table1.Rows.Add(4, "李");

                var table2 = new DataTable("b");
                table2.Columns.Add("id", typeof(int));
                table2.Columns.Add("date", typeof(string));
                table2.Rows.Add(1, "2016-04-20");
                table2.Rows.Add(2, "2016-04-21");
                table2.Rows.Add(3, "2016-04-22A");
                table2.Rows.Add(4, "2016-04-23");

                var dataset = new DataSet();
                dataset.Tables.Add(table1);
                dataset.Tables.Add(table2);

                dataset.To<Dictionary<string, List<user>>>();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        [TestMethod]
        public void 优化错误信息()
        {
            var test = 123456;
            var a = test.Convert(typeof(Convert));
            if (a.Success || !a.Exception.Message.Contains("静态类型"))
            {
                Assert.Fail();
            }

            var b = test.Convert(typeof(List<>));
            if (b.Success || !b.Exception.Message.Contains("泛型定义类型"))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void 优化单值转数组的体验()
        {
            var x = 123456;
            //var a = x.To<string[]>();
            //Assert.AreEqual(1, a.Length);
            //Assert.AreEqual(x.ToString(), a[0]);
            //var b = x.To<int[]>();
            //Assert.AreEqual(1, b.Length);
            //Assert.AreEqual(x, b[0]);
            //var c = x.To<long[]>();
            //Assert.AreEqual(1, c.Length);
            //Assert.AreEqual((long)x, c[0]);
            var d = x.To<object[]>();
            Assert.AreEqual(1, d.Length);
            Assert.AreEqual(x, d[0]);


            var e = x.To<IList<string>>();
            Assert.AreEqual(1, e.Count);
            Assert.AreEqual(x.ToString(), e[0]);
            var f = x.To<IList<int>>();
            Assert.AreEqual(1, f.Count);
            Assert.AreEqual(x, f[0]);
            var g = x.To<IList<long>>();
            Assert.AreEqual(1, g.Count);
            Assert.AreEqual((long)x, g[0]);
            var h = x.To<IList<object>>();
            Assert.AreEqual(1, h.Count);
            Assert.AreEqual(x, h[0]);

            var i = x.To<IList>();
            Assert.AreEqual(1, i.Count);
            Assert.AreEqual(x, i[0]);


            var y = "2016-1-1";
            var j = y.To<DateTime[]>();
            Assert.AreEqual(1, j.Length);
            Assert.AreEqual(DateTime.Parse("2016-1-1"), j[0]);

        }

        [TestMethod]
        public void NullToObject()
        {
            object a = null;
            var b = a.To<object>();
            Assert.AreEqual(null, b);
        }
    }
}
