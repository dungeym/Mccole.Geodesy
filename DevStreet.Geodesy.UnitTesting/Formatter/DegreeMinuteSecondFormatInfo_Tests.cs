using DevStreet.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevStreet.Geodesy.UnitTesting.Formatter
{
    [TestClass]
    public class DegreeMinuteSecondFormatInfo_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_Scale_LessThan_Zero_ThrowsException()
        {
            DegreeMinuteSecondFormatInfo subject = new DegreeMinuteSecondFormatInfo(-1);
        }

        [TestMethod]
        public void Format_Format_Null_CustomValue_Assert()
        {
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = info.Format(null, new MyClass(), info);

            Assert.AreEqual(typeof(MyClass).FullName, result);
        }

        [TestMethod]
        public void Format_Format_Null_DoubleValue_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = info.Format(null, degrees, info);

            Assert.AreEqual("041° 47' 46''", result);
        }

        [TestMethod]
        public void Format_MyClass_Assert()
        {
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = info.Format("{0:XYZ}", new MyClass(), info);

            Assert.AreEqual(typeof(MyClass).FullName, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Format_Value_Null_ThrowsException()
        {
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = info.Format("{0:XYZ}", null, info);
        }

        [TestMethod]
        public void FormatUnexpectedDataType_MyClass_Assert()
        {
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = string.Format(info, "{0:XYZ}", new MyClass());

            Assert.AreEqual(typeof(MyClass).FullName, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatUnexpectedDataType_Null_ThrowsException()
        {
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = string.Format(info, "{0:XYZ}", null);
        }

        [TestMethod]
        public void FormatUnexpectedDataType_POCO_Assert()
        {
            int value = 12;
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = string.Format(info, "{0:XYZ}", value);

            Assert.AreEqual("XYZ", result);
        }

        [TestMethod]
        public void ToDegreeMinute_Internal_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();

            string result = string.Format(info, "{0:DM}", degrees);

            Assert.AreEqual("041° 48'", result);
        }

        [TestMethod]
        public void UpdateScaleFromFormatString_Internal_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecondFormatInfo info = new DegreeMinuteSecondFormatInfo();
            info.Scale = 6;

            // Scale supplied.
            string result = string.Format(info, "{0:D}", degrees);

            Assert.AreEqual("041.796202°", result);
        }

        public class MyClass
        {
        }
    }
}
