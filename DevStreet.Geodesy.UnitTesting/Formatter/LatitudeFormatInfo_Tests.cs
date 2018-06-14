using DevStreet.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevStreet.Geodesy.UnitTesting.Formatter
{
    [TestClass]
    public class LatitudeFormatInfo_Tests
    {
        [TestMethod]
        public void FormatUnexpectedDataType_MyClass_Assert()
        {
            LatitudeFormatInfo info = new LatitudeFormatInfo();

            string result = string.Format(info, "{0:XYZ}", new MyClass());

            Assert.AreEqual(typeof(MyClass).FullName, result);
        }

        [TestMethod]
        public void FormatUnexpectedDataType_POCO_Assert()
        {
            int value = 12;
            LatitudeFormatInfo info = new LatitudeFormatInfo();

            string result = string.Format(info, "{0:XYZ}", value);

            Assert.AreEqual("XYZ", result);
        }

        public class MyClass
        {
        }
    }
}
