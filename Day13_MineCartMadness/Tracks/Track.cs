using System;
using System.Collections.Generic;
using Day13_MineCartMadness.Carts;
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Rails;

namespace Day13_MineCartMadness.Tracks
{
    public class Track
    {
        public Coord TopLeft { get; set; }
        public Guid TrackId { get; private set; }
        public LinkedList<Rail> Rails { get; private set; }
        public bool IsComplete { get; set; }
        public List<Cart> CartsOnTrack { get; private set; }
        private int currCurveMarker;
        public Track(Coord topleft)
        {
            this.TopLeft = topleft;
            this.TrackId = Guid.NewGuid();
            this.Rails = new LinkedList<Rail>();
            this.CartsOnTrack = new List<Cart>();
            this.currCurveMarker = 1;
        }

        public void AddRail(int x, int y, char c)
        {
            if (c.IsCart())
            {
                CartsOnTrack.Add(new Cart(new Coord(x,y),this, c.GetCartDirection()));
                switch (c.GetCartDirection())
                {
                    case CartDirection.Up:
                    case CartDirection.Down:
                        c = '|';
                        break;
                    case CartDirection.Left:
                    case CartDirection.Right:
                        c = '-';
                        break;
                }
            }

            if (c.IsCurve())
                Rails.AddLast(new Curve(this, c, new Coord(x, y), currCurveMarker++));
            else
                Rails.AddLast(new Rail(this, c, new Coord(x, y)));
            //todo: this will fail to work if tracks are anything but squares/rectangles.
            if (TopLeft.X == x - 1 && TopLeft.Y == y)
                this.IsComplete = true;
        }

        public void AddRail(int nextX, int nextY, char nextRail, Dictionary<Coord, Intersection> intersectionMap)
        {
            if (intersectionMap.ContainsKey(new Coord(nextX, nextY)))
                intersectionMap[new Coord(nextX, nextY)].Owners.Add(this);
            else
                intersectionMap[new Coord(nextX, nextY)] = new Intersection(this, new Coord(nextX, nextY));
            this.AddRail(nextX, nextY, nextRail);
        }
    }
}