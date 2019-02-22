using System;
using System.Collections.Generic;
using System.Linq;
using Day13_MineCartMadness.Navigation;
using Day13_MineCartMadness.Rails;
using Day13_MineCartMadness.Tracks;

namespace Day13_MineCartMadness.Carts
{
    public class Cart
    {

        public Cart(Coord coordinates, Track owner)
        {
            Coordinates = coordinates;
            OnTrack = owner;
            CurrentBehavior = CartIntersectionBehavior.Left;
        }

        public Cart(Coord coordinates, Track owner, CartDirection direction) : this(coordinates, owner)
        {
            CurrentDirection = direction;
        }

        public Coord Coordinates { get; set; }
        public Track OnTrack { get; set; }
        public CartDirection CurrentDirection { get; set; }
        public bool IsGoingBackwards { get; set; }
        public CartIntersectionBehavior CurrentBehavior { get; set; }
        public LinkedListNode<Rail> CurrentRailNode { get; set; }
        public bool Moved { get; set; }

        public void WhereAmI()
        {
            Console.WriteLine($"I am at {Coordinates.X}, {Coordinates.Y} going {CurrentDirection}");
        }

        public void ResetMovement()
        {
            Moved = false;
        }

        public void Move(Dictionary<Coord, Intersection> intersectionMap)
        {
            if (Moved)
                return;
            if (CurrentRailNode == null)
                CurrentRailNode = OnTrack.Rails.Find(new Rail(null, '*',
                    new Coord(Coordinates.X, Coordinates.Y)));

            switch (CurrentDirection)
            {
                case CartDirection.Down:
                    CurrentRailNode =
                        OnTrack.Rails.Find(new Rail(null, '*', new Coord(Coordinates.X + 1, Coordinates.Y)));
                    break;
                case CartDirection.Up:
                    CurrentRailNode =
                        OnTrack.Rails.Find(new Rail(null, '*', new Coord(Coordinates.X - 1, Coordinates.Y)));
                    break;
                case CartDirection.Left:
                    CurrentRailNode =
                        OnTrack.Rails.Find(new Rail(null, '*', new Coord(Coordinates.X, Coordinates.Y - 1)));
                    break;
                case CartDirection.Right:
                    CurrentRailNode =
                        OnTrack.Rails.Find(new Rail(null, '*', new Coord(Coordinates.X, Coordinates.Y + 1)));
                    break;
            }

            Coordinates = CurrentRailNode.Value.Coordinates;
            if (!checkForIntersection(intersectionMap))
                checkForCurves();
            Moved = true;
        }

        public void Destroy()
        {
            OnTrack.CartsOnTrack.Remove(this);
            OnTrack = null;
        }

        private void checkForCurves()
        {
            if (CurrentRailNode.Value is Curve)
            {
                switch (CurrentDirection)
                {
                    case CartDirection.Down:
                        if (CurrentRailNode.Value.RailType == '\\')
                            CurrentDirection = CartDirection.Right;
                        else
                            CurrentDirection = CartDirection.Left;
                        break;
                    case CartDirection.Right:
                        if (CurrentRailNode.Value.RailType == '\\')
                            CurrentDirection = CartDirection.Down;
                        else
                            CurrentDirection = CartDirection.Up;
                        break;
                    case CartDirection.Left:
                        if (CurrentRailNode.Value.RailType == '\\')
                            CurrentDirection = CartDirection.Up;
                        else
                            CurrentDirection = CartDirection.Down;
                        break;
                    case CartDirection.Up:
                        if (CurrentRailNode.Value.RailType == '\\')
                            CurrentDirection = CartDirection.Left;
                        else
                            CurrentDirection = CartDirection.Right;
                        break;
                }
            }
        }

        private bool checkForIntersection(Dictionary<Coord, Intersection> intersectionMap)
        {
            if (intersectionMap.ContainsKey(Coordinates))
                try
                {
                    var otherTrack = intersectionMap[Coordinates].Owners
                        .FirstOrDefault(x => x.TrackId != OnTrack.TrackId);
                    var newDirection = getIntersectDirection();
                    if (CurrentDirection == newDirection)
                    {
                        cycleBehavior();
                        return false;
                    }

                    if (otherTrack != null)
                    {
                        OnTrack.CartsOnTrack.Remove(this);
                        OnTrack = otherTrack;
                        OnTrack.CartsOnTrack.Add(this);
                    }

                    CurrentRailNode = OnTrack.Rails.Find(new Rail(null, '*',
                        new Coord(Coordinates.X, Coordinates.Y)));
                    CurrentDirection = newDirection;
                    cycleBehavior();
                    return true;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(
                        "Intersection belongs to single track (it must overlay itself at some point. Returning false");
                    return false;
                }

            return false;
        }

        private CartDirection getIntersectDirection()
        {
            switch (CurrentDirection)
            {
                case CartDirection.Right:
                    switch (CurrentBehavior)
                    {
                        case CartIntersectionBehavior.Left:
                            return CartDirection.Up;
                        case CartIntersectionBehavior.Right:
                            return CartDirection.Down;
                        case CartIntersectionBehavior.Straight:
                            return CurrentDirection;
                    }

                    break;
                case CartDirection.Left:
                    switch (CurrentBehavior)
                    {
                        case CartIntersectionBehavior.Left:
                            return CartDirection.Down;
                        case CartIntersectionBehavior.Right:
                            return CartDirection.Up;
                        case CartIntersectionBehavior.Straight:
                            return CurrentDirection;
                    }

                    break;
                case CartDirection.Up:
                    switch (CurrentBehavior)
                    {
                        case CartIntersectionBehavior.Left:
                            return CartDirection.Left;
                        case CartIntersectionBehavior.Right:
                            return CartDirection.Right;
                        case CartIntersectionBehavior.Straight:
                            return CurrentDirection;
                    }

                    break;
                case CartDirection.Down:
                    switch (CurrentBehavior)
                    {
                        case CartIntersectionBehavior.Left:
                            return CartDirection.Right;
                        case CartIntersectionBehavior.Right:
                            return CartDirection.Left;
                        case CartIntersectionBehavior.Straight:
                            return CurrentDirection;
                    }

                    break;
            }

            return CartDirection.Error;
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
                    CurrentBehavior = CartIntersectionBehavior.Left;
                    break;
            }
        }
    }
}