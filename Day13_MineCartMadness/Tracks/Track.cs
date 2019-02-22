using System;
using System.Collections.Generic;
using System.Drawing;
using Day13_MineCartMadness.Carts;
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Rails;
using Console = Colorful.Console;

namespace Day13_MineCartMadness.Tracks
{
    public class Track
    {
        private int currCurveMarker;

        public Track(Coord topleft)
        {
            TopLeft = topleft;
            TrackId = Guid.NewGuid();
            Rails = new LinkedList<Rail>();
            CartsOnTrack = new List<Cart>();
            currCurveMarker = 1;
        }

        public Coord TopLeft { get; set; }
        public Guid TrackId { get; }
        public LinkedList<Rail> Rails { get; }
        public bool IsComplete { get; set; }
        public List<Cart> CartsOnTrack { get; }

        public void AddRail(int x, int y, char c, bool isTopLeft = false)
        {
            if (!isTopLeft)
            {
                var newCoord = new Coord(x, y);
                if (newCoord.Equals(TopLeft))
                {
                    Console.WriteLine("New coord is the topleft coord, we are finished", Color.Green);
                    IsComplete = true;
                    return;
                }
            }


            if (c.IsCart())
            {
                CartsOnTrack.Add(new Cart(new Coord(x, y), this, c.GetCartDirection()));
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
        }

        public void AddRail(int nextX, int nextY, char nextRail, Dictionary<Coord, Intersection> intersectionMap)
        {
            if (intersectionMap.ContainsKey(new Coord(nextX, nextY)) &&
                !intersectionMap[new Coord(nextX, nextY)].Owners.Contains(this))
                intersectionMap[new Coord(nextX, nextY)].Owners.Add(this);
            else
                intersectionMap[new Coord(nextX, nextY)] = new Intersection(this, new Coord(nextX, nextY));
            AddRail(nextX, nextY, nextRail);
        }
    }
}