using System;

namespace Mccole.Geodesy.Formatter
{
    /// <summary>
    /// Provides culture-specific information for formatting and parsing numeric values to a compass point.
    /// </summary>
    public class CompassPointFormatInfo : DegreeMinuteSecondFormatInfo
    {
        // The sequence is important.
        private static string[] Cardinals = new[] {
             Direction.North,
             Direction.NorthNorthEast,
             Direction.NorthEast,
             Direction.EastNorthEast,
             Direction.East,
             Direction.EastSouthEast,
             Direction.SouthEast,
             Direction.SouthSouthEast,
             Direction.South ,
             Direction.SouthSouthWest,
             Direction.SouthWest,
             Direction.WestSouthWest,
             Direction.West,
             Direction.WestNorthWest,
             Direction.NorthWest,
             Direction.NorthNorthWest
        };

        private readonly int _precision;

        /// <summary>
        /// The default CompassPointPrecision setting.
        /// </summary>
        public const CompassPointPrecision DefaultCompassPointPrecision = CompassPointPrecision.SecondaryIntercardinal;

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a compass point.
        /// </summary>
        public CompassPointFormatInfo()
            : this(CompassPointFormatInfo.DefaultCompassPointPrecision)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a compass point.
        /// </summary>
        /// <param name="precision">The CompassPointPrecision to use.</param>
        public CompassPointFormatInfo(CompassPointPrecision precision)
        {
            _precision = (int)precision;
        }

        /// <summary>
        /// Format this object to a Compass Point string.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        protected override string DoFormat(string format, object arg, IFormatProvider formatProvider)
        {
            DegreeMinuteSecond dms = null;
            if (!DegreeMinuteSecondFormatInfo.TryParse(arg, out dms))
            {
                // Provide default formatting if the argument is not as expected (Double or DegreeMinuteSecond).
                return FormatUnexpectedDataType(format, arg);
            }

            // No of compass points at required precision (1=>4, 2=>8, 3=>16).
            var n = 4 * Math.Pow(2, _precision - 1);
            double cardinalIndex = Math.Round(dms.Bearing * n / 360) % n * 16 / n;
            var cardinal = Cardinals[(int)cardinalIndex];
            return cardinal;
        }
    }
}
