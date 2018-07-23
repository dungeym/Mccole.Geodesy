using DevStreet.Geodesy.Calculator;
using DevStreet.Geodesy.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevStreet.Geodesy.Simplification
{
    /// <summary>
    /// Wrapper object used to retain the original sequence of ICoordinate objects.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Index} {Coordinate}")]
    internal class SequentialCoordinate : ICoordinate
    {
        private readonly ICoordinate _coordinate;

        /// <summary>
        /// Wrapper object used to retain the original sequence of ICoordinate objects.
        /// </summary>
        /// <param name="index">The index position of the ICoordinate from the source.</param>
        /// <param name="coordinate"></param>
        public SequentialCoordinate(int index, ICoordinate coordinate)
        {
            this.Index = index;
            _coordinate = coordinate;
        }

        /// <summary>
        /// The original ICoordinate object.
        /// </summary>
        public ICoordinate Coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        /// <summary>
        /// The index position of the ICoordinate from the source.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The latitude from the original ICoordinate object.
        /// </summary>

        public double Latitude
        {
            get
            {
                return _coordinate.Latitude;
            }

            set
            {
                _coordinate.Latitude = value;
            }
        }

        /// <summary>
        /// The longitude from the original ICoordinate object.
        /// </summary>

        public double Longitude
        {
            get
            {
                return _coordinate.Longitude;
            }

            set
            {
                _coordinate.Longitude = value;
            }
        }
    }
}