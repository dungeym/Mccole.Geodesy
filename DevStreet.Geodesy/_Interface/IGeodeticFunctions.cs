namespace DevStreet.Geodesy
{
    /// <summary>
    /// Provides a series of Geodetic functions that can be used measure the earth's surface.
    /// </summary>
    public interface IGeodeticFunctions : ICoordinateFactory
    {
        /// <summary>
        /// Calculate the (initial) bearing from point A to point B.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Initial bearing in degrees from north.</returns>
        double Bearing(ICoordinate pointA, ICoordinate pointB);

        /// <summary>
        /// Calculate the destination point from point A having travelled the given distance on the given initial bearing.
        /// <para>Bearing normally varies around path followed.</para>
        /// <para>Uses the mean radius of the Earth in metres.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="distance">Distance travelled, in same units as the earth's radius (metres).</param>
        /// <param name="bearing">Initial bearing in degrees from north.</param>
        /// <returns>The destination point.</returns>
        ICoordinate Destination(ICoordinate pointA, double distance, double bearing);

        /// <summary>
        /// Calculate the destination point from point A having travelled the given distance on the given initial bearing.
        /// <para>Bearing normally varies around path followed.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="distance">Distance travelled, in same units as the earth's radius.</param>
        /// <param name="bearing">Initial bearing in degrees from north.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>The destination point.</returns>
        ICoordinate Destination(ICoordinate pointA, double distance, double bearing, double radius);

        /// <summary>
        /// Calculate the distance between 2 points (using haversine formula).
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Distance between this point and destination point, in metres.</returns>
        double Distance(ICoordinate pointA, ICoordinate pointB);

        /// <summary>
        /// Calculate the distance between 2 points (using haversine formula).
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>Distance between this point and destination point, in same units as radius.</returns>
        double Distance(ICoordinate pointA, ICoordinate pointB, double radius);

        /// <summary>
        /// Calculate the midpoint between point A and point B.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Midpoint between this point and the supplied point.</returns>
        ICoordinate Midpoint(ICoordinate pointA, ICoordinate pointB);
    }
}
