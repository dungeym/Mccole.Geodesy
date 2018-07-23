using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mccole.Geodesy
{
    /// <summary>
    /// Common radius values for the Earth, expressed in metres.
    /// </summary>
    public static class Radius
    {
        /// <summary>
        /// The radius of Earth at the equator.
        /// </summary>
        public const double Equatorial = 6378 * 1000;

        /// <summary>
        /// The radius of Earth at the equator using the same longitude.
        /// </summary>
        public const double EquatorialMeridian = 6399 * 1000;

        /// <summary>
        /// The radius of the Earth as used by Google Maps.
        /// </summary>
        public const double GoogleMaps = 6378137;

        /// <summary>
        /// The mean value of the radius of the Earth.
        /// </summary>
        public const double Mean = 6371 * 1000;

        /// <summary>
        /// The radius of Earth from pole to pole.
        /// </summary>
        public const double Polar = 6357 * 1000;

        /// <summary>
        /// The radius of Earth at the equator using the same longitude.
        /// </summary>
        public const double PolarMeridian = 6371 * 1000;
    }
}
