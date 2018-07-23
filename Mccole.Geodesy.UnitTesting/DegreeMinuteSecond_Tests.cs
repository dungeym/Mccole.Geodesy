using Mccole.Geodesy.Extension;
using Mccole.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mccole.Geodesy.UnitTesting
{
    [TestClass]
    public class DegreeMinuteSecond_Tests
    {
        [TestMethod]
        public void Constructor_Degree_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            Assert.IsTrue(subject.Degree.WithinTolerance(41), "Value == {0}", subject.Degree);
        }

        [TestMethod]
        public void Constructor_Degrees_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            Assert.IsTrue(subject.Degrees.WithinTolerance(degrees), "Value == {0}", subject.Degrees);
        }

        [TestMethod]
        public void Constructor_Mils_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            Assert.IsTrue(subject.Mils.WithinTolerance(0.006530656496875), "Value == {0}", subject.Mils);
        }

        [TestMethod]
        public void Constructor_Minute_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            Assert.IsTrue(subject.Minute.WithinTolerance(47), "Value == {0}", subject.Minute);
        }

        [TestMethod]
        public void Constructor_Minutes_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            Assert.IsTrue(subject.Minutes.WithinTolerance(47.7720948000001), "Value == {0}", subject.Minutes);
        }

        [TestMethod]
        public void Constructor_SecondAssert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            Assert.IsTrue(subject.Second.WithinTolerance(46), "Value == {0}", subject.Second);
        }

        [TestMethod]
        public void Constructor_Seconds_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            Assert.IsTrue(subject.Seconds.WithinTolerance(46.3256880000117), "Value == {0}", subject.Seconds);
        }

        [TestMethod]
        public void ToBearing_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToBearing();

            Assert.AreEqual("042°", result);
        }

        [TestMethod]
        public void ToBearing_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms3";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToBearing(format);

            Assert.AreEqual("041° 47' 46.326''", result);
        }

        [TestMethod]
        public void ToBearing_Negative_Assert()
        {
            var degrees = -41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToBearing();

            Assert.AreEqual("318°", result);
        }

        [TestMethod]
        public void ToBearing_Provider_Assert()
        {
            var degrees = 41.79620158;
            BearingFormatInfo provider = new BearingFormatInfo();
            provider.Separator = "-";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToBearing(provider);

            Assert.AreEqual("042°", result);
        }

        [TestMethod]
        public void ToBearing_Provider_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms5";
            IFormatProvider provider = new BearingFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToBearing(provider, format);

            Assert.AreEqual("041° 47' 46.32569''", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToBearing_Provider_Invalid_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = new System.Globalization.DateTimeFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToBearing(provider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToBearing_Provider_Null_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = default(IFormatProvider);
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToBearing(provider);
        }

        [TestMethod]
        public void ToCompassPoint_Cardinalt_Assert()
        {
            var degrees = 10.5;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);
            IFormatProvider provider = new CompassPointFormatInfo(CompassPointPrecision.Cardinal);

            string result = subject.ToCompassPoint(provider);

            Assert.AreEqual("N", result);
        }

        [TestMethod]
        public void ToCompassPoint_Default_Assert()
        {
            var degrees = 10.5;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToCompassPoint();

            Assert.AreEqual("N", result);
        }

        [TestMethod]
        public void ToCompassPoint_Intercardinal_Assert()
        {
            var degrees = 44.5;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);
            IFormatProvider provider = new CompassPointFormatInfo(CompassPointPrecision.Intercardinal);

            string result = subject.ToCompassPoint(provider);

            Assert.AreEqual("NE", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToCompassPoint_Provider_Invalid_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = new System.Globalization.DateTimeFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToCompassPoint(provider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToCompassPoint_Provider_Null_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = default(IFormatProvider);
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToCompassPoint(provider);
        }

        [TestMethod]
        public void ToCompassPoint_SecondaryIntercardinal_Assert()
        {
            var degrees = 21.5;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);
            IFormatProvider provider = new CompassPointFormatInfo(CompassPointPrecision.SecondaryIntercardinal);

            string result = subject.ToCompassPoint(provider);

            Assert.AreEqual("NNE", result);
        }

        [TestMethod]
        public void ToLatitude_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLatitude();

            Assert.AreEqual("41° 47' 46'' N", result);
        }

        [TestMethod]
        public void ToLatitude_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms3";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLatitude(format);

            Assert.AreEqual("41° 47' 46.326'' N", result);
        }

        [TestMethod]
        public void ToLatitude_Provider_Assert()
        {
            var degrees = 41.79620158;
            LatitudeFormatInfo provider = new LatitudeFormatInfo();
            provider.Separator = "-";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLatitude(provider);

            Assert.AreEqual("41°-47'-46''-N", result);
        }

        [TestMethod]
        public void ToLatitude_Provider_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms5";
            IFormatProvider provider = new LatitudeFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLatitude(provider, format);

            Assert.AreEqual("41° 47' 46.32569'' N", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToLatitude_Provider_Invalid_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = new System.Globalization.DateTimeFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToLatitude(provider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToLatitude_Provider_Null_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = default(IFormatProvider);
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToLatitude(provider);
        }

        [TestMethod]
        public void ToLatitude_South_Assert()
        {
            var degrees = -41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLatitude();

            Assert.AreEqual("41° 47' 46'' S", result);
        }

        [TestMethod]
        public void ToLongitude_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLongitude();

            Assert.AreEqual("041° 47' 46'' E", result);
        }

        [TestMethod]
        public void ToLongitude_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms3";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLongitude(format);

            Assert.AreEqual("041° 47' 46.326'' E", result);
        }

        [TestMethod]
        public void ToLongitude_Provider_Assert()
        {
            var degrees = 41.79620158;
            LongitudeFormatInfo provider = new LongitudeFormatInfo();
            provider.Separator = "-";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLongitude(provider);

            Assert.AreEqual("041°-47'-46''-E", result);
        }

        [TestMethod]
        public void ToLongitude_Provider_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms5";
            IFormatProvider provider = new LongitudeFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLongitude(provider, format);

            Assert.AreEqual("041° 47' 46.32569'' E", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToLongitude_Provider_Invalid_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = new System.Globalization.DateTimeFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToLongitude(provider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToLongitude_Provider_Null_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = default(IFormatProvider);
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToLongitude(provider);
        }

        [TestMethod]
        public void ToLongitude_West_Assert()
        {
            var degrees = -41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToLongitude();

            Assert.AreEqual("041° 47' 46'' W", result);
        }

        [TestMethod]
        public void ToMils_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToMils();

            Assert.AreEqual("711 mils", result);
        }

        [TestMethod]
        public void ToMils_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms3";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToMils(format);

            Assert.AreEqual("710.535 mils", result);
        }

        [TestMethod]
        public void ToMils_Negative_Assert()
        {
            var degrees = -41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToMils();

            Assert.AreEqual("5409 mils", result);
        }

        [TestMethod]
        public void ToMils_Provider_Assert()
        {
            var degrees = 41.79620158;
            MilliradianFormatInfo provider = new MilliradianFormatInfo();
            provider.Separator = "-";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToMils(provider);

            Assert.AreEqual("711-mils", result);
        }

        [TestMethod]
        public void ToMils_Provider_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms5";
            IFormatProvider provider = new MilliradianFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToMils(provider, format);

            Assert.AreEqual("710.53543 mils", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToMils_Provider_Invalid_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = new System.Globalization.DateTimeFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToMils(provider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToMils_Provider_Null_ThrowsException()
        {
            var degrees = 41.79620158;
            IFormatProvider provider = default(IFormatProvider);
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            subject.ToMils(provider);
        }

        [TestMethod]
        public void ToString_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToString();

            Assert.AreEqual("041° 47' 46''", result);
        }

        [TestMethod]
        public void ToString_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms3";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToString(format);

            Assert.AreEqual("041° 47' 46.326''", result);
        }

        [TestMethod]
        public void ToString_Provider_Assert()
        {
            var degrees = 41.79620158;
            DegreeMinuteSecondFormatInfo provider = new DegreeMinuteSecondFormatInfo();
            provider.Separator = "-";
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToString(provider);

            Assert.AreEqual("041°-47'-46''", result);
        }

        [TestMethod]
        public void ToString_Provider_Format_Assert()
        {
            var degrees = 41.79620158;
            string format = "dms5";
            IFormatProvider provider = new DegreeMinuteSecondFormatInfo();
            DegreeMinuteSecond subject = new DegreeMinuteSecond(degrees);

            string result = subject.ToString(provider, format);

            Assert.AreEqual("041° 47' 46.32569''", result);
        }

        [TestMethod]
        public void TryParse_DMS_Assert()
        {
            DegreeMinuteSecond dms = null;
            string value = "041° 47' 46.32569''";

            bool result = DegreeMinuteSecond.TryParse(value, out dms);

            Assert.IsTrue(result);
            Assert.IsTrue(dms.Degrees.WithinTolerance(41.79620158));
        }

        [TestMethod]
        public void TryParse_DMS_South_Assert()
        {
            DegreeMinuteSecond dms = null;
            string value = "041° 47' 46.32569'' S";

            bool result = DegreeMinuteSecond.TryParse(value, out dms);

            Assert.IsTrue(result);
            Assert.IsTrue(dms.Degrees.WithinTolerance(-41.7962015805556), dms.Degrees.ToString());
        }

        [TestMethod]
        public void TryParse_DMS_West_Assert()
        {
            DegreeMinuteSecond dms = null;
            string value = "041° 47' 46.32569'' W";

            bool result = DegreeMinuteSecond.TryParse(value, out dms);

            Assert.IsTrue(result);
            Assert.IsTrue(dms.Degrees.WithinTolerance(-41.7962015805556), dms.Degrees.ToString());
        }

        [TestMethod]
        public void TryParse_DoubleString_Assert()
        {
            DegreeMinuteSecond dms = null;
            string value = "41.79620158";

            bool result = DegreeMinuteSecond.TryParse(value, out dms);

            Assert.IsTrue(result);
            Assert.AreEqual(Convert.ToDouble(value), dms.Degrees);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryParse_Null_ThrowsException()
        {
            DegreeMinuteSecond dms = null;

            DegreeMinuteSecond.TryParse(null, out dms);
        }
    }
}
