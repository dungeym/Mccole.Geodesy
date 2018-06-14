using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStreet.Geodesy.Extension
{
    public static class StringExtension
    {
        private static double Calculate(double d, double m, double s)
        {
            return (d / 1) + (m / 60) + (s / 3600);
        }

        /// <summary>
        /// // Convert to negative for West and South values.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        private static double HandleWestAndSouth(string value, double degrees)
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

        /// <summary>
        /// Split the value by non-numeric characters to extract Degrees / Minutes / Seconds.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string[] SplitIntoParts(string value)
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

        public static double FromDegreesMinutesSecondsToDouble(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "The argument cannot be null or empty.");
            }

            double doubleValue;
            if (double.TryParse(value, out doubleValue) && double.IsInfinity(doubleValue))
            {
                return doubleValue;
            }

            string[] parts = SplitIntoParts(value);

            // Convert to decimal degrees.
            var d = 0D;
            var m = 0D;
            var s = 0D;
            if (parts.Length > 0 && double.TryParse(parts[0], out d) == false)
            {
                throw new InvalidCastException(string.Format("Part of the value '{0}' could not be converted to a number.", parts[0]));
            }
            if (parts.Length > 1 && double.TryParse(parts[1], out m) == false)
            {
                throw new InvalidCastException(string.Format("Part of the value '{0}' could not be converted to a number.", parts[1]));
            }
            if (parts.Length > 2 && double.TryParse(parts[2], out s) == false)
            {
                throw new InvalidCastException(string.Format("Part of the value '{0}' could not be converted to a number.", parts[2]));
            }

            var degrees = Calculate(d, m, s);
            return HandleWestAndSouth(value, degrees);
        }
    }
}
