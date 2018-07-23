using System;

namespace Mccole.Geodesy.Formatter
{
    /// <summary>
    /// Provides culture-specific information for formatting and parsing numeric values to a Milliradian (NATO) bearing.
    /// </summary>
    public class MilliradianFormatInfo : BearingFormatInfo
    {
        /// <summary>
        /// The default symbol for a Milliradian (NATO) bearing.
        /// </summary>
        public const string DefaultMilsSymbol = "mils";

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a Milliradian (NATO) bearing.
        /// </summary>
        public MilliradianFormatInfo()
            : this(DegreeMinuteSecondFormatInfo.DefaultScale)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a Milliradian (NATO) bearing.
        /// </summary>
        /// <param name="scale">Number of decimal places to use.</param>
        public MilliradianFormatInfo(int scale)
            : this(scale, DegreeMinuteSecondFormatInfo.DefaultSeparator)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a Milliradian (NATO) bearing.
        /// </summary>
        /// <param name="scale">Number of decimal places to use.</param>
        /// <param name="separator">The character (usually a space) used to separate the degree, minute and second values.</param>
        public MilliradianFormatInfo(int scale, string separator)
            : base(scale, separator)
        {
            this.MilsSymbol = DefaultMilsSymbol;
        }

        /// <summary>
        /// The symbol used to represent a Milliradian (NATO) value.
        /// </summary>
        public string MilsSymbol { get; set; }

        /// <summary>
        /// Format this object to a Milliradian (NATO) string.
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

            UpdateScaleFromFormatString(format);
            var mils = dms.Bearing * (6400 / 360);
            var m = Math.Round(mils, base.Scale);
            return string.Format("{0}{1}{2}", m, base.Separator, this.MilsSymbol);
        }
    }
}
