using System;

namespace DevStreet.Geodesy
{
    /// <summary>
    /// Represents a point-in-time that a specific coordinate was travelled through.
    /// </summary>
    public interface ITrackpoint : ICoordinate
    {
        /// <summary>
        /// The date and time the coordinate was travelled through.
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
