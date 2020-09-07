using Microsoft.VisualStudio.TestTools.UnitTesting;
using zijian666.SuperConvert;

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
    }
}
