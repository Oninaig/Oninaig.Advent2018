using System.Collections.Generic;
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness.Rails
{
    public class Intersection
    {
        public Intersection(Track owner, Coord coordinates)
        {
            if (Owners == null)
                Owners = new List<Track>();
            Owners.Add(owner);
            Coordinates = coordinates;
        }

        public List<Track> Owners { get; set; }
        public Coord Coordinates { get; set; }
    }
}