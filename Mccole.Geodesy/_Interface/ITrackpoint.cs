using System;

namespace DevStreet.Geodesy
{
    /// <summary>
    /// Represents the point-in-time that a specific Waypoint was travelled through.
    /// </summary>
    public interface ITrackpoint : IWaypoint
    {
        /// <summary>
        /// The date and time the coordinate was travelled through.
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
