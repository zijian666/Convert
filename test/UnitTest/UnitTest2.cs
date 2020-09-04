using Microsoft.VisualStudio.TestTools.UnitTesting;
using zijian666.SuperConvert;
using zijian666.SuperConvert.Convertor.Base;
using zijian666.SuperConvert.Core;
using zijian666.SuperConvert.Interface;

namespace UnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void 自定义转换器()
        {
            var i = 11;
            var my = i.To<MyClass>();
            Assert.AreEqual(i, my.ID);
        }
        public class MyClass
        {
            public int ID { get; set; }
        }

        public class MyConvertor : AllowNullConvertor<MyClass>, IFrom<int, MyClass>
        {
            public ConvertResult<MyClass> From(IConvertContext context, int input)
            {
                return new MyClass() { ID = input };
            }
        }
    }

}
