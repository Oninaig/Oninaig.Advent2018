using System;

namespace Advent2018.Common.Navigation
{
    public struct Coord
    {
        private int? _x;
        private int? _y;

        public int X
        {
            get => _x ?? -1;
            set => _x = value;
        }

        public int Y
        {
            get => _y ?? -1;
            set => _y = value;
        }

        public Coord(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public double DistanceFrom(Coord otherCoord)
        {
            var result = Math.Sqrt((otherCoord.X - X) * (otherCoord.X - X) + (otherCoord.Y - Y) * (otherCoord.Y - Y));
            return result;
        }

        public override bool Equals(object obj)
        {
            var otherCoord = (Coord) obj;
            return otherCoord.X == X && otherCoord.Y == Y;
        }
    }
}