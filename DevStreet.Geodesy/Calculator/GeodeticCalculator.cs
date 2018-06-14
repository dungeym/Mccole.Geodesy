using DevStreet.Geodesy.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStreet.Geodesy.Calculator
{
    /// <summary>
    /// Provides a series of Geodetic functions that can be used measure the earth's surface.
    /// </summary>
    public class GeodeticCalculator : CalculatorBase, IGeodeticCalculator
    {
        private static Lazy<IGeodeticCalculator> _instance = new Lazy<IGeodeticCalculator>(() => { return new GeodeticCalculator(); });

        private GeodeticCalculator()
        {
        }

        /// <summary>
        /// Singleton instance of IGeodeticCalculator.
        /// </summary>
        public static IGeodeticCalculator Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        /// <summary>
        /// Determine if a polygon encloses pole.
        /// Sum of course deltas around pole is 0° rather than normal ±360°.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns>True if the polygon encloses a pole.</returns>
        private static bool IsPoleEnclosedBy(IEnumerable<ICoordinate> polygon)
        {
            // http://blog.element84.com/determining-if-a-spherical-polygon-contains-a-pole.html

            // Chris Veness: any better test than this?
            var ΣΔ = 0D;
            var prevBrng = GeodeticCalculator.Instance.Bearing(polygon.First(), polygon.ElementAt(1));
            for (var v = 0; v < polygon.Count() - 1; v++)
            {
                var bearing = GeodeticCalculator.Instance.Bearing(polygon.ElementAt(v), polygon.ElementAt(v + 1));
                var finalBearing = GeodeticCalculator.Instance.FinalBearing(polygon.ElementAt(v), polygon.ElementAt(v + 1));
                ΣΔ += (bearing - prevBrng + 540) % 360 - 180;
                ΣΔ += (finalBearing - bearing + 540) % 360 - 180;
                prevBrng = finalBearing;
            }

            var initialBearing = GeodeticCalculator.Instance.Bearing(polygon.First(), polygon.ElementAt(1));
            ΣΔ += (initialBearing - prevBrng + 540) % 360 - 180;

            // Chris Veness: fix (intermittent) edge crossing pole - eg (85,90), (85,0), (85,-90)
            // 0°-ISH.
            var enclosed = Math.Abs(ΣΔ) < 90;
            return enclosed;
        }

        /// <summary>
        /// Calculate how far the point is along a track from the start-point, heading towards the end-point.
        /// <para>That is, if a perpendicular is drawn from the point to the (great circle) path, the along-track distance is the distance from the start point to where the perpendicular crosses the path.</para>
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <returns>The distance along great circle to point nearest point A in metres.</returns>
        public double AlongTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint)
        {
            return AlongTrackDistance(pointA, startPoint, endPoint, Radius.Mean);
        }

        /// <summary>
        /// Calculate how far the point is along a track from the start-point, heading towards the end-point.
        /// <para>That is, if a perpendicular is drawn from the point to the (great circle) path, the along-track distance is the distance from the start point to where the perpendicular crosses the path.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>The distance along great circle to point nearest point A in the same units as the radius.</returns>
        public double AlongTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint, double radius)
        {
            if (startPoint == null)
            {
                throw new ArgumentNullException(nameof(startPoint), "The argument cannot be null.");
            }
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint), "The argument cannot be null.");
            }

            ValidateRadius(radius);

            var δ13 = startPoint.DistanceTo(pointA, radius) / radius;
            var θ13 = startPoint.BearingTo(pointA).ToRadians();
            var θ12 = startPoint.BearingTo(endPoint).ToRadians();

            var δxt = Math.Asin(Math.Sin(δ13) * Math.Sin(θ13 - θ12));

            var δat = Math.Acos(Math.Cos(δ13) / Math.Abs(Math.Cos(δxt)));

            var result = δat * Math.Sign(Math.Cos(θ12 - θ13)) * radius;
            return result;
        }

        /// <summary>
        /// Calculate the area of a spherical polygon where the sides of the polygon are great circle arcs joining the vertices.
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="polygon">Array of points defining vertices of the polygon.</param>
        /// <returns>The area of the polygon, in the same units as radius.</returns>
        public double AreaOf(IEnumerable<ICoordinate> polygon)
        {
            return AreaOf(polygon, Radius.Mean);
        }

        /// <summary>
        /// Calculate the area of a spherical polygon where the sides of the polygon are great circle arcs joining the vertices.
        /// </summary>
        /// <param name="polygon">Array of points defining vertices of the polygon.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>The area of the polygon, in the same units as radius.</returns>
        public double AreaOf(IEnumerable<ICoordinate> polygon, double radius)
        {
            /*
             * Uses method due to Karney - http://osgeo-org.1560.x6.nabble.com/Area-of-a-spherical-polygon-td3841625.html
             * For each edge of the polygon, tan(E/2) = tan(Δλ/2)·(tan(φ1/2) + tan(φ2/2)) / (1 + tan(φ1/2)·tan(φ2/2))
             * Where E is the spherical excess of the trapezium obtained by extending the edge to the equator
             */

            ValidateRadius(radius);

            List<ICoordinate> shape = new List<ICoordinate>(polygon);

            // Close polygon so that last point equals first point.
            var closed = shape.First().Equals(shape.Last());
            if (!closed)
            {
                shape.Add(shape.First());
            }

            // Spherical excess in steradians.
            var s = 0D;

            var vertices = shape.Count - 1;
            for (var v = 0; v < vertices; v++)
            {
                var φ1 = shape.ElementAt(v).Latitude.ToRadians();
                var φ2 = shape.ElementAt(v + 1).Latitude.ToRadians();
                var Δλ = (shape.ElementAt(v + 1).Longitude - shape.ElementAt(v).Longitude).ToRadians();
                var E = 2 * Math.Atan2(Math.Tan(Δλ / 2) * (Math.Tan(φ1 / 2) + Math.Tan(φ2 / 2)), 1 + Math.Tan(φ1 / 2) * Math.Tan(φ2 / 2));
                s += E;
            }

            if (IsPoleEnclosedBy(shape))
            {
                s = Math.Abs(s) - 2 * Math.PI;
            }

            // Area in units of radius.
            var result = Math.Abs(s * radius * radius);
            return result;
        }

        /// <summary>
        /// Calculate the (initial) bearing from point A to point B.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Initial bearing in degrees from north.</returns>
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
            var y = Math.Sin(Δλ) * Math.Cos(φ2);
            var x = Math.Cos(φ1) * Math.Sin(φ2) -
                    Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(Δλ);
            var θ = Math.Atan2(y, x);

            var result = (θ.ToDegrees() + 360) % 360;
            return result;
        }

        /// <summary>
        /// Calculate the pair of meridians at which a great circle defined by two points crosses the given latitude.
        /// <para>If the great circle doesn't reach the given latitude, null is returned.</para>
        /// </summary>
        /// <param name="point1">First point defining great circle.</param>
        /// <param name="point2">Second point defining great circle.</param>
        /// <param name="latitude">Latitude crossings are to be determined for.</param>
        /// <returns>Object containing { Longitude1, Longitude2 } or null if given latitude not reached.</returns>
        public Tuple<double, double> CrossingParallels(ICoordinate point1, ICoordinate point2, double latitude)
        {
            if (point1 == null)
            {
                throw new ArgumentNullException(nameof(point1), "The argument cannot be null.");
            }
            if (point2 == null)
            {
                throw new ArgumentNullException(nameof(point2), "The argument cannot be null.");
            }

            var φ = latitude.ToRadians();
            var φ1 = point1.Latitude.ToRadians();
            var λ1 = point1.Longitude.ToRadians();
            var φ2 = point2.Latitude.ToRadians();
            var λ2 = point2.Longitude.ToRadians();

            var Δλ = λ2 - λ1;

            var x = Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(φ) * Math.Sin(Δλ);
            var y = Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(φ) * Math.Cos(Δλ) - Math.Cos(φ1) * Math.Sin(φ2) * Math.Cos(φ);
            var z = Math.Cos(φ1) * Math.Cos(φ2) * Math.Sin(φ) * Math.Sin(Δλ);

            // Great circle doesn't reach latitude.
            if (z * z > x * x + y * y)
            {
                return null;
            }

            // Longitude at maximum latitude.
            var λm = Math.Atan2(-y, x);

            // Δλ from λm to intersection points.
            var Δλi = Math.Acos(z / Math.Sqrt(x * x + y * y));

            var λi1 = λ1 + λm - Δλi;
            var λi2 = λ1 + λm + Δλi;

            // Normalise to −180..+180°
            var result = new Tuple<double, double>((λi1.ToDegrees() + 540) % 360 - 180, (λi2.ToDegrees() + 540) % 360 - 180);
            return result;
        }

        /// <summary>
        /// Calculate the (signed) distance from point A to great circle defined by start-point and end-point.
        /// <para>Uses the mean radius of the Earth.</para>
        /// <para>Also known as Cross Track Error.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <returns>Distance to great circle (negative if to left, positive if to right of path) in metres.</returns>
        public double CrossTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint)
        {
            return CrossTrackDistance(pointA, startPoint, endPoint, Radius.Mean);
        }

        /// <summary>
        /// Calculate the (signed) distance from point A to great circle defined by start-point and end-point.
        /// <para>Also known as Cross Track Error.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>Distance to great circle (negative if to left, positive if to right of path) in the same units as the radius.</returns>
        public double CrossTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint, double radius)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (startPoint == null)
            {
                throw new ArgumentNullException(nameof(startPoint), "The argument cannot be null.");
            }
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint), "The argument cannot be null.");
            }

            ValidateRadius(radius);

            var δ13 = startPoint.DistanceTo(pointA, radius) / radius;
            var θ13 = startPoint.BearingTo(pointA).ToRadians();
            var θ12 = startPoint.BearingTo(endPoint).ToRadians();

            var δxt = Math.Asin(Math.Sin(δ13) * Math.Sin(θ13 - θ12));

            var result = δxt * radius;
            return result;
        }

        /// <summary>
        /// Calculate the destination point from point A having travelled the given distance on the given initial bearing.
        /// <para>Bearing normally varies around path followed.</para>
        /// <para>Uses the mean radius of the Earth in metres.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="distance">Distance travelled, in same units as the earth's radius (metres).</param>
        /// <param name="bearing">Initial bearing in degrees from north.</param>
        /// <returns>The destination point.</returns>
        public ICoordinate Destination(ICoordinate pointA, double distance, double bearing)
        {
            return Destination(pointA, distance, bearing, Radius.Mean);
        }

        /// <summary>
        /// Calculate the destination point from point A having travelled the given distance on the given initial bearing.
        /// <para>Bearing normally varies around path followed.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="distance">Distance travelled, in same units as the earth's radius.</param>
        /// <param name="bearing">Initial bearing in degrees from north.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>The destination point.</returns>
        public ICoordinate Destination(ICoordinate pointA, double distance, double bearing, double radius)
        {
            /*
             * sinφ2 = sinφ1⋅cosδ + cosφ1⋅sinδ⋅cosθ
             * tanΔλ = sinθ⋅sinδ⋅cosφ1 / cosδ−sinφ1⋅sinφ2
             * See http://mathforum.org/library/drmath/view/52049.html for derivation.
            */

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
            var θ = bearing.ToRadians();

            var φ1 = pointA.Latitude.ToRadians();
            var λ1 = pointA.Longitude.ToRadians();

            var sinφ1 = Math.Sin(φ1);
            var cosφ1 = Math.Cos(φ1);
            var sinδ = Math.Sin(δ);
            var cosδ = Math.Cos(δ);
            var sinθ = Math.Sin(θ);
            var cosθ = Math.Cos(θ);

            var sinφ2 = sinφ1 * cosδ + cosφ1 * sinδ * cosθ;
            var φ2 = Math.Asin(sinφ2);
            var y = sinθ * sinδ * cosφ1;
            var x = cosδ - sinφ1 * sinφ2;
            var λ2 = λ1 + Math.Atan2(y, x);

            // Normalise to −180..+180°.
            var result = base.NewCoordinate(φ2.ToDegrees(), (λ2.ToDegrees() + 540) % 360 - 180);
            return result;
        }

        /// <summary>
        /// Calculate the distance between 2 points (using haversine formula).
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Distance between this point and destination point, in same units as radius.</returns>
        public double Distance(ICoordinate pointA, ICoordinate pointB)
        {
            return Distance(pointA, pointB, Radius.Mean);
        }

        /// <summary>
        /// Calculate the distance between 2 points (using haversine formula).
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>Distance between this point and destination point, in same units as radius.</returns>
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

            /*
             * a = sin²(Δφ/2) + cos(φ1)⋅cos(φ2)⋅sin²(Δλ/2)
             * tanδ = √(a) / √(1−a)
             * See http://mathforum.org/library/drmath/view/51879.html for derivation.
             */

            var φ1 = pointA.Latitude.ToRadians();
            var λ1 = pointA.Longitude.ToRadians();
            var φ2 = pointB.Latitude.ToRadians();
            var λ2 = pointB.Longitude.ToRadians();
            var Δφ = φ2 - φ1;
            var Δλ = λ2 - λ1;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2)
                  + Math.Cos(φ1) * Math.Cos(φ2)
                  * Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = radius * c;

            return d;
        }

        /// <summary>
        /// Using the line A->B calculate the shortest distance from point C that line.
        /// <para>If point C is outside the bounds of A or B (a perpendicular line cannot be made to A or B) then -1 is returned.</para>
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <returns>The distance in the same units as the radius, unless C extends past the line A->B in which case -1.</returns>
        public double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC)
        {
            return DistanceToLine(pointA, pointB, pointC, Radius.Mean);
        }

        /// <summary>
        /// Using the line A->B calculate the shortest distance from point C that line.
        /// <para>If point C is outside the bounds of A or B (a perpendicular line cannot be made to A or B) then -1 is returned.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <returns>The distance in the same units as the radius, unless C extends past the line A->B in which case -1.</returns>
        public double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius)
        {
            ICoordinate pointX;
            return DistanceToLine(pointA, pointB, pointC, radius, out pointX);
        }

        /// <summary>
        /// Using the line A->B calculate the shortest distance from point C that line.
        /// <para>If point C is outside the bounds of A or B (a perpendicular line cannot be made to A or B) then -1 is returned.</para>
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="pointX">Out, the point on the line (A->B) that is perpendicular to point C.</param>
        /// <returns>The distance in the same units as the radius, unless C extends past the line A->B in which case -1.</returns>
        public double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, out ICoordinate pointX)
        {
            return DistanceToLine(pointA, pointB, pointC, Radius.Mean, out pointX);
        }

        /// <summary>
        /// Using the line A->B calculate the shortest distance from point C that line.
        /// <para>If point C is outside the bounds of A or B (a perpendicular line cannot be made to A or B) then -1 is returned.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <param name="pointX">Out, the point on the line (A->B) that is perpendicular to point C.</param>
        /// <returns>The distance in the same units as the radius, unless C extends past the line A->B in which case -1.</returns>
        public double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius, out ICoordinate pointX)
        {
            pointX = default(ICoordinate);

            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (pointB == null)
            {
                throw new ArgumentNullException(nameof(pointB), "The argument cannot be null.");
            }
            if (pointC == null)
            {
                throw new ArgumentNullException(nameof(pointC), "The argument cannot be null.");
            }

            pointX = PerpendicularPoint(pointA, pointB, pointC);
            if (pointX == null)
            {
                return -1;
            }
            else
            {
                var d = GeodeticCalculator.Instance.Distance(pointC, pointX);
                return d;
            }
        }

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        public double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC)
        {
            return DistanceToPlane(pointA, pointB, pointC, Radius.Mean);
        }

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        public double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius)
        {
            ICoordinate pointX;
            return DistanceToPlane(pointA, pointB, pointC, radius, out pointX);
        }

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="pointX">Out, the point on the plane (A->B) that is perpendicular to point C.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        public double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, out ICoordinate pointX)
        {
            return DistanceToPlane(pointA, pointB, pointC, Radius.Mean, out pointX);
        }

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <param name="pointX">Out, the point on the plane (A->B) that is perpendicular to point C.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        public double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius, out ICoordinate pointX)
        {
            pointX = default(ICoordinate);

            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (pointB == null)
            {
                throw new ArgumentNullException(nameof(pointB), "The argument cannot be null.");
            }
            if (pointC == null)
            {
                throw new ArgumentNullException(nameof(pointC), "The argument cannot be null.");
            }

            // To accommodate where the line A->B does not extend far enough that a perpendicular line could be drawn from
            // it to pointC we extend the A->B line by the greatest of the 2 distances, A->B and A->C.
            var abDistance = GeodeticCalculator.Instance.Distance(pointA, pointB);
            var acDistance = GeodeticCalculator.Instance.Distance(pointA, pointC);
            var distanceMax = abDistance > acDistance ? abDistance : acDistance;
            ICoordinate extendedPointA = GeodeticCalculator.Instance.Destination(pointA, distanceMax, GeodeticCalculator.Instance.Bearing(pointA, pointB));
            ICoordinate extendedPointB = GeodeticCalculator.Instance.Destination(pointB, distanceMax, GeodeticCalculator.Instance.Bearing(pointB, pointA));

            pointX = PerpendicularPoint(extendedPointA, extendedPointB, pointC);
            var d = GeodeticCalculator.Instance.Distance(pointC, pointX);
            return d;
        }

        /// <summary>
        /// Calculate final bearing arriving at destination point from point A.
        /// <para>The final bearing will differ from the initial bearing by varying degrees according to distance and latitude.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Final bearing in degrees from north.</returns>
        public double FinalBearing(ICoordinate pointA, ICoordinate pointB)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (pointB == null)
            {
                throw new ArgumentNullException(nameof(pointB), "The argument cannot be null.");
            }

            // Get initial bearing from destination point to this point & reverse it by adding 180°.
            var result = (pointB.BearingTo(pointA) + 180) % 360;
            return result;
        }

        /// <summary>
        /// Calculate the point at a given fraction along the track between point A and point B.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <param name="fraction">Fraction between the two points (0 = this point, 1 = specified point).</param>
        /// <returns>The intermediate point between this point and destination point.</returns>
        public ICoordinate IntermediatePoint(ICoordinate pointA, ICoordinate pointB, double fraction)
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
            var λ1 = pointA.Longitude.ToRadians();
            var φ2 = pointB.Latitude.ToRadians();
            var λ2 = pointB.Longitude.ToRadians();
            var sinφ1 = Math.Sin(φ1);
            var cosφ1 = Math.Cos(φ1);
            var sinλ1 = Math.Sin(λ1);
            var cosλ1 = Math.Cos(λ1);
            var sinφ2 = Math.Sin(φ2);
            var cosφ2 = Math.Cos(φ2);
            var sinλ2 = Math.Sin(λ2);
            var cosλ2 = Math.Cos(λ2);

            // Distance between points.
            var Δφ = φ2 - φ1;
            var Δλ = λ2 - λ1;
            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2)
                + Math.Cos(φ1) * Math.Cos(φ2) * Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var δ = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var A = Math.Sin((1 - fraction) * δ) / Math.Sin(δ);
            var B = Math.Sin(fraction * δ) / Math.Sin(δ);

            var x = A * cosφ1 * cosλ1 + B * cosφ2 * cosλ2;
            var y = A * cosφ1 * sinλ1 + B * cosφ2 * sinλ2;
            var z = A * sinφ1 + B * sinφ2;

            var φ3 = Math.Atan2(z, Math.Sqrt(x * x + y * y));
            var λ3 = Math.Atan2(y, x);

            // Normalise to −180..+180°.
            var result = base.NewCoordinate(φ3.ToDegrees(), (λ3.ToDegrees() + 540) % 360 - 180);
            return result;
        }

        /// <summary>
        /// Calculate the point of intersection of two paths defined by points and bearings.
        /// </summary>
        /// <param name="point1">First point.</param>
        /// <param name="bearing1">Initial bearing from first point.</param>
        /// <param name="point2">Second point.</param>
        /// <param name="bearing2">Initial bearing from second point.</param>
        /// <returns>Destination point (null if no unique intersection defined).</returns>
        public ICoordinate Intersection(ICoordinate point1, double bearing1, ICoordinate point2, double bearing2)
        {
            if (point1 == null)
            {
                throw new ArgumentNullException(nameof(point1), "The argument cannot be null.");
            }
            if (point2 == null)
            {
                throw new ArgumentNullException(nameof(point2), "The argument cannot be null.");
            }

            ValidateBearing(bearing1, nameof(bearing1));
            ValidateBearing(bearing2, nameof(bearing2));

            // See http://edwilliams.org/avform.htm#Intersection

            var φ1 = point1.Latitude.ToRadians();
            var λ1 = point1.Longitude.ToRadians();
            var φ2 = point2.Latitude.ToRadians();
            var λ2 = point2.Longitude.ToRadians();
            var θ13 = bearing1.ToRadians();
            var θ23 = bearing2.ToRadians();
            var Δφ = φ2 - φ1;
            var Δλ = λ2 - λ1;

            // Angular distance p1-p2.
            var δ12 = 2 * Math.Asin(Math.Sqrt(Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2)
                + Math.Cos(φ1) * Math.Cos(φ2) * Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2)));

            if (δ12.WithinTolerance(0))
            {
                return null;
            }

            // Initial/final bearings between points
            var θa = Math.Acos((Math.Sin(φ2) - Math.Sin(φ1) * Math.Cos(δ12)) / (Math.Sin(δ12) * Math.Cos(φ1)));

            if (double.IsNaN(θa))
            {
                // Protect against rounding.
                θa = 0;
            }

            var θb = Math.Acos((Math.Sin(φ1) - Math.Sin(φ2) * Math.Cos(δ12)) / (Math.Sin(δ12) * Math.Cos(φ2)));

            var θ12 = Math.Sin(λ2 - λ1) > 0 ? θa : 2 * Math.PI - θa;
            var θ21 = Math.Sin(λ2 - λ1) > 0 ? 2 * Math.PI - θb : θb;

            var α1 = θ13 - θ12; // angle 2-1-3
            var α2 = θ21 - θ23; // angle 1-2-3

            // Infinite intersections.
            if (Math.Sin(α1).WithinTolerance(0) && Math.Sin(α2).WithinTolerance(0))
            {
                return null;
            }

            // Ambiguous intersection.
            if (Math.Sin(α1) * Math.Sin(α2) < 0)
            {
                return null;
            }

            var α3 = Math.Acos(-Math.Cos(α1) * Math.Cos(α2) + Math.Sin(α1) * Math.Sin(α2) * Math.Cos(δ12));
            var δ13 = Math.Atan2(Math.Sin(δ12) * Math.Sin(α1) * Math.Sin(α2), Math.Cos(α2) + Math.Cos(α1) * Math.Cos(α3));
            var φ3 = Math.Asin(Math.Sin(φ1) * Math.Cos(δ13) + Math.Cos(φ1) * Math.Sin(δ13) * Math.Cos(θ13));
            var Δλ13 = Math.Atan2(Math.Sin(θ13) * Math.Sin(δ13) * Math.Cos(φ1), Math.Cos(δ13) - Math.Sin(φ1) * Math.Sin(φ3));
            var λ3 = λ1 + Δλ13;

            // Normalise to −180..+180°.
            var result = base.NewCoordinate(φ3.ToDegrees(), (λ3.ToDegrees() + 540) % 360 - 180);
            return result;
        }

        /// <summary>
        /// Calculate the maximum latitude reached when travelling on a great circle on given bearing from this point ('Clairaut's formula').
        /// <para>Negate the result for the minimum latitude (in the Southern hemisphere).</para>
        /// <para>The maximum latitude is independent of longitude; it will be the same for all points on a given latitude.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="bearing">Initial bearing.</param>
        /// <returns>The maximum latitude.</returns>
        public double MaximumLatitude(ICoordinate pointA, double bearing)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }

            ValidateBearing(bearing);

            var θ = bearing.ToRadians();

            var φ = pointA.Latitude.ToRadians();

            var φMax = Math.Acos(Math.Abs(Math.Sin(θ) * Math.Cos(φ)));

            var result = φMax.ToDegrees();
            return result;
        }

        /// <summary>
        /// Calculate the midpoint between point A and point B.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Midpoint between this point and the supplied point.</returns>
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

            /*
             * φm = atan2( sinφ1 + sinφ2, √( (cosφ1 + cosφ2⋅cosΔλ) ⋅ (cosφ1 + cosφ2⋅cosΔλ) ) + cos²φ2⋅sin²Δλ )
             * λm = λ1 + atan2(cosφ2⋅sinΔλ, cosφ1 + cosφ2⋅cosΔλ)
             * See http://mathforum.org/library/drmath/view/51822.html for derivation.
             */

            var φ1 = pointA.Latitude.ToRadians();
            var λ1 = pointA.Longitude.ToRadians();
            var φ2 = pointB.Latitude.ToRadians();
            var Δλ = (pointB.Longitude - pointA.Longitude).ToRadians();

            var Bx = Math.Cos(φ2) * Math.Cos(Δλ);
            var By = Math.Cos(φ2) * Math.Sin(Δλ);

            var x = Math.Sqrt((Math.Cos(φ1) + Bx) * (Math.Cos(φ1) + Bx) + By * By);
            var y = Math.Sin(φ1) + Math.Sin(φ2);
            var φ3 = Math.Atan2(y, x);

            var λ3 = λ1 + Math.Atan2(By, Math.Cos(φ1) + Bx);

            // Normalise to −180..+180°.
            var result = base.NewCoordinate(φ3.ToDegrees(), (λ3.ToDegrees() + 540) % 360 - 180);
            return result;
        }

        /// <summary>
        /// Calculate the point on the line A->B that is perpendicular from the line to point C.
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the line A->B.</param>
        /// <returns>Null if pointC extends past the line A->B.</returns>
        public ICoordinate PerpendicularPoint(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC)
        {
            if (pointA == null)
            {
                throw new ArgumentNullException(nameof(pointA), "The argument cannot be null.");
            }
            if (pointB == null)
            {
                throw new ArgumentNullException(nameof(pointB), "The argument cannot be null.");
            }
            if (pointC == null)
            {
                throw new ArgumentNullException(nameof(pointC), "The argument cannot be null.");
            }

            // http://csharphelper.com/blog/2016/09/find-the-shortest-distance-between-a-point-and-a-line-segment-in-c/

            double dx = pointB.Longitude - pointA.Longitude;
            double dy = pointB.Latitude - pointA.Latitude;

            if (dx.WithinTolerance(0) && dy.WithinTolerance(0))
            {
                // The point (pointC) is not perpendicular to the line A->B.
                return default(ICoordinate);
            }
            else
            {
                // Calculate the t that minimizes the distance.
                double t = ((pointC.Longitude - pointA.Longitude) * dx + (pointC.Latitude - pointA.Latitude) * dy) / (dx * dx + dy * dy);

                // See if this represents one of the segment's end points or a point in the middle.
                if (t < 0)
                {
                    return NewCoordinate(pointA.Latitude, pointA.Longitude);
                }
                else if (t > 1)
                {
                    return NewCoordinate(pointB.Latitude, pointB.Longitude);
                }
                else
                {
                    return NewCoordinate(pointA.Latitude + t * dy, pointA.Longitude + t * dx);
                }
            }
        }
    }
}
