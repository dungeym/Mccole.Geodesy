using DevStreet.Geodesy.Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevStreet.Geodesy
{
    /// <summary>
    /// Represent a numeric degrees value as a Degrees-Minutes-Seconds value.
    /// </summary>
    public class DegreeMinuteSecond
    {
        private const string DefaultBearingFormatString = "d0";
        private const string DefaultFormatString = "dms0";
        private readonly double _rawValue;

        /// <summary>
        /// Represent a numeric degrees value as a Degrees-Minutes-Seconds value.
        /// </summary>
        /// <param name="value">The raw numeric value.</param>
        public DegreeMinuteSecond(double value)
        {
            _rawValue = value;
        }

        /// <summary>
        /// The raw value converted to a bearing (0° to 360°), negative values are normalised (180° to 360°).
        /// </summary>
        public double Bearing
        {
            get
            {
                return (_rawValue + 360D) % 360D;
            }
        }

        /// <summary>
        /// The degree component of the raw value.
        /// </summary>
        public double Degree
        {
            get
            {
                return Math.Floor(_rawValue);
            }
        }

        /// <summary>
        /// The complete degrees value, including minutes and seconds data.
        /// </summary>
        public double Degrees
        {
            get
            {
                return _rawValue;
            }
        }

        /// <summary>
        /// The raw value converted into a MILS (NATO) bearing.
        /// </summary>
        public double Mils
        {
            get
            {
                return this.Bearing / 6400;
            }
        }

        /// <summary>
        /// The minute component of the raw value.
        /// </summary>
        public double Minute
        {
            get
            {
                return Math.Floor(((_rawValue * 3600) / 60) % 60);
            }
        }

        /// <summary>
        /// The complete minutes value, including seconds data.
        /// </summary>
        public double Minutes
        {
            get
            {
                return ((_rawValue * 3600) / 60) % 60;
            }
        }

        /// <summary>
        /// The second component of the raw value.
        /// </summary>
        public double Second
        {
            get
            {
                return Math.Round((_rawValue * 3600 % 60));
            }
        }

        /// <summary>
        /// The seconds data.
        /// </summary>
        public double Seconds
        {
            get
            {
                return (_rawValue * 3600 % 60);
            }
        }

        private static double TryParseCalculateNumericDegrees(double d, double m, double s)
        {
            return (d / 1) + (m / 60) + (s / 3600);
        }

        private static double TryParseConvertToNegativeIfSouthOrWest(string value, double degrees)
        {
            double result = degrees;
            if (value.IndexOf(Direction.West, StringComparison.OrdinalIgnoreCase) != -1)
            {
                result *= -1;
            }
            else if (value.IndexOf(Direction.South, StringComparison.OrdinalIgnoreCase) != -1)
            {
                result *= -1;
            }

            return result;
        }

        private static string[] TryParseSplitStringIntoDegreeMinuteSecond(string value)
        {
            List<string> split = new List<string>();
            split.Add(string.Empty);

            foreach (char c in value)
            {
                if (char.IsNumber(c) || c == '.')
                {
#pragma warning disable S1643
                    split[split.Count - 1] += c.ToString();
#pragma warning restore S1643
                }
                else
                {
                    split.Add(string.Empty);
                }
            }

            return split.Where((x) => string.IsNullOrWhiteSpace(x) == false).ToArray();
        }

        /// <summary>
        /// Converts the specified string representation of a Degree-Minute-Second to its DegreeMinuteSecond
        /// equivalent and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out DegreeMinuteSecond result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "The argument cannot be null or empty.");
            }

            double doubleValue;
            if (double.TryParse(value, out doubleValue) && double.IsInfinity(doubleValue) == false)
            {
                result = new DegreeMinuteSecond(doubleValue);
                return true;
            }

            string[] parts = TryParseSplitStringIntoDegreeMinuteSecond(value);

            var d = Convert.ToDouble(parts[0]);
            var m = parts.Length >= 2 ? Convert.ToDouble(parts[1]) : 0D;
            var s = parts.Length >= 3 ? Convert.ToDouble(parts[2]) : 0D;
            var degrees = TryParseCalculateNumericDegrees(d, m, s);
            result = new DegreeMinuteSecond(TryParseConvertToNegativeIfSouthOrWest(value, degrees));
            return true;
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Bearing representation.
        /// </summary>
        /// <returns></returns>
        public string ToBearing()
        {
            return this.ToBearing(DefaultBearingFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Bearing representation using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToBearing(string format)
        {
            return this.ToBearing(new BearingFormatInfo(), format);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Bearing representation using the specified format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <returns></returns>
        public string ToBearing(IFormatProvider provider)
        {
            return this.ToBearing(provider, DefaultBearingFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Bearing representation using the specified format and format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToBearing(IFormatProvider provider, string format)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "The argument cannot be null.");
            }
            if (provider is BearingFormatInfo == false)
            {
                throw new InvalidOperationException(string.Format("The IFormatProvider was not of the expected type '{0}', it was a '{1}'.", typeof(BearingFormatInfo).Name, provider.GetType().Name));
            }

            return this.ToString(provider, format);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Compass Point representation.
        /// </summary>
        /// <returns></returns>
        public string ToCompassPoint()
        {
            return this.ToString(new CompassPointFormatInfo());
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Compass Point representation.
        /// </summary>
        /// <returns></returns>
        public string ToCompassPoint(IFormatProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "The argument cannot be null.");
            }
            if (provider is CompassPointFormatInfo == false)
            {
                throw new InvalidOperationException(string.Format("The IFormatProvider was not of the expected type '{0}', it was a '{1}'.", typeof(CompassPointFormatInfo).Name, provider.GetType().Name));
            }

            return this.ToString(provider);
        }

        /// <summary>
        /// Converts the raw value of this instance to a latitude coordinate using the specified format and format information.
        /// </summary>
        /// <returns></returns>
        public string ToLatitude()
        {
            return this.ToLatitude(DefaultFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to a latitude coordinate using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToLatitude(string format)
        {
            return this.ToLatitude(new LatitudeFormatInfo(), format);
        }

        /// <summary>
        /// Converts the raw value of this instance to a latitude coordinate using the specified format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <returns></returns>
        public string ToLatitude(IFormatProvider provider)
        {
            return this.ToLatitude(provider, DefaultFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to a latitude coordinate using the specified format and format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToLatitude(IFormatProvider provider, string format)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "The argument cannot be null.");
            }
            if (provider is LatitudeFormatInfo == false)
            {
                throw new InvalidOperationException(string.Format("The IFormatProvider was not of the expected type '{0}', it was a '{1}'.", typeof(LatitudeFormatInfo).Name, provider.GetType().Name));
            }

            return this.ToString(provider, format);
        }

        /// <summary>
        /// Convert the raw value of this instance to a longitude coordinate.
        /// </summary>
        /// <returns></returns>
        public string ToLongitude()
        {
            return this.ToLongitude(DefaultFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to a longitude coordinate using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToLongitude(string format)
        {
            return this.ToLongitude(new LongitudeFormatInfo(), format);
        }

        /// <summary>
        /// Converts the raw value of this instance to a longitude coordinate using the specified format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <returns></returns>
        public string ToLongitude(IFormatProvider provider)
        {
            return this.ToLongitude(provider, DefaultFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to a longitude coordinate using the specified format and format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToLongitude(IFormatProvider provider, string format)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "The argument cannot be null.");
            }
            if (provider is LongitudeFormatInfo == false)
            {
                throw new InvalidOperationException(string.Format("The IFormatProvider was not of the expected type '{0}', it was a '{1}'.", typeof(LongitudeFormatInfo).Name, provider.GetType().Name));
            }

            return this.ToString(provider, format);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Milliradian (NATO) representation.
        /// </summary>
        /// <returns></returns>
        public string ToMils()
        {
            return this.ToMils(DefaultBearingFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Milliradian (NATO) representation using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToMils(string format)
        {
            return this.ToMils(new MilliradianFormatInfo(), format);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Milliradian (NATO) representation using the specified format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <returns></returns>
        public string ToMils(IFormatProvider provider)
        {
            return this.ToMils(provider, DefaultBearingFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent Milliradian (NATO) representation using the specified format and format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToMils(IFormatProvider provider, string format)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "The argument cannot be null.");
            }
            if (provider is MilliradianFormatInfo == false)
            {
                throw new InvalidOperationException(string.Format("The IFormatProvider was not of the expected type '{0}', it was a '{1}'.", typeof(MilliradianFormatInfo).Name, provider.GetType().Name));
            }

            return this.ToString(provider, format);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(DefaultFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent string representation using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToString(string format)
        {
            return this.ToString(new DegreeMinuteSecondFormatInfo(), format);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent string representation using the specified format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider)
        {
            return this.ToString(provider, DefaultFormatString);
        }

        /// <summary>
        /// Converts the raw value of this instance to its equivalent string representation using the specified format and format information.
        /// </summary>
        /// <param name="provider">An object that supplies specific formatting information.</param>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns></returns>
        public string ToString(IFormatProvider provider, string format)
        {
            string f = string.Format("{{0:{0}}}", format);
            return string.Format(provider, f, this);
        }
    }
}
