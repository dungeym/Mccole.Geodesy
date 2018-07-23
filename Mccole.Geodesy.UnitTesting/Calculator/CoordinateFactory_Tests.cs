using Mccole.Geodesy.Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mccole.Geodesy.UnitTesting.Calculator
{
    [TestClass]
    public class CoordinateFactory_Tests
    {
        [TestMethod]
        public void Create_Valid_Assert()
        {
            Func<ICoordinate> func = new Func<ICoordinate>(() => { return new TestCoordinate(); });
            ((ICoordinateFactory)RhumbCalculator.Instance).SetFactoryMethod(func);

            var result = ((ICoordinateFactory)RhumbCalculator.Instance).Create();

            Assert.IsInstanceOfType(result, typeof(TestCoordinate));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetFactoryMethod_Func_Null_ThrowsException()
        {
            ((ICoordinateFactory)RhumbCalculator.Instance).SetFactoryMethod(null);
        }

        [TestMethod]
        public void SetFactoryMethod_Valid_Assert()
        {
            Func<ICoordinate> func = new Func<ICoordinate>(() => { return new TestCoordinate(); });

            var result = ((ICoordinateFactory)RhumbCalculator.Instance).SetFactoryMethod(func);

            Assert.IsTrue(result);
        }
    }

    public class TestCoordinate : ICoordinate
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}