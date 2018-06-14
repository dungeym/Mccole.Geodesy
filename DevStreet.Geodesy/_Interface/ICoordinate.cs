namespace DevStreet.Geodesy
{
    /// <summary>
    /// A pair of numbers that show the exact position of a point on the earth.
    /// </summary>
    public interface ICoordinate
    {
        /// <summary>
        /// The angular distance of a place north or south of the earth's equator.
        /// <para>Usually expressed in degrees and minutes.</para>
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// The angular distance of a place east or west of the Greenwich meridian.
        /// <para>Usually expressed in degrees and minutes.</para>
        /// </summary>
        double Longitude { get; set; }
    }
}