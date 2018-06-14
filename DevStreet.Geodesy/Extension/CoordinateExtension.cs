using DevStreet.Geodesy.Calculator;

namespace DevStreet.Geodesy.Extension
{
    /// <summary>
    /// Extension methods for ICoordinate.
    /// </summary>
    public static class CoordinateExtension
    {
        /// <summary>
        /// Calculate the (initial) bearing from this point to another.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static double BearingTo(this ICoordinate @this, ICoordinate point)
        {
            return GeodeticCalculator.Instance.Bearing(@this, point);
        }

        /// <summary>
        /// Calculate the distance between this point and another (using haversine formula) and the average radius of the earth.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="point">The destination point.</param>
        /// <returns></returns>
        public static double DistanceTo(this ICoordinate @this, ICoordinate point)
        {
            return DistanceTo(@this, point, Radius.Mean);
        }

        /// <summary>
        /// Calculate the distance between this point and another (using haversine formula).
        /// </summary>
        /// <param name="this"></param>
        /// <param name="point">The destination point.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <returns></returns>
        public static double DistanceTo(this ICoordinate @this, ICoordinate point, double radius)
        {
            return GeodeticCalculator.Instance.Distance(@this, point, radius);
        }
    }
}
