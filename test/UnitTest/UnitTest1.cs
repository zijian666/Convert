using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using zijian666.SuperConvert;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void test()
        {

            Converts.Settings.Trace = null;
            Assert.AreEqual(1, "1".To<int>());
            try
            {
                "a".To<int>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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

    }
}
