using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness.Rails
{
    public class Rail
    {
        public Rail(Track owner, char railType, Coord coordinates)
        {
            OwnerTrack = owner;
            RailType = railType;
            Coordinates = coordinates;
        }

        public char RailType { get; }
        public Track OwnerTrack { get; }
        public Coord Coordinates { get; }

        public override bool Equals(object obj)
        {
            var otherRail = (Rail) obj;
            return otherRail.Coordinates.X == Coordinates.X && otherRail.Coordinates.Y == Coordinates.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Curve : Rail
    {
        public Curve(Track owner, char railType, Coord coordinates, int marker) : base(owner, railType, coordinates)
        {
            CurveMarker = marker;
        }

        public int CurveMarker { get; }
    }
}