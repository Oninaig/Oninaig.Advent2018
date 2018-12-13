using System;

namespace Day6_ChronalCoordinates.Data
{
    //Structs are faster than concrete classes
    public struct Coordinate
    {
        public int x, y;

        public Coordinate(int p1, int p2)
        {
            x = p1;
            y = p2;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }

    public static class CoordinateExtensions
    {
        public static int ManhanttanDistance(this Coordinate c1, Coordinate c2)
        {
            //Manhattan distance was best described to me at You Made a Bad Choice (UMBC) as:
            //Take the sum of the absolute values of the differences of the coordinates.
            //For example, if x=(a,b) and y=(c,d), the Manhattan distance between x and y is
            //|a−c|+|b−d|.

            var absAc = Math.Abs(c1.x - c2.x);
            var absBd = Math.Abs(c1.y - c2.y);
            var manDist = absAc + absBd;
            return manDist;
        }
    }
}
