namespace Day6_ChronalCoordinates.Grid
{
    public struct MasterGridMeta
    {
        public int MaxX, MaxY, MinX, MinY;
        public Coordinate TopRight, BottomLeft;
        public MasterGridMeta(int maxX, int maxY, int minX, int minY)
        {
            MaxX = maxX;
            MaxY = maxY;
            MinX = minX;
            MinY = minY;
            TopRight = new Coordinate(maxX, maxY);
            BottomLeft = new Coordinate(minX, minY);
        }

       
    }
}
