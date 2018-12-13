using System;
using Day6_ChronalCoordinates.Data;

namespace Day6_ChronalCoordinates.Grid
{
    public struct MasterGridMeta
    {
        public int MaxX, MaxY, MinX, MinY, Length, Width, RelativeOffsetX, RelativeOffsetY;
        public Coordinate TopRight, BottomLeft;
        public MasterGridMeta(int maxX, int maxY, int minX, int minY)
        {
            MaxX = maxX;
            MaxY = maxY;
            MinX = minX;
            MinY = minY;
            TopRight = new Coordinate(maxX, maxY);
            BottomLeft = new Coordinate(minX, minY);
            Length = MaxX - MinX;
            Width = MaxY - MinY;
            RelativeOffsetX = MinX < 0 ? Math.Abs(MinX) : -MinX;
            RelativeOffsetY = MinY < 0 ? Math.Abs(MinY) : -MinY;
        }

       
    }
}
