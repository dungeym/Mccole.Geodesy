using DevStreet.Geodesy.Calculator;
using System;

namespace DevStreet.Geodesy.Formatter
{
    /// <summary>
    /// Provides culture-specific information for formatting and parsing numeric values to a Degree-Minute-Second format.
    /// </summary>
    public class DegreeMinuteSecondFormatInfo : IFormatProvider, ICustomFormatter
    {
        /*
         * Scale        Value       Distance (m)
         * -------      ---------   -----------
         * 1            0.1000000   11,057.43       11 km
         * 2            0.0100000   1,105.74        1.1 km
         * 3            0.0010000   110.57          110 m
         * 4            0.0001000   11.06           10 m
         * 5            0.0000100   1.11            1.1 m
         * 6            0.0000010   0.11            11 cm
         * 7            0.0000001   0.01            1 cm
         */

        /// <summary>
        /// Format string for formatting a floating point value to have 2 whole numbers.
        /// </summary>
        protected const string DegreeFormatString2 = "{0:00.#########}";

        /// <summary>
        /// Format string for formatting a floating point value to have 3 whole numbers.
        /// </summary>
        protected const string DegreeFormatString3 = "{0:000.#########}";

        /// <summary>
        /// The default value used to separate the Degrees-Minutes-Seconds value, a single space.
        /// </summary>
        protected readonly static string DefaultSeparator = new string((char)32, 1);

        /// <summary>
        /// The default symbol used for the Degrees part of a Degrees-Minutes-Seconds value.
        /// </summary>
        public const string DefaultDegreeSymbol = "°";

        /// <summary>
        /// The default symbol used for the Minutes part of a Degrees-Minutes-Seconds value.
        /// </summary>
        public const string DefaultMinuteSymbol = "'";

        /// <summary>
        /// The default scale used for the last numeric component of a Degrees-Minutes-Seconds value.
        /// </summary>
        public const int DefaultScale = 0;

        /// <summary>
        /// The default symbol used for the Seconds part of a Degrees-Minutes-Seconds value.
        /// </summary>
        public const string DefaultSecondSymbol = "''";

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a Degree-Minute-Second format.
        /// </summary>
        public DegreeMinuteSecondFormatInfo()
                : this(DefaultScale)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a Degree-Minute-Second format.
        /// </summary>
        /// <param name="scale">Number of decimal places to use of the last part of.</param>
        public DegreeMinuteSecondFormatInfo(int scale)
            : this(scale, DefaultSeparator)
        {
        }

        /// <summary>
        /// Provides culture-specific information for formatting and parsing numeric values to a Degree-Minute-Second format.
        /// </summary>
        /// <param name="scale">Number of decimal places used for the last value displayed.</param>
        /// <param name="separator">The character (usually a space) used to separate the degree, minute and second values.</param>
        public DegreeMinuteSecondFormatInfo(int scale, string separator)
        {
            if (scale < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scale), "The argument must be greater than or equal to zero.");
            }

            this.Scale = scale;
            this.Separator = separator;
            this.DegreeSymbol = DefaultDegreeSymbol;
            this.MinuteSymbol = DefaultMinuteSymbol;
            this.SecondSymbol = DefaultSecondSymbol;
        }

        /// <summary>
        /// The format string used to format the degrees value.
        /// </summary>
        protected virtual string DegreeFormatString

        {
            get
            {
                return DegreeFormatString3;
            }
        }

        /// <summary>
        /// The symbol used to signify a degrees value.
        /// </summary>
        public string DegreeSymbol { get; set; }

        /// <summary>
        /// The symbol used to signify a minutes value.
        /// </summary>
        public string MinuteSymbol { get; set; }

        /// <summary>
        /// The number of decimal places to display for the last part of a Degrees-Minutes-Seconds value.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// The symbol used to signify a seconds value.
        /// </summary>
        public string SecondSymbol { get; set; }

        /// <summary>
        /// The space character(s) to insert between the Degrees, Minutes and Seconds values after each symbol.
        /// </summary>
        public string Separator { get; set; }

        private string ToDegree(DegreeMinuteSecond dms)
        {
            string d = string.Format(this.DegreeFormatString, Math.Round(dms.Degrees, this.Scale));
            return string.Format("{0}{1}", d, this.DegreeSymbol);
        }

        private string ToDegreeMinute(DegreeMinuteSecond dms)
        {
            string d = string.Format(this.DegreeFormatString, dms.Degree);
            string m = string.Format("{0:00.#########}", Math.Round(dms.Minutes, this.Scale));
            return string.Format("{1}{2}{0}{3}{4}", this.Separator, d, this.DegreeSymbol, m, this.MinuteSymbol);
        }

        private string ToDegreeMinuteSecond(DegreeMinuteSecond dms)
        {
            string d = string.Format(this.DegreeFormatString, dms.Degree);
            string m = string.Format("{0:00.#########}", dms.Minute);
            string s = string.Format("{0:00.#########}", Math.Round(dms.Seconds, this.Scale));
            return string.Format("{1}{2}{0}{3}{4}{0}{5}{6}", this.Separator, d, this.DegreeSymbol, m, this.MinuteSymbol, s, this.SecondSymbol);
        }

        /// <summary>
        /// Format the argument when the data type is not one of the expected types.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected static string FormatUnexpectedDataType(string format, object arg)
        {
            try
            {
                IFormattable formattable = arg as IFormattable;
                if (formattable != null)
                {
                    return formattable.ToString(format, System.Globalization.CultureInfo.CurrentCulture);
                }
                else if (arg != null)
                {
                    return arg.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (FormatException ex)
            {
                throw new FormatException(string.Format("The format of '{0}' is invalid, use 'D', 'DM', or 'DMS' optionally suffixed with a scale value.", format), ex);
            }
        }

        /// <summary>
        /// Attempt to parse the argument to a DegreeMinuteSecond object.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected static bool TryParse(object arg, out DegreeMinuteSecond result)
        {
            DegreeMinuteSecond dms = arg as DegreeMinuteSecond;
            if (dms == null && arg is double == false)
            {
                result = null;
                return false;
            }
            else if (dms == null)
            {
                result = new DegreeMinuteSecond((double)arg);
                return true;
            }
            else
            {
                result = dms;
                return true;
            }
        }

        /// <summary>
        /// Format this object to a Degrees-Minutes-Seconds string.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        protected virtual string DoFormat(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg), "The argument cannot be null.");
            }

            DegreeMinuteSecond dms = null;
            if (!DegreeMinuteSecondFormatInfo.TryParse(arg, out dms))
            {
                // Provide default formatting if the argument is not as expected (Double or DegreeMinuteSecond).
                return FormatUnexpectedDataType(format, arg);
            }

            // Adjust for missing format.
#pragma warning disable S3900
            string f = string.Empty;
            if (string.IsNullOrWhiteSpace(format))
            {
                // Default to everything.
                f = "DMS";
            }
            else
            {
                f = format.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
            }
#pragma warning restore S3900

            UpdateScaleFromFormatString(format);

            string secondsFormat = "S";
            string minutesFormat = "M";
            if (f.Contains(secondsFormat))
            {
                return ToDegreeMinuteSecond(dms);
            }
            else if (f.Contains(minutesFormat))
            {
                return ToDegreeMinute(dms);
            }
            else
            {
                return ToDegree(dms);
            }
        }

        /// <summary>
        /// Attempt to read a scale value from the format string, expects the last character to be numeric.
        /// </summary>
        /// <param name="format"></param>
        protected void UpdateScaleFromFormatString(string format)
        {
#pragma warning disable S3900

            int scale;
            if (string.IsNullOrEmpty(format) || format.Trim().Length == 1)
            {
                // Do nothing.
            }
            else if (int.TryParse(format.Substring(format.Length - 1), out scale))
            {
                this.Scale = scale;
            }
#pragma warning restore S3900
        }

        /// <summary>
        /// Converts the value of a specified object to an equivalent string representation using specified format and culture-specific formatting information.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return DoFormat(format, arg, formatProvider);
        }

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType"></param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
}
