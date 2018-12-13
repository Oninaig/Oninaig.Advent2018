using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6_ChronalCoordinates
{
    public struct MasterGrid
    {
        public int MaxX, MaxY, MinX, MinY;
        public Coordinate TopRight, BottomLeft;
        public MasterGrid(int maxX, int maxY, int minX, int minY)
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
