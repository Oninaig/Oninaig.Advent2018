namespace Day6_ChronalCoordinates.Data
{
    public struct CoordinateData
    {
        public object Data { get; set; }
        public Coordinate Coords { get; set; }
        public int DistanceToAllOthers { get; set; }

        public CoordinateData(object data, Coordinate coords, int distanceToAllOthers)
        {
            Data = data;
            Coords = coords;
            DistanceToAllOthers = distanceToAllOthers;
        }
    }
}