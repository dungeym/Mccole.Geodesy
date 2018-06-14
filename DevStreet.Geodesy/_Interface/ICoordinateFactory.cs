using System;

namespace DevStreet.Geodesy
{
    /// <summary>
    /// Provides functions to create a custom instance of ICoordinate.
    /// </summary>
    public interface ICoordinateFactory
    {
        /// <summary>
        /// Create an instance of ICoordinate.
        /// </summary>
        /// <returns></returns>
        ICoordinate Create();

        /// <summary>
        /// Override the default method for creating an instance of ICoordinate.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        bool SetFactoryMethod(Func<ICoordinate> factory);
    }
}