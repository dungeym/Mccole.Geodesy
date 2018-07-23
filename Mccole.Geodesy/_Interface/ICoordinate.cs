namespace Mccole.Geodesy
{
    /// <summary>
    /// Represents 2 dimensional a point on the earth's surface.
    /// </summary>
    public interface ICoordinate
    {
        /// <summary>
        /// The angular distance of a place north or south of the earth's equator.
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// The angular distance of a place east or west of the Greenwich meridian.
        /// </summary>
        double Longitude { get; set; }
    }
}