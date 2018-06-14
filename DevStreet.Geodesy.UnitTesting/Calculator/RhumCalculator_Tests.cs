using DevStreet.Geodesy.Calculator;
using DevStreet.Geodesy.Extension;
using DevStreet.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevStreet.Geodesy.UnitTesting.Calculator
{
    [TestClass]
    public class RhumbCalculator_Tests
    {
        private static double ConvertToBearing(string value)
        {
            DegreeMinuteSecond dms;
            if (DegreeMinuteSecond.TryParse(value, out dms))
            {
                return dms.Bearing;
            }
            else
            {
                throw new InvalidCastException(string.Format("Could not convert '{0}' to a DegreeMinuteSecond.", value));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Bearing_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            RhumbCalculator.Instance.Bearing(pointA, pointB);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Bearing_PointB_Null_ThrowsException()
        {
            var pointA = new Coordinate("50 03 59N", "005 42 53W");
            ICoordinate pointB = default(ICoordinate);

            RhumbCalculator.Instance.Bearing(pointA, pointB);
        }

        [TestMethod]
        public void Bearing_Valid_Assert()
        {
            var pointA = new Coordinate("50 21 59N", "004 08 02W");
            var pointB = new Coordinate("42 21 04N", "071 02 27W");

            var result = RhumbCalculator.Instance.Bearing(pointA, pointB);

            System.Diagnostics.Debug.WriteLine(string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DMS}", result));
            Assert.AreEqual(260.1272, System.Math.Round(result, 4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Above_Range_Kilometres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 6000;

            RhumbCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Above_Range_Metres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 6000 * 1000;

            RhumbCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Bearing_GreaterThan_360_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = 361;

            RhumbCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Bearing_Negative_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = -1;

            RhumbCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Below_Range_Kilometres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 4000;

            RhumbCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Below_Range_Metres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 4000 * 1000;

            RhumbCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Distance_Negative_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = -1;
            double bearing = ConvertToBearing("096°01′18″");

            RhumbCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Destination_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");

            RhumbCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Radius_Negative_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = -1;

            RhumbCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        public void Destination_Valid_Assert()
        {
            var pointA = new Coordinate("51 07 32N", "001 20 17E");
            double distance = 40.23 * 1000;
            double bearing = ConvertToBearing("116°38′10");

            var result = RhumbCalculator.Instance.Destination(pointA, distance, bearing);

            System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(50.96335, Math.Round(result.Latitude, 5));
            Assert.AreEqual(1.85244, Math.Round(result.Longitude, 5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Distance_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            RhumbCalculator.Instance.Distance(pointA, pointB);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Distance_PointB_Null_ThrowsException()
        {
            var pointA = new Coordinate("58 38 38N", "003 04 12W");
            ICoordinate pointB = default(ICoordinate);

            RhumbCalculator.Instance.Distance(pointA, pointB);
        }

        [TestMethod]
        public void Distance_Valid_Assert()
        {
            var pointA = new Coordinate("50 21 59N", "004 08 02W");
            var pointB = new Coordinate("42 21 04N", "071 02 27W");

            var result = RhumbCalculator.Instance.Distance(pointA, pointB);

            System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(5198001.8698, System.Math.Round(result, 4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Midpoint_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            RhumbCalculator.Instance.Midpoint(pointA, pointB);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Midpoint_PointB_Null_ThrowsException()
        {
            var pointA = new Coordinate("58 38 38N", "003 04 12W");
            ICoordinate pointB = default(ICoordinate);

            RhumbCalculator.Instance.Midpoint(pointA, pointB);
        }

        [TestMethod]
        public void Midpoint_Valid_Assert()
        {
            var pointA = new Coordinate("50 21 59N", "004 08 02W");
            var pointB = new Coordinate("42 21 04N", "071 02 27W");

            var result = RhumbCalculator.Instance.Midpoint(pointA, pointB);

            System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(46.35875, Math.Round(result.Latitude, 5));
            Assert.AreEqual(-37.58736, Math.Round(result.Longitude, 5));
        }
    }
}