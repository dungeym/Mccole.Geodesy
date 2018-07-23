using System;

namespace Mccole.Geodesy.Formatter
{
    /// <summary>
    /// Provides culture-specific information for formatting and parsing numeric values to longitude.
    /// </summary>
    public class LongitudeFormatInfo : DegreeMinuteSecondFormatInfo
    {
        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to longitude.
        /// </summary>
        public LongitudeFormatInfo()
            : this(DegreeMinuteSecondFormatInfo.DefaultScale)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to longitude.
        /// </summary>
        /// <param name="scale">Number of decimal places to use.</param>
        public LongitudeFormatInfo(int scale)
            : this(scale, DegreeMinuteSecondFormatInfo.DefaultSeparator)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to longitude.
        /// </summary>
        /// <param name="scale">Number of decimal places to use.</param>
        /// <param name="separator">The character (usually a space) used to separate the degree, minute and second values.</param>
        public LongitudeFormatInfo(int scale, string separator)
            : base(scale, separator)
        {
        }

        /// <summary>
        /// Format this object to a Degrees-Minutes-Seconds string.
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

            DegreeMinuteSecond degreeMinuteSecond = dms.Degrees < 0 ? new DegreeMinuteSecond(Math.Abs(dms.Degrees)) : dms;
            string longitude = base.DoFormat(format, degreeMinuteSecond, formatProvider);
            string cardinal = dms.Degrees < 0 ? Direction.West : Direction.East;
            return longitude == null ? "–" : longitude + base.Separator + cardinal;
        }
    }
}
