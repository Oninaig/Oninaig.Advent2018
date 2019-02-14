using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness.Carts
{
    public class Cart
    {
        public Coord Coordinates { get; set; }
        public Track OnTrack { get; set; }
        public CartDirection CurrentDirection { get; set; }
        public Cart(Coord coordinates, Track owner)
        {
            this.Coordinates = coordinates;
            this.OnTrack = owner;
        }
    }
}