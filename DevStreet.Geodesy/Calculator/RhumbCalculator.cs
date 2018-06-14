using DevStreet.Geodesy.Extension;
using System;

namespace DevStreet.Geodesy.Calculator
{
    /// <summary>
    /// Provides a series of Geodetic functions that can be used measure the earth's surface using Rhumb lines.
    /// </summary>
    public class RhumbCalculator : CalculatorBase, IGeodeticFunctions
    {
        private static Lazy<IGeodeticFunctions> _instance = new Lazy<IGeodeticFunctions>(() => { return new RhumbCalculator(); });

        private RhumbCalculator()
        {
        }

        /// <summary>
        /// Singleton instance of IGeodeticFunctions for Rhumb calculations.
        /// </summary>
        public static IGeodeticFunctions Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        /// <summary>
        /// Calculate the bearing from point A to point B along a Rhumb line.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Bearing in degrees from north.</returns>
        public double Bearing(ICoordinate pointA, ICoordinate pointB)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (pointB == null)
            {
                throw new ArgumentNullException(nameof(pointB), "The argument cannot be null.");
            }

            var φ1 = pointA.Latitude.ToRadians();
            var φ2 = pointB.Latitude.ToRadians();
            var Δλ = (pointB.Longitude - pointA.Longitude).ToRadians();

            // If dLon over 180° take shorter Rhumb line across the anti-meridian:
            if (Δλ > Math.PI)
            {
                Δλ -= 2 * Math.PI;
            }
            if (Δλ < -Math.PI)
            {
                Δλ += 2 * Math.PI;
            }

            var Δψ = Math.Log(Math.Tan(φ2 / 2 + Math.PI / 4) / Math.Tan(φ1 / 2 + Math.PI / 4));

            var θ = Math.Atan2(Δλ, Δψ);

            return (θ.ToDegrees() + 360) % 360;
        }

        /// <summary>
        /// Calculate the destination point having travelled along a Rhumb line from point A for the given distance on the given bearing.
        /// <para> Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="distance">Distance travelled, in same units as earth radius (default: metres).</param>
        /// <param name="bearing">Bearing in degrees from north.</param>
        /// <returns>The destination point.</returns>
        public ICoordinate Destination(ICoordinate pointA, double distance, double bearing)
        {
            return Destination(pointA, distance, bearing, Radius.Mean);
        }

        /// <summary>
        /// Calculate the destination point having travelled along a Rhumb line from point A for the given distance on the given bearing.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="distance">Distance travelled, in same units as earth radius (default: metres).</param>
        /// <param name="bearing">Bearing in degrees from north.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>The destination point.</returns>
        public ICoordinate Destination(ICoordinate pointA, double distance, double bearing, double radius)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (distance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(distance), "Must not be a negative number.");
            }

            ValidateBearing(bearing);
            ValidateRadius(radius);

            // Angular distance in radians.
            var δ = distance / radius;
            var φ1 = pointA.Latitude.ToRadians();
            var λ1 = pointA.Longitude.ToRadians();
            var θ = bearing.ToRadians();

            var Δφ = δ * Math.Cos(θ);
            var φ2 = φ1 + Δφ;

            // Check for some daft bugger going past the pole, normalise latitude if so.
            if (Math.Abs(φ2) > Math.PI / 2)
            {
                φ2 = φ2 > 0 ? Math.PI - φ2 : -Math.PI - φ2;
            }

            var Δψ = Math.Log(Math.Tan(φ2 / 2 + Math.PI / 4) / Math.Tan(φ1 / 2 + Math.PI / 4));

            // E-W course becomes ill-conditioned with 0/0.
            var q = Math.Abs(Δψ) > 10e-12 ? Δφ / Δψ : Math.Cos(φ1);

            var Δλ = δ * Math.Sin(θ) / q;
            var λ2 = λ1 + Δλ;

            // Normalise to −180..+180°.
            var result = base.NewCoordinate(φ2.ToDegrees(), (λ2.ToDegrees() + 540) % 360 - 180);
            return result;
        }

        /// <summary>
        /// Calculate the distance travelling from point A to point B along a Rhumb line.
        /// <para> Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Distance in km between this point and destination point (same units as radius).</returns>
        public double Distance(ICoordinate pointA, ICoordinate pointB)
        {
            return Distance(pointA, pointB, Radius.Mean);
        }

        /// <summary>
        /// Calculate the distance travelling from point A to point B along a Rhumb line.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>Distance in km between this point and destination point (same units as radius).</returns>
        public double Distance(ICoordinate pointA, ICoordinate pointB, double radius)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (pointB == null)
            {
                throw new ArgumentNullException(nameof(pointB), "The argument cannot be null.");
            }

            ValidateRadius(radius);

            // See http://edwilliams.org/avform.htm#Rhumb

            var φ1 = pointA.Latitude.ToRadians();
            var φ2 = pointB.Latitude.ToRadians();
            var Δφ = φ2 - φ1;
            var Δλ = Math.Abs(pointB.Longitude - pointA.Longitude).ToRadians();

            // if dLon over 180° take shorter rhumb line across the anti-meridian:
            if (Δλ > Math.PI)
            {
                Δλ -= 2 * Math.PI;
            }

            // On Mercator projection, longitude distances shrink by latitude; q is the 'stretch factor'.
            // q becomes ill-conditioned along E-W line (0/0); use empirical tolerance to avoid it.
            var Δψ = Math.Log(Math.Tan(φ2 / 2 + Math.PI / 4) / Math.Tan(φ1 / 2 + Math.PI / 4));
            var q = Math.Abs(Δψ) > 10e-12 ? Δφ / Δψ : Math.Cos(φ1);

            // Distance is Pythagoras on 'stretched' Mercator projection.
            // Angular distance in radians.
            var δ = Math.Sqrt(Δφ * Δφ + q * q * Δλ * Δλ);
            var dist = δ * radius;

            return dist;
        }

        /// <summary>
        /// Calculate the Loxodromic midpoint (along a Rhumb line) between point A and second point B.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">Latitude/longitude of second point.</param>
        /// <returns>Midpoint between this point and second point.</returns>
        public ICoordinate Midpoint(ICoordinate pointA, ICoordinate pointB)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (pointB == null)
            {
                throw new ArgumentNullException(nameof(pointB), "The argument cannot be null.");
            }

            // See http://mathforum.org/kb/message.jspa?messageID=148837

            var φ1 = pointA.Latitude.ToRadians();
            var λ1 = pointA.Longitude.ToRadians();
            var φ2 = pointB.Latitude.ToRadians();
            var λ2 = pointB.Longitude.ToRadians();

            // Crossing anti-meridian.
            if (Math.Abs(λ2 - λ1) > Math.PI)
            {
                λ1 += 2 * Math.PI;
            }

            var φ3 = (φ1 + φ2) / 2;
            var f1 = Math.Tan(Math.PI / 4 + φ1 / 2);
            var f2 = Math.Tan(Math.PI / 4 + φ2 / 2);
            var f3 = Math.Tan(Math.PI / 4 + φ3 / 2);
            var λ3 = ((λ2 - λ1) * Math.Log(f3) + λ1 * Math.Log(f2) - λ2 * Math.Log(f1)) / Math.Log(f2 / f1);

            // Parallel of latitude.
            if (!double.IsInfinity(λ3))
            {
                λ3 = (λ1 + λ2) / 2;
            }

            // Normalise to −180..+180°.
            var result = base.NewCoordinate(φ3.ToDegrees(), (λ3.ToDegrees() + 540) % 360 - 180);
            return result;
        }
    }
}
