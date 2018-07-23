using System;

namespace DevStreet.Geodesy.Calculator
{
    /// <summary>
    /// Base class for all Geodesy calculators.
    /// </summary>
    public class CalculatorBase : ICoordinateFactory
    {
        private Func<ICoordinate> _factory = () => { return new Coordinate(); };

        /// <summary>
        /// Base class for all Geodesy calculators.
        /// </summary>
        protected CalculatorBase()
        {
        }

        /// <summary>
        /// Validate the value as a bearing to ensure it's between 0 and 360.
        /// </summary>
        /// <param name="bearing">The value to validate.</param>
        protected static void ValidateBearing(double bearing)
        {
            ValidateBearing(bearing, nameof(bearing));
        }

        /// <summary>
        /// Validate the value as a bearing to ensure it's between 0 and 360.
        /// </summary>
        /// <param name="bearing">The value to validate.</param>
        /// <param name="argumentName">The name of the value.</param>
        protected static void ValidateBearing(double bearing, string argumentName)
        {
            if (bearing < 0 || bearing > 360)
            {
                throw new ArgumentOutOfRangeException(argumentName, "A bearing cannot be less than zero or greater than 360.");
            }
        }

        /// <summary>
        /// Validate the value as a radius to ensure it's between the lower and upper boundaries of the various radius values of the Earth.
        /// <para>Works for both metres and kilometres.</para>
        /// </summary>
        /// <param name="radius">The value to validate.</param>
        protected static void ValidateRadius(double radius)
        {
            double kimometresMinimum = 6350;
            double kimometresMaximum = 6400;
            double metresMinimum = kimometresMinimum * 1000;
            double metresMaximum = kimometresMaximum * 1000;

            if (((radius >= metresMinimum && radius <= metresMaximum) || (radius >= kimometresMinimum && radius <= kimometresMaximum)) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), string.Format("The radius must be within the expected range {0}km to {1}km (in metres or kilometres).", kimometresMinimum, kimometresMaximum));
            }
        }

        /// <summary>
        /// Create a new object that implements ICoordinate using the factory method previously supplied.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        protected ICoordinate NewCoordinate(double latitude, double longitude)
        {
            ICoordinate coordinate = ((ICoordinateFactory)this).Create();
            coordinate.Latitude = latitude;
            coordinate.Longitude = longitude;
            return coordinate;
        }

        /// <summary>
        /// Create a new object that implements ICoordinate using the factory method previously supplied.
        /// </summary>
        /// <returns></returns>
        public ICoordinate Create()
        {
            return _factory();
        }

        /// <summary>
        /// Override the default factory method for creating an object that implements ICoordinate.
        /// </summary>
        /// <param name="factory"></param>
        public bool SetFactoryMethod(Func<ICoordinate> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory), "The argument cannot be null.");
            }

            _factory = factory;
            return true;
        }
    }
}
