using Microsoft.VisualStudio.TestTools.UnitTesting;
using zijian666.SuperConvert;


namespace UnitTest
{
    [TestClass]
    public class UnitTest3
    {
        [TestMethod]
        public void 自定义强转()
        {
            var i = 11;
            var my = i.To<MyClass>();
            Assert.AreEqual(i, my.ID);
        }

        public class MyClass
        {
            public int ID { get; set; }
            public static explicit operator MyClass(int i) => new MyClass() { ID = i };
        }

        [TestMethod]
        public void 自定义隐转()
        {
            var i = 11;
            var my = i.To<MyClass2>();
            Assert.AreEqual(i, my.ID);
        }

        public class MyClass2
        {
            public int ID { get; set; }
            public static implicit operator MyClass2(int i) => new MyClass2() { ID = i };
        }
    }
}
