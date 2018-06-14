namespace DevStreet.Geodesy
{
    /// <summary>
    /// Enumeration of compass precision values.
    /// </summary>
    public enum CompassPointPrecision
    {
        /// <summary>
        /// Cardinal directions are the four main points of a compass.
        /// <para>North, East, West and South which are also known by the first letters: N, E, W and S.</para>
        /// </summary>
        Cardinal = 1,

        /// <summary>
        /// Inter-cardinal directions refer to the direction found at the mid-point between each cardinal direction.
        /// <para>Inter-cardinal directions are: North-East (NE), South-East (SE), South-West (SW), and North-West (NW).</para>
        /// </summary>
        Intercardinal = 2,

        /// <summary>
        /// Secondary Inter-cardinal directions refer to the direction found at the mid-point between each cardinal and inter-cardinal direction.
        /// </summary>
        SecondaryIntercardinal = 3,
    }
}