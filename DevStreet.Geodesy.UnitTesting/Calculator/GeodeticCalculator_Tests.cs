using DevStreet.Geodesy.Calculator;
using DevStreet.Geodesy.Extension;
using DevStreet.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DevStreet.Geodesy.UnitTesting.Calculator
{
    [TestClass]
    public class GeodeticCalculator_Tests
    {
        private ICoordinate _pointA = new Coordinate(0, 0);

        private ICoordinate _pointB_NE = new Coordinate(0.005, 0.005);
        private ICoordinate _pointB_NW = new Coordinate(0.005, -0.005);
        private ICoordinate _pointB_SE = new Coordinate(-0.005, 0.005);
        private ICoordinate _pointB_SW = new Coordinate(-0.005, -0.005);

        private ICoordinate _pointC_ENE = new Coordinate(0.001, 0.009);
        private ICoordinate _pointC_ESE = new Coordinate(-0.001, 0.009);
        private ICoordinate _pointC_NNE = new Coordinate(0.009, 0.001);
        private ICoordinate _pointC_NNW = new Coordinate(0.009, -0.001);
        private ICoordinate _pointC_SSE = new Coordinate(-0.009, 0.001);
        private ICoordinate _pointC_SSW = new Coordinate(-0.009, -0.001);
        private ICoordinate _pointC_WNW = new Coordinate(0.001, -0.009);
        private ICoordinate _pointC_WSW = new Coordinate(-0.001, -0.009);

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

        private static double DistanceToPlaneWithBearingAssertion(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC)
        {
            ICoordinate pointX;
            var d = GeodeticCalculator.Instance.DistanceToPlane(pointA, pointB, pointC, out pointX);

            var b1 = GeodeticCalculator.Instance.Bearing(pointA, pointB);
            var b2 = GeodeticCalculator.Instance.Bearing(pointA, pointX);

            if (b1.WithinTolerance(b2) == false && (b2 - 180).WithinTolerance(b1) == false && (b1 - 180).WithinTolerance(b2) == false)
            {
                Assert.Fail("PointX is not on the same plane as A->B: {0}, {1}, {2}, {3}.", b1, b2, (b2 - 180), (b1 - 180));
            }

            return d;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Bearing_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            GeodeticCalculator.Instance.Bearing(pointA, pointB);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Bearing_PointB_Null_ThrowsException()
        {
            var pointA = new Coordinate("50 03 59N", "005 42 53W");
            ICoordinate pointB = default(ICoordinate);

            GeodeticCalculator.Instance.Bearing(pointA, pointB);
        }

        [TestMethod]
        public void Bearing_Valid_Assert()
        {
            var pointA = new Coordinate("50 03 59N", "005 42 53W");
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            var result = GeodeticCalculator.Instance.Bearing(pointA, pointB);

            System.Diagnostics.Debug.WriteLine(string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DMS}", result));
            Assert.AreEqual(9.1198, System.Math.Round(result, 4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CrossTrackDistance_EndPoint_Null_ThrowsException()
        {
            ICoordinate pointA = new Coordinate(0.0005, 0.0005);
            ICoordinate startPoint = new Coordinate(0, 0);
            ICoordinate endPoint = default(ICoordinate);

            double result = GeodeticCalculator.Instance.CrossTrackDistance(pointA, startPoint, endPoint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CrossTrackDistance_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            ICoordinate startPoint = new Coordinate(0, 0);
            ICoordinate endPoint = new Coordinate(0, 0.001);

            double result = GeodeticCalculator.Instance.CrossTrackDistance(pointA, startPoint, endPoint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CrossTrackDistance_Randius_Negative_ThrowsException()
        {
            ICoordinate pointA = new Coordinate(0.0005, 0.0005);
            ICoordinate startPoint = new Coordinate(0, 0);
            ICoordinate endPoint = new Coordinate(0, 0.001);
            double radius = -1;

            double result = GeodeticCalculator.Instance.CrossTrackDistance(pointA, startPoint, endPoint, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CrossTrackDistance_StartPoint_Null_ThrowsException()
        {
            ICoordinate pointA = new Coordinate(0.0005, 0.0005);
            ICoordinate startPoint = default(ICoordinate);
            ICoordinate endPoint = new Coordinate(0, 0.001);

            double result = GeodeticCalculator.Instance.CrossTrackDistance(pointA, startPoint, endPoint);
        }

        [TestMethod]
        public void CrossTrackDistance_Valid_Assert()
        {
            /*
             * Start and End is on the same latitude, 110m apart.
             * Point A is 55m above the start-end line and 90% of the way along it.
             */

            ICoordinate pointA = new Coordinate(0.0005, 0.0009);
            ICoordinate startPoint = new Coordinate(0, 0);
            ICoordinate endPoint = new Coordinate(0, 0.001);

            double result = GeodeticCalculator.Instance.CrossTrackDistance(pointA, startPoint, endPoint);

            Assert.IsTrue(result.WithinTolerance(-55.59746), result.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Above_Range_Kilometres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 6000;

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Above_Range_Metres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 6000 * 1000;

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Bearing_GreaterThan_360_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = 361;

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Bearing_Negative_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = -1;

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Below_Range_Kilometres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 4000;

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Below_Range_Metres_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = 4000 * 1000;

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Distance_Negative_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = -1;
            double bearing = ConvertToBearing("096°01′18″");

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Destination_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Destination_Radius_Negative_ThrowsException()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");
            double radius = -1;

            GeodeticCalculator.Instance.Destination(pointA, distance, bearing, radius);
        }

        [TestMethod]
        public void Destination_Valid_Assert()
        {
            var pointA = new Coordinate("53 19 14N", "001 43 47W");
            double distance = 124.8 * 1000;
            double bearing = ConvertToBearing("096°01′18″");

            var result = GeodeticCalculator.Instance.Destination(pointA, distance, bearing);

            System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(53.18827, Math.Round(result.Latitude, 5));
            Assert.AreEqual(0.13328, Math.Round(result.Longitude, 5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Distance_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            GeodeticCalculator.Instance.Distance(pointA, pointB);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Distance_PointB_Null_ThrowsException()
        {
            var pointA = new Coordinate("58 38 38N", "003 04 12W");
            ICoordinate pointB = default(ICoordinate);

            GeodeticCalculator.Instance.Distance(pointA, pointB);
        }

        [TestMethod]
        public void Distance_Valid_Assert()
        {
            var pointA = new Coordinate("50 03 59N", "005 42 53W");
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            var result = GeodeticCalculator.Instance.Distance(pointA, pointB);

            System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(968853.5467, System.Math.Round(result, 4));
        }

        [TestMethod]
        public void DistanceToLine_NE_ENE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_ENE);

            Assert.IsTrue(result.WithinTolerance(629.013492812523), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NE_ESE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_ESE);

            Assert.IsTrue(result.WithinTolerance(786.266866809431), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NE_NNE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_NNE);

            Assert.IsTrue(result.WithinTolerance(629.013492812523), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NE_NNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_NNW);

            Assert.IsTrue(result.WithinTolerance(786.266864955727), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NE_SSE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_SSE);

            Assert.IsTrue(result.WithinTolerance(885.990732194681), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NE_SSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_SSW);

            Assert.IsTrue(result.WithinTolerance(845.922523887795), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NE_WNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_WNW);

            Assert.IsTrue(result.WithinTolerance(885.990732194681), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NE_WSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NE, _pointC_WSW);

            Assert.IsTrue(result.WithinTolerance(845.922524290679), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_ENE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_ENE);

            Assert.IsTrue(result.WithinTolerance(885.990733212337), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_ESE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_ESE);

            Assert.IsTrue(result.WithinTolerance(845.922524290679), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_NNE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_NNE);

            Assert.IsTrue(result.WithinTolerance(786.266864955728), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_NNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_NNW);

            Assert.IsTrue(result.WithinTolerance(629.01349197916), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_SSE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_SSE);

            Assert.IsTrue(result.WithinTolerance(845.922524290679), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_SSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_SSW);

            Assert.IsTrue(result.WithinTolerance(885.990733212337), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_WNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_WNW);

            Assert.IsTrue(result.WithinTolerance(629.013491737309), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_NW_WSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_NW, _pointC_WSW);

            Assert.IsTrue(result.WithinTolerance(786.266865408391), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_ENE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_ENE);

            Assert.IsTrue(result.WithinTolerance(786.266865408391), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_ESE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_ESE);

            Assert.IsTrue(result.WithinTolerance(629.013491737309), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_NNE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_NNE);

            Assert.IsTrue(result.WithinTolerance(885.990732194681), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_NNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_NNW);

            Assert.IsTrue(result.WithinTolerance(845.922523887795), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_SSE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_SSE);

            Assert.IsTrue(result.WithinTolerance(629.013491737309), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_SSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_SSW);

            Assert.IsTrue(result.WithinTolerance(786.266865408391), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_WNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_WNW);

            Assert.IsTrue(result.WithinTolerance(845.922523887795), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SE_WSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SE, _pointC_WSW);

            Assert.IsTrue(result.WithinTolerance(885.9907321946819), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_ENE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_ENE);

            Assert.IsTrue(result.WithinTolerance(845.922524290679), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_ESE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_ESE);

            Assert.IsTrue(result.WithinTolerance(885.990733212337), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_NNE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_NNE);

            Assert.IsTrue(result.WithinTolerance(845.922524290679), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_NNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_NNW);

            Assert.IsTrue(result.WithinTolerance(885.990733212337), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_SSE_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_SSE);

            Assert.IsTrue(result.WithinTolerance(786.266865408391), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_SSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_SSW);

            Assert.IsTrue(result.WithinTolerance(629.013491737309), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_WNW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_WNW);

            Assert.IsTrue(result.WithinTolerance(786.266865408391), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_SW_WSW_Assert()
        {
            var result = DistanceToPlaneWithBearingAssertion(_pointA, _pointB_SW, _pointC_WSW);

            Assert.IsTrue(result.WithinTolerance(629.013491737309), "{0}", result);
        }

        [TestMethod]
        public void DistanceToLine_VeryLongDistances_Assert()
        {
            var pointA = new Coordinate(0, 0);
            var pointB = new Coordinate(52.722831, 34.057617);
            var pointC = new Coordinate(-30.943449, -61.565307);

            var result = DistanceToPlaneWithBearingAssertion(pointA, pointB, pointC);

            Assert.IsTrue(result.WithinTolerance(6769185.42702526), "{0}", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FinalBearing_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            GeodeticCalculator.Instance.FinalBearing(pointA, pointB);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FinalBearing_PointB_Null_ThrowsException()
        {
            var pointA = new Coordinate("58 38 38N", "003 04 12W");
            ICoordinate pointB = default(ICoordinate);

            GeodeticCalculator.Instance.FinalBearing(pointA, pointB);
        }

        [TestMethod]
        public void FinalBearing_Valid_Assert()
        {
            var pointA = new Coordinate("50 03 59N", "005 42 53W");
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            var result = GeodeticCalculator.Instance.FinalBearing(pointA, pointB);

            System.Diagnostics.Debug.WriteLine(string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DMS}", result));
            Assert.AreEqual(11.2752, System.Math.Round(result, 4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Intersection_Bearing1_GreaterThan_360_ThrowsException()
        {
            var point1 = new Coordinate(51.8853, 0.2545);
            var bearing1 = 361;
            var point2 = new Coordinate(49.0034, 2.5735);
            var bearing2 = ConvertToBearing("32.44°");

            GeodeticCalculator.Instance.Intersection(point1, bearing1, point2, bearing2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Intersection_Bearing1_Negative_ThrowsException()
        {
            var point1 = new Coordinate(51.8853, 0.2545);
            var bearing1 = -1;
            var point2 = new Coordinate(49.0034, 2.5735);
            var bearing2 = ConvertToBearing("32.44°");

            GeodeticCalculator.Instance.Intersection(point1, bearing1, point2, bearing2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Intersection_Bearing2_GreaterThan_360_ThrowsException()
        {
            var point1 = new Coordinate(51.8853, 0.2545);
            var bearing1 = ConvertToBearing("108.55°");
            var point2 = new Coordinate(49.0034, 2.5735);
            var bearing2 = 361;

            GeodeticCalculator.Instance.Intersection(point1, bearing1, point2, bearing2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Intersection_Bearing2_Negative_ThrowsException()
        {
            var point1 = new Coordinate(51.8853, 0.2545);
            var bearing1 = ConvertToBearing("108.55°");
            var point2 = new Coordinate(49.0034, 2.5735);
            var bearing2 = -1;

            GeodeticCalculator.Instance.Intersection(point1, bearing1, point2, bearing2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Intersection_Point1_Null_ThrowsException()
        {
            ICoordinate point1 = default(ICoordinate);
            var bearing1 = ConvertToBearing("108.55°");
            var point2 = new Coordinate(49.0034, 2.5735);
            var bearing2 = ConvertToBearing("32.44°");

            GeodeticCalculator.Instance.Intersection(point1, bearing1, point2, bearing2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Intersection_Point2_Null_ThrowsException()
        {
            var point1 = new Coordinate(51.8853, 0.2545);
            var bearing1 = ConvertToBearing("108.55°");
            ICoordinate point2 = default(ICoordinate);
            var bearing2 = ConvertToBearing("32.44°");

            GeodeticCalculator.Instance.Intersection(point1, bearing1, point2, bearing2);
        }

        [TestMethod]
        public void Intersection_Valid_Assert()
        {
            var point1 = new Coordinate(51.8853, 0.2545);
            var bearing1 = ConvertToBearing("108.55°");
            var point2 = new Coordinate(49.0034, 2.5735);
            var bearing2 = ConvertToBearing("32.44°");

            var result = GeodeticCalculator.Instance.Intersection(point1, bearing1, point2, bearing2);

            System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(50.90761, Math.Round(result.Latitude, 5));
            Assert.AreEqual(4.50857, Math.Round(result.Longitude, 5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Midpoint_PointA_Null_ThrowsException()
        {
            ICoordinate pointA = default(ICoordinate);
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            GeodeticCalculator.Instance.Midpoint(pointA, pointB);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Midpoint_PointB_Null_ThrowsException()
        {
            var pointA = new Coordinate("58 38 38N", "003 04 12W");
            ICoordinate pointB = default(ICoordinate);

            GeodeticCalculator.Instance.Midpoint(pointA, pointB);
        }

        [TestMethod]
        public void Midpoint_Valid_Assert()
        {
            var pointA = new Coordinate("50 03 59N", "005 42 53W");
            var pointB = new Coordinate("58 38 38N", "003 04 12W");

            var result = GeodeticCalculator.Instance.Midpoint(pointA, pointB);

            System.Diagnostics.Debug.WriteLine(result);
            Assert.AreEqual(54.36229, Math.Round(result.Latitude, 5));
            Assert.AreEqual(-4.53067, Math.Round(result.Longitude, 5));
        }
    }
}