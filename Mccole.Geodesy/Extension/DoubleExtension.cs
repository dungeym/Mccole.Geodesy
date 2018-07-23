using Mccole.Geodesy.Formatter;
using System;

namespace Mccole.Geodesy.Extension
{
    /// <summary>
    /// Extension methods for System.Double.
    /// </summary>
    internal static class DoubleExtension
    {
        /// <summary>
        /// Convert a radians value to degrees.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static double ToDegrees(this double @this)
        {
            return @this * 180D / Math.PI;
        }

        /// <summary>
        /// Convert a degrees value to radians.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static double ToRadians(this double @this)
        {
            return @this * Math.PI / 180D;
        }
    }
}
