using DevStreet.Geodesy.Extensions;
using DevStreet.Geodesy.Formatters;
using System;

namespace DevStreet.Geodesy
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            /*
             * https://www.namekdev.net/2014/06/iterative-version-of-ramer-douglas-peucker-line-simplification-algorithm/
             * https://www.codeproject.com/articles/18936/a-csharp-implementation-of-douglas-peucker-line-ap
             * https://stackoverflow.com/questions/6601824/douglas-peucker-algorithm
             * https://www.movable-type.co.uk/scripts/latlong.html
             * https://stackoverflow.com/questions/4480434/simplification-optimization-of-gps-track
             * https://www.codeproject.com/Articles/18936/A-C-Implementation-of-Douglas-Peucker-Line-Approxi
             * https://en.wikipedia.org/wiki/Ramer%E2%80%93Douglas%E2%80%93Peucker_algorithm
             * http://www.gpsvisualizer.com/tutorials/track_filters.html
             * 
             * https://bost.ocks.org/mike/simplify/
             * https://stackoverflow.com/questions/10558299/visvalingam-whyatt-polyline-simplification-algorithm-clarification
             * 
             * 
             * https://www.strava.com/activities/669258678
             */
             
            Test1(1.23456789D);
            Test1(23.456789);
            Test1(345.6789D);

            Test2(1.23456789D);
            Test2(23.456789);
            Test2(345.6789D);
            
            Test3(1.23456789D);
            Test3(23.456789);
            Test3(345.6789D);
        }

        private static void Test1(double value)
        {
            Console.Clear();
            Console.WriteLine("D => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:D}", value));
            Console.WriteLine("DM => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DM}", value));
            Console.WriteLine("DMS => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DMS}", value));
            Console.WriteLine("Latitude => {0}", string.Format(new LatitudeFormatInfo(), "{0:DMS}", value));
            Console.WriteLine("Longitude => {0}", string.Format(new LongitudeFormatInfo(), "{0:DMS}", value));
            Console.WriteLine("Bearing => {0}", string.Format(new BearingFormatInfo(), "{0:DMS}", value));
            Console.WriteLine("MILS => {0}", string.Format(new MilsFormatInfo(), "{0:DMS}", value));
            Console.WriteLine("Compass => {0}", string.Format(new CompassPointFormatInfo(), "{0:DMS}", value));
        }


        private static void Test2(double value)
        {
            Console.Clear();
            Console.WriteLine("D => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:2D}", value));
            Console.WriteLine("DM => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:2DM}", value));
            Console.WriteLine("DMS => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:2DMS}", value));
            Console.WriteLine("Latitude => {0}", string.Format(new LatitudeFormatInfo(), "{0:2DMS}", value));
            Console.WriteLine("Longitude => {0}", string.Format(new LongitudeFormatInfo(), "{0:2DMS}", value));
            Console.WriteLine("Bearing => {0}", string.Format(new BearingFormatInfo(), "{0:2DMS}", value));
            Console.WriteLine("MILS => {0}", string.Format(new MilsFormatInfo(), "{0:2DMS}", value));
            Console.WriteLine("Compass => {0}", string.Format(new CompassPointFormatInfo(), "{0:2DMS}", value));
        }

        private static void Test3(double value)
        {
            Console.Clear();
            Console.WriteLine("D => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:D}", value).FromDegreesMinutesSecondsToDouble());
            Console.WriteLine("DM => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DM}", value).FromDegreesMinutesSecondsToDouble());
            Console.WriteLine("DMS => {0}", string.Format(new DegreeMinuteSecondFormatInfo(), "{0:DMS}", value).FromDegreesMinutesSecondsToDouble());
            Console.WriteLine("Latitude => {0}", string.Format(new LatitudeFormatInfo(), "{0:DMS}", value).FromDegreesMinutesSecondsToDouble());
            Console.WriteLine("Longitude => {0}", string.Format(new LongitudeFormatInfo(), "{0:DMS}", value).FromDegreesMinutesSecondsToDouble());
            Console.WriteLine("Bearing => {0}", string.Format(new BearingFormatInfo(), "{0:DMS}", value).FromDegreesMinutesSecondsToDouble());
            Console.WriteLine("MILS => {0}", string.Format(new MilsFormatInfo(), "{0:DMS}", value).FromDegreesMinutesSecondsToDouble());
            Console.WriteLine("Compass => {0}", string.Format(new CompassPointFormatInfo(), "{0:DMS}", value).FromDegreesMinutesSecondsToDouble());
        }
    }
}