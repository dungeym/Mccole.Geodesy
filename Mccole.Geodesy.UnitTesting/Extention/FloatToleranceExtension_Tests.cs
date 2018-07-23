using Mccole.Geodesy.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mccole.Geodesy.UnitTesting.Extention
{
    [TestClass]
    public class FloatToleranceExtension_Tests
    {
        [TestMethod]
        public void WithinTolerance_Default_No_Assert()
        {
            double value = 1D;
            double obj = value + FloatToleranceExtension.DefaultTolerance;

            bool result = FloatToleranceExtension.WithinTolerance(value, obj);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WithinTolerance_Default_Yes_Assert()
        {
            double value = 1D;
            double obj = value + (FloatToleranceExtension.DefaultTolerance / 2);

            bool result = FloatToleranceExtension.WithinTolerance(value, obj);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WithinTolerance_No_Assert()
        {
            double tolerance = 0.001;
            double value = 1D;
            double obj = value + (tolerance * 2);

            bool result = FloatToleranceExtension.WithinTolerance(value, obj, tolerance);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WithinTolerance_Yes_Assert()
        {
            double tolerance = 0.001;
            double value = 1D;
            double obj = value + (tolerance / 2);

            bool result = FloatToleranceExtension.WithinTolerance(value, obj, tolerance);

            Assert.IsTrue(result);
        }
    }
}
