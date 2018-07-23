using System;

namespace Mccole.Geodesy.Formatter
{
    /// <summary>
    /// Provides culture-specific information for formatting and parsing numeric values to a bearing.
    /// </summary>
    public class BearingFormatInfo : DegreeMinuteSecondFormatInfo
    {
        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a bearing.
        /// </summary>
        public BearingFormatInfo()
            : this(DegreeMinuteSecondFormatInfo.DefaultScale)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a bearing.
        /// </summary>
        /// <param name="scale">Number of decimal places to use.</param>
        public BearingFormatInfo(int scale)
            : this(scale, DegreeMinuteSecondFormatInfo.DefaultSeparator)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a bearing.
        /// </summary>
        /// <param name="scale">Number of decimal places to use.</param>
        /// <param name="separator">The character (usually a space) used to separate the degree, minute and second values.</param>
        public BearingFormatInfo(int scale, string separator)
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

            var bearing = base.DoFormat(format, dms.Bearing, formatProvider);

            // Just in case rounding took us up to 360°!
            return bearing == null ? "–" : bearing.Replace("360", "0");
        }
    }
}
