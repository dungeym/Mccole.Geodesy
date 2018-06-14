using DevStreet.Geodesy.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStreet.Geodesy.Extension
{
    /// <summary>
    /// Extension methods for testing the tolerance of a Ssytem.Double value.
    /// </summary>
    public static class FloatToleranceExtension
    {
        /// <summary>
        /// The default scale used to test the 2 values.
        /// </summary>
        public const double DefaultTolerance = 0.00001;

        /// <summary>
        /// Determine if two the floating point numbers are within the DefaultTolerance.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool WithinTolerance(this double @this, double value)
        {
            return WithinTolerance(@this, value, FloatToleranceExtension.DefaultTolerance);
        }

        /// <summary>
        /// Determine if two the floating point numbers are within the tolerance.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool WithinTolerance(this double @this, double value, double tolerance)
        {
            return (Math.Abs(@this - value) < tolerance);
        }
    }
}
