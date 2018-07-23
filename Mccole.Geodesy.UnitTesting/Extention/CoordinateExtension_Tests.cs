using Mccole.Geodesy.Extension;
using Mccole.Geodesy.Formatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mccole.Geodesy.UnitTesting.Extention
{
    [TestClass]
    public class CoordinateExtension_Tests
    {
        [TestMethod]
        public void BearingTo_Valid_Assert()
        {
            var pointA = new Coordinate("50 21 59N", "004 08 02W");
            var pointB = new Coordinate("42 21 04N", "071 02 27W");

            double result = pointA.BearingTo(pointB);

            System.Diagnostics.Debug.WriteLine(string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DMS}", result));
            Assert.IsTrue(result.WithinTolerance(286.895294733006), result.ToString());
        }

        [TestMethod]
        public void DistanceTo_Radius_Valid_Assert()
        {
            var pointA = new Coordinate("50 21 59N", "004 08 02W");
            var pointB = new Coordinate("42 21 04N", "071 02 27W");

            double result = pointA.DistanceTo(pointB, Radius.GoogleMaps);

            Assert.IsTrue(result.WithinTolerance(5039851.57028237), result.ToString());
        }

        [TestMethod]
        public void DistanceTo_Valid_Assert()
        {
            var pointA = new Coordinate("50 21 59N", "004 08 02W");
            var pointB = new Coordinate("42 21 04N", "071 02 27W");

            double result = pointA.DistanceTo(pointB);

            Assert.IsTrue(result.WithinTolerance(5034212.08328842), result.ToString());
        }
    }
}
