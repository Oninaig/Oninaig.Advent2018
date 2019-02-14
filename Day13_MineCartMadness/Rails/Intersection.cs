using System.Collections.Generic;
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness.Rails
{
    public class Intersection
    {
        public List<Track> Owners { get; set; }
        public Coord Coordinates { get; set; }
        public Intersection(Track owner, Coord coordinates)
        {
            if(this.Owners == null)
                this.Owners = new List<Track>();
            this.Owners.Add(owner);
            this.Coordinates = coordinates;
        }
    }
}