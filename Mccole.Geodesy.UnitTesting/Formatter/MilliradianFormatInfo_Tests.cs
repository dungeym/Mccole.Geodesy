using Mccole.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mccole.Geodesy.UnitTesting.Formatter
{
    [TestClass]
    public class MilliradianFormatInfo_Tests
    {
        [TestMethod]
        public void FormatUnexpectedDataType_MyClass_Assert()
        {
            MilliradianFormatInfo info = new MilliradianFormatInfo();

            string result = string.Format(info, "{0:XYZ}", new MyClass());

            Assert.AreEqual(typeof(MyClass).FullName, result);
        }

        [TestMethod]
        public void FormatUnexpectedDataType_POCO_Assert()
        {
            int value = 12;
            MilliradianFormatInfo info = new MilliradianFormatInfo();

            string result = string.Format(info, "{0:XYZ}", value);

            Assert.AreEqual("XYZ", result);
        }

        public class MyClass
        {
        }
    }
}
