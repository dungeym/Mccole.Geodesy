# Mccole.Geodesy

A C# implementation of Geodesic Functions to work with points and paths on the earth's surface using simple spherical trigonometric formulae.

Transcribed from the original [JavaScript work](https://www.movable-type.co.uk/scripts/latlong.html) by [Chris Veness](https://github.com/chrisveness/geodesy) (C) 2005-2016 and published under the same [MIT License](https://opensource.org/licenses/MIT).

## Methods
**AlongTrackDistance**
- Calculate how far the point is along a track from the start-point, heading towards the end-point. 
- That is, if a perpendicular is drawn from the point to the (great circle) path, the along-track distance is the distance from the start point to where the perpendicular crosses the path.

**AreaOf**
- Calculate the area of a spherical polygon where the sides of the polygon are great circle arcs joining the vertices.

**Bearing**
- Calculate the (initial) bearing from point A to point B.

**CrossingParallels**
- Calculate the pair of meridians at which a great circle defined by two points crosses the given latitude.

**CrossTrackDistance**
- Calculate the (signed) distance from point A to great circle defined by start-point and end-point.  
- Also known as 'Cross Track Error'

**Destination**
- Calculate the destination point from point A having travelled the given distance on the given initial bearing.

**Distance**
- Calculate the distance between 2 points (using haversine formula).

**DistanceToLine**
- Using the line A->B calculate the shortest distance from point C that line.

**DistanceToPlane**
- Using a virtual line on the same plane as the line A->B calculate the shortest distance from point C that line.

**FinalBearing**
- Calculate final bearing arriving at destination point from point A.
- The final bearing will differ from the initial bearing by varying degrees according to distance and latitude.

**IntermediatePoint**
- Calculate the point at a given fraction along the track between point A and point B.

**Intersection**
- Calculate the point of intersection of two paths defined by points and bearings.

**MaximumLatitude**
- Calculate the maximum latitude reached when travelling on a great circle on given bearing from this point ('Clairaut's formula').

**Midpoint**
- Calculate the midpoint between point A and point B.

**PerpendicularPoint**
- Calculate the point on the line A->B that is perpendicular from the line to point C.
