namespace Mccole.Geodesy
{
    /// <summary>
    /// A direction
    /// </summary>
    public static class Direction
    {
        /// <summary>
        /// East
        /// </summary>
        public const string East = "E";

        /// <summary>
        /// EastNorthEast
        /// </summary>
        public const string EastNorthEast = East + North + East;

        /// <summary>
        /// EastSouthEast
        /// </summary>
        public const string EastSouthEast = East + South + East;

        /// <summary>
        /// North
        /// </summary>
        public const string North = "N";

        /// <summary>
        /// NorthEast
        /// </summary>
        public const string NorthEast = North + East;

        /// <summary>
        /// NorthNorthEast
        /// </summary>
        public const string NorthNorthEast = North + North + East;

        /// <summary>
        /// NorthNorthWest
        /// </summary>
        public const string NorthNorthWest = North + North + West;

        /// <summary>
        /// NorthWest
        /// </summary>
        public const string NorthWest = North + West;

        /// <summary>
        /// South
        /// </summary>
        public const string South = "S";

        /// <summary>
        /// SouthEast
        /// </summary>
        public const string SouthEast = South + East;

        /// <summary>
        /// SouthSouthEast
        /// </summary>
        public const string SouthSouthEast = South + South + East;

        /// <summary>
        /// SouthSouthWest
        /// </summary>
        public const string SouthSouthWest = South + South + West;

        /// <summary>
        /// SouthWest
        /// </summary>
        public const string SouthWest = South + West;

        /// <summary>
        /// West
        /// </summary>
        public const string West = "W";

        /// <summary>
        /// WestNorthWest
        /// </summary>
        public const string WestNorthWest = West + North + West;

        /// <summary>
        /// WestSouthWest
        /// </summary>
        public const string WestSouthWest = West + South + West;
    }
}
