using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness.Rails
{
    public class Rail
    {
        public char RailType { get; private set; }
        public Track OwnerTrack { get; private set; }
        public Coord Coordinates { get; private set; }
        public Rail(Track owner, char railType, Coord coordinates)
        {
            this.OwnerTrack = owner;
            this.RailType = railType;
            this.Coordinates = coordinates;
        }

        public override bool Equals(object obj)
        {
            var otherRail = (Rail) obj;
            return otherRail.Coordinates.X == this.Coordinates.X && otherRail.Coordinates.Y == this.Coordinates.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Curve : Rail
    {
        public int CurveMarker { get; private set; }
        public Curve(Track owner, char railType, Coord coordinates, int marker) : base(owner, railType, coordinates)
        {
            this.CurveMarker = marker;
        }
    }
}