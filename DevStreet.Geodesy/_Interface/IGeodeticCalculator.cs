using System;
using System.Collections.Generic;

namespace DevStreet.Geodesy
{
    /// <summary>
    /// A series of Geodetic functions that can be used measure the earth's surface.
    /// </summary>
    public interface IGeodeticCalculator : IGeodeticFunctions
    {
        /// <summary>
        /// Calculate how far the point is along a track from the start-point, heading towards the end-point.
        /// <para>That is, if a perpendicular is drawn from the point to the (great circle) path, the along-track distance is the distance from the start point to where the perpendicular crosses the path.</para>
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <returns>The distance along great circle to point nearest point A in metres.</returns>
        double AlongTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint);

        /// <summary>
        /// Calculate how far the point is along a track from the start-point, heading towards the end-point.
        /// <para>That is, if a perpendicular is drawn from the point to the (great circle) path, the along-track distance is the distance from the start point to where the perpendicular crosses the path.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>The distance along great circle to point nearest point A in the same units as the radius.</returns>
        double AlongTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint, double radius);

        /// <summary>
        /// Calculate the area of a spherical polygon where the sides of the polygon are great circle arcs joining the vertices.
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="polygon">Array of points defining vertices of the polygon.</param>
        /// <returns>The area of the polygon, in metres.</returns>
        double AreaOf(IEnumerable<ICoordinate> polygon);

        /// <summary>
        /// Calculate the area of a spherical polygon where the sides of the polygon are great circle arcs joining the vertices.
        /// </summary>
        /// <param name="polygon">Array of points defining vertices of the polygon.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>The area of the polygon, in the same units as radius.</returns>
        double AreaOf(IEnumerable<ICoordinate> polygon, double radius);

        /// <summary>
        /// Calculate the pair of meridians at which a great circle defined by two points crosses the given latitude.
        /// <para>If the great circle doesn't reach the given latitude, null is returned.</para>
        /// </summary>
        /// <param name="point1">First point defining great circle.</param>
        /// <param name="point2">Second point defining great circle.</param>
        /// <param name="latitude">Latitude crossings are to be determined for.</param>
        /// <returns>Object containing { Longitude1, Longitude2 } or null if given latitude not reached.</returns>
        Tuple<double, double> CrossingParallels(ICoordinate point1, ICoordinate point2, double latitude);

        /// <summary>
        /// Calculate the (signed) distance from point A to great circle defined by start-point and end-point.
        /// <para>Uses the mean radius of the Earth.</para>
        /// <para>Also known as Cross Track Error.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <returns>Distance to great circle (negative if to left, positive if to right of path) in metres.</returns>
        double CrossTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint);

        /// <summary>
        /// Calculate the (signed) distance from point A to great circle defined by start-point and end-point.
        /// <para>Also known as Cross Track Error.</para>
        /// </summary>
        /// <param name="pointA">The point to calculate the distance from.</param>
        /// <param name="startPoint">Start point of great circle path.</param>
        /// <param name="endPoint">End point of great circle path.</param>
        /// <param name="radius">Radius of earth.</param>
        /// <returns>Distance to great circle (negative if to left, positive if to right of path) in the same units as the radius.</returns>
        double CrossTrackDistance(ICoordinate pointA, ICoordinate startPoint, ICoordinate endPoint, double radius);

        /// <summary>
        /// Using the line A->B calculate the shortest distance from point C that line.
        /// <para>If point C is outside the bounds of A or B (a perpendicular line cannot be made to A or B) then -1 is returned.</para>
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <returns>The distance in the same units as the radius, unless C extends past the line A->B in which case -1.</returns>
        double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC);

        /// <summary>
        /// Using the line A->B calculate the shortest distance from point C that line.
        /// <para>If point C is outside the bounds of A or B (a perpendicular line cannot be made to A or B) then -1 is returned.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <returns>The distance in the same units as the radius, unless C extends past the line A->B in which case -1.</returns>
        double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius);

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
        double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, out ICoordinate pointX);

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
        double DistanceToLine(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius, out ICoordinate pointX);

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC);

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius);

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// <para>Uses the mean radius of the Earth.</para>
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="pointX">Out, the point on the plane (A->B) that is perpendicular to point C.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, out ICoordinate pointX);

        /// <summary>
        /// Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the plane A->B.</param>
        /// <param name="radius">The radius of the earth.</param>
        /// <param name="pointX">Out, the point on the plane (A->B) that is perpendicular to point C.</param>
        /// <returns>The distance in the same units as the radius.</returns>
        double DistanceToPlane(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC, double radius, out ICoordinate pointX);

        /// <summary>
        /// Calculate the final bearing arriving at the destination point from point A.
        /// <para>The final bearing will differ from the initial bearing by varying degrees according to distance and latitude.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <returns>Final bearing in degrees from north.</returns>
        double FinalBearing(ICoordinate pointA, ICoordinate pointB);

        /// <summary>
        /// Calculate the point at a given fraction along the track between point A and point B.
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="pointB">The end point.</param>
        /// <param name="fraction">Fraction between the two points (0 = this point, 1 = specified point).</param>
        /// <returns>The intermediate point between this point and destination point.</returns>
        ICoordinate IntermediatePoint(ICoordinate pointA, ICoordinate pointB, double fraction);

        /// <summary>
        /// Calculate the point of intersection of two paths defined by points and bearings.
        /// </summary>
        /// <param name="point1">First point.</param>
        /// <param name="bearing1">Initial bearing from first point.</param>
        /// <param name="point2">Second point.</param>
        /// <param name="bearing2">Initial bearing from second point.</param>
        /// <returns>Destination point (null if no unique intersection defined).</returns>
        ICoordinate Intersection(ICoordinate point1, double bearing1, ICoordinate point2, double bearing2);

        /// <summary>
        /// Calculate the maximum latitude reached when travelling on a great circle on given bearing from this point ('Clairaut's formula').
        /// <para>Negate the result for the minimum latitude (in the Southern hemisphere).</para>
        /// <para>The maximum latitude is independent of longitude; it will be the same for all points on a given latitude.</para>
        /// </summary>
        /// <param name="pointA">The start point.</param>
        /// <param name="bearing">Initial bearing.</param>
        /// <returns>The maximum latitude.</returns>
        double MaximumLatitude(ICoordinate pointA, double bearing);

        /// <summary>
        /// Calculate the point on the line A->B that is perpendicular from the line to point C.
        /// </summary>
        /// <param name="pointA">The start point of a line.</param>
        /// <param name="pointB">The end point of a line.</param>
        /// <param name="pointC">The point from which to find the perpendicular on the line A->B.</param>
        /// <returns>Null if pointC extends parts the line A->B.</returns>
        ICoordinate PerpendicularPoint(ICoordinate pointA, ICoordinate pointB, ICoordinate pointC);
    }
}