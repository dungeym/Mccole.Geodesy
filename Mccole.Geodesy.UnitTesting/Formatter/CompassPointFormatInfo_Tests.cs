using Mccole.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mccole.Geodesy.UnitTesting.Formatter
{
    [TestClass]
    public class CompassPointFormatInfo_Tests
    {
        [TestMethod]
        public void FormatUnexpectedDataType_MyClass_Assert()
        {
            CompassPointFormatInfo info = new CompassPointFormatInfo();

            string result = string.Format(info, "{0:XYZ}", new MyClass());

            Assert.AreEqual(typeof(MyClass).FullName, result);
        }

        [TestMethod]
        public void FormatUnexpectedDataType_POCO_Assert()
        {
            int value = 12;
            CompassPointFormatInfo info = new CompassPointFormatInfo();

            string result = string.Format(info, "{0:XYZ}", value);

            Assert.AreEqual("XYZ", result);
        }

        public class MyClass
        {
        }
    }
}
