using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Rails;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness.Carts
{
    public class Cart
    {
        public Coord Coordinates { get; set; }
        public Track OnTrack { get; set; }
        public CartDirection CurrentDirection { get; set; }
        public CartIntersectionBehavior CurrentBehavior { get; set; }
        public LinkedListNode<Rail> CurrentRailNode { get; set; }
        public bool Moved { get; set; }

        public Cart(Coord coordinates, Track owner)
        {
            this.Coordinates = coordinates;
            this.OnTrack = owner;
            this.CurrentBehavior = CartIntersectionBehavior.Left;
        }

        public Cart(Coord coordinates, Track owner, CartDirection direction) : this(coordinates, owner)
        {
            this.CurrentDirection = direction;
        }

        public void Move()
        {
            if (CurrentRailNode == null)
                CurrentRailNode = OnTrack.Rails.Find(new Rail(null, '*',
                    new Coord(this.Coordinates.X, this.Coordinates.Y)));
        }

        private void cycleBehavior()
        {
            switch (CurrentBehavior)
            {
                case CartIntersectionBehavior.Left:
                    CurrentBehavior = CartIntersectionBehavior.Straight;
                    break;
                case CartIntersectionBehavior.Straight:
                    CurrentBehavior = CartIntersectionBehavior.Right;
                    break;
                case CartIntersectionBehavior.Right:
                    CurrentBehavior = CartIntersectionBehavior.Left:
                    break;
            }
        }
    }
}