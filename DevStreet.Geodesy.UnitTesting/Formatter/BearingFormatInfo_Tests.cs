using DevStreet.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevStreet.Geodesy.UnitTesting.Formatter
{
    [TestClass]
    public class BearingFormatInfo_Tests
    {
        [TestMethod]
        public void FormatUnexpectedDataType_MyClass_Assert()
        {
            BearingFormatInfo info = new BearingFormatInfo();

            string result = string.Format(info, "{0:XYZ}", new MyClass());

            Assert.AreEqual(typeof(MyClass).FullName, result);
        }

        [TestMethod]
        public void FormatUnexpectedDataType_POCO_Assert()
        {
            int value = 12;
            BearingFormatInfo info = new BearingFormatInfo();

            string result = string.Format(info, "{0:XYZ}", value);

            Assert.AreEqual("XYZ", result);
        }

        public class MyClass
        {
        }
    }
}
