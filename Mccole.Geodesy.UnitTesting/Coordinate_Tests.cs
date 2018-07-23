using Mccole.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mccole.Geodesy.UnitTesting
{
    [TestClass]
    public class Coordinate_Tests
    {
        [TestMethod]
        public void CompareTo_GreaterThan_Assert()
        {
            Coordinate left = new Coordinate(30, 30);
            Coordinate right = new Coordinate(20, 20);

            var result = left.CompareTo(right);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CompareTo_InvalidType_Assert()
        {
            Coordinate left = new Coordinate(30, 30);

            var result = left.CompareTo(DateTime.Now);
        }

        [TestMethod]
        public void CompareTo_LessThan_Assert()
        {
            Coordinate left = new Coordinate(10, 10);
            Coordinate right = new Coordinate(20, 20);

            var result = left.CompareTo(right);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CompareTo_Null_Assert()
        {
            Coordinate left = new Coordinate(30, 30);

            var result = left.CompareTo(null);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CompareTo_Same_Assert()
        {
            Coordinate left = new Coordinate(10, 10);

            var result = left.CompareTo(left);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Constructor_Double_Assert()
        {
            double latitude = TestData.Double();
            double longitude = TestData.Double();

            Coordinate subject = new Coordinate(latitude, longitude);

            Assert.AreEqual(latitude, subject.Latitude);
            Assert.AreEqual(longitude, subject.Longitude);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Latitude_Empty_ThrowsException()
        {
            string latitude = string.Empty;
            string longitude = "004 08 02W";

            Coordinate subject = new Coordinate(latitude, longitude);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Latitude_Null_ThrowsException()
        {
            string latitude = null;
            string longitude = "004 08 02W";

            Coordinate subject = new Coordinate(latitude, longitude);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Longitude_Empty_ThrowsException()
        {
            string latitude = "50 21 59N";
            string longitude = string.Empty;

            Coordinate subject = new Coordinate(latitude, longitude);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Longitude_Null_ThrowsException()
        {
            string latitude = "50 21 59N";
            string longitude = null;

            Coordinate subject = new Coordinate(latitude, longitude);
        }

        [TestMethod]
        public void Constructor_String_Assert()
        {
            string latitude = "50 21 59N";
            string longitude = "004 08 02W";

            Coordinate subject = new Coordinate(latitude, longitude);

            Assert.AreEqual(50.36639, Math.Round(subject.Latitude, 5));
            Assert.AreEqual(-4.13389, Math.Round(subject.Longitude, 5));
        }

        [TestMethod]
        public void Equal_Equal_Assert()
        {
            Coordinate left = new Coordinate(30, 30);
            Coordinate right = new Coordinate(30, 30);

            var result = left == right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Equal_Not_Assert()
        {
            Coordinate left = new Coordinate(20, 20);
            Coordinate right = new Coordinate(30, 30);

            var result = left == right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Equal_Null_Assert()
        {
            Coordinate left = new Coordinate(20, 20);

            var result = left == null;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GreaterThan_Equal_Assert()
        {
            Coordinate left = new Coordinate(20, 20);
            Coordinate right = new Coordinate(20, 20);

            var result = left > right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GreaterThan_Less_Assert()
        {
            Coordinate left = new Coordinate(10, 10);
            Coordinate right = new Coordinate(20, 20);

            var result = left > right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GreaterThan_More_Assert()
        {
            Coordinate left = new Coordinate(30, 30);
            Coordinate right = new Coordinate(20, 20);

            var result = left > right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void GreaterThanEqualTo_Equal_Assert()
        {
            Coordinate left = new Coordinate(20, 20);
            Coordinate right = new Coordinate(20, 20);

            var result = left >= right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void GreaterThanEqualTo_Less_Assert()
        {
            Coordinate left = new Coordinate(10, 10);
            Coordinate right = new Coordinate(20, 20);

            var result = left >= right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GreaterThanEqualTo_More_Assert()
        {
            Coordinate left = new Coordinate(30, 30);
            Coordinate right = new Coordinate(20, 20);

            var result = left >= right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void LessThan_Equal_Assert()
        {
            Coordinate left = new Coordinate(20, 20);
            Coordinate right = new Coordinate(20, 20);

            var result = left < right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void LessThan_Less_Assert()
        {
            Coordinate left = new Coordinate(10, 10);
            Coordinate right = new Coordinate(20, 20);

            var result = left < right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void LessThan_More_Assert()
        {
            Coordinate left = new Coordinate(30, 30);
            Coordinate right = new Coordinate(20, 20);

            var result = left < right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void LessThanEqualTo_Equal_Assert()
        {
            Coordinate left = new Coordinate(20, 20);
            Coordinate right = new Coordinate(20, 20);

            var result = left <= right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void LessThanEqualTo_Less_Assert()
        {
            Coordinate left = new Coordinate(10, 10);
            Coordinate right = new Coordinate(20, 20);

            var result = left <= right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void LessThanEqualTo_More_Assert()
        {
            Coordinate left = new Coordinate(30, 30);
            Coordinate right = new Coordinate(20, 20);

            var result = left <= right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void NotEqual_Equal_Assert()
        {
            Coordinate left = new Coordinate(30, 30);
            Coordinate right = new Coordinate(30, 30);

            var result = left != right;

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void NotEqual_Not_Assert()
        {
            Coordinate left = new Coordinate(20, 20);
            Coordinate right = new Coordinate(30, 30);

            var result = left != right;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void NotEqual_Null_Assert()
        {
            Coordinate left = new Coordinate(20, 20);

            var result = left != null;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ToDegreeMinuteSecond_Assert()
        {
            Coordinate subject = new Coordinate(41.79620158, 145.5487268);

            string result = subject.ToDegreeMinuteSecond();

            Assert.AreEqual("41° 47' 46'' N, 145° 32' 55'' E", result);
        }
    }
}