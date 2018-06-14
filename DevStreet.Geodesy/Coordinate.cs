using DevStreet.Geodesy.Extension;
using System;

namespace DevStreet.Geodesy
{
    /// <summary>
    /// A pair of numbers that show the exact position of a point on the earth.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay}")]
    public class Coordinate : ICoordinate, IComparable
    {
        /// <summary>
        /// A pair of numbers that show the exact position of a point on the earth.
        /// </summary>
        public Coordinate()
        {
        }

        /// <summary>
        /// A pair of numbers that show the exact position of a point on the earth.
        /// </summary>
        /// <param name="latitude">The angular distance of a place north or south of the earth's equator.</param>
        /// <param name="longitude">The angular distance of a place east or west of the Greenwich meridian.</param>
        public Coordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// A pair of numbers that show the exact position of a point on the earth.
        /// </summary>
        /// <param name="latitude">The angular distance of a place north or south of the earth's equator.</param>
        /// <param name="longitude">The angular distance of a place east or west of the Greenwich meridian.</param>
        public Coordinate(string latitude, string longitude)
        {
            if (string.IsNullOrWhiteSpace(latitude))
            {
                throw new ArgumentNullException(nameof(latitude), "The argument cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(longitude))
            {
                throw new ArgumentNullException(nameof(longitude), "The argument cannot be null or empty.");
            }

            this.Latitude = ConvertToDegrees(latitude);
            this.Longitude = ConvertToDegrees(longitude);
        }

#pragma warning disable S1144

        private string DebuggerDisplay
        {
            get
            {
                return this.ToString();
            }
        }

#pragma warning restore S1144

        /// <summary>
        /// The angular distance of a place north or south of the earth's equator.
        /// <para>Usually expressed in degrees and minutes.</para>
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The angular distance of a place east or west of the Greenwich meridian.
        /// <para>Usually expressed in degrees and minutes.</para>
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Compare the left against the right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int Compare(IComparable left, IComparable right)
        {
            return left.CompareTo(right);
        }

        private static double ConvertToDegrees(string value)
        {
            DegreeMinuteSecond dms;
            if (DegreeMinuteSecond.TryParse(value, out dms))
            {
                return dms.Degrees;
            }
            else
            {
                throw new InvalidCastException(string.Format("Could not convert '{0}' to a DegreeMinuteSecond.", value));
            }
        }

        /// <summary>
        /// Determine if the left Coordinate does not equal the right Coordinate.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determine if the left Coordinate is less than the right Coordinate.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(Coordinate left, Coordinate right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        /// Determine if the left Coordinate is less than or equal to the right Coordinate.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(Coordinate left, Coordinate right)
        {
            return Compare(left, right) <= 0;
        }

        /// <summary>
        /// Determine if the left Coordinate is equal to the right Coordinate.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Coordinate left, Coordinate right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Determine if the left Coordinate is greater than the right Coordinate.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(Coordinate left, Coordinate right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        /// Determine if the left Coordinate is greater than or equal to the right Coordinate.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(Coordinate left, Coordinate right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether
        /// the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            Coordinate coordinate = obj as Coordinate;
            if (coordinate != null)
            {
                int result = this.Latitude.CompareTo(coordinate.Latitude);
                return result == 0 ? this.Longitude.CompareTo(coordinate.Longitude) : result;
            }
            else
            {
                throw new InvalidCastException(string.Format("The type '{0}' could not be converted to a {1}.", obj.GetType().Name, nameof(Coordinate)));
            }
        }

        /// <summary>
        /// Determine if this Coordinate equals the defined object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = obj as Coordinate;
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// Get the hash code for this Coordinate.
        /// </summary>
        /// <returns></returns>

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Latitude.GetHashCode() * 1369) ^ (this.Longitude.GetHashCode() * 37);
            }
        }

        /// <summary>
        /// Get the string representation of this object after converting the Latitude and Longitude to Degrees-Minutes-Seconds.
        /// </summary>
        /// <returns></returns>
        public string ToDegreeMinuteSecond()
        {
            string latitude = new DegreeMinuteSecond(this.Latitude).ToLatitude();
            string longitude = new DegreeMinuteSecond(this.Longitude).ToLongitude();
            return string.Format("{0}, {1}", latitude, longitude);
        }

        /// <summary>
        /// Get the string representation of this object using the Latitude and Longitude numeric values.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}", this.Latitude, this.Longitude);
        }
    }
}
