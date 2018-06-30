using System;

namespace DevStreet.Geodesy
{
    /// <summary>
    /// Represents 3 dimensional a point on the earth's surface.
    /// </summary>
    public interface IWaypoint : ICoordinate
    {
        /// <summary>
        /// The height above sea level of this location.
        /// </summary>
        double Elevation { get; set; }
    }
}
