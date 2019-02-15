using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
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
        public bool IsGoingBackwards { get; set; }
        public CartIntersectionBehavior CurrentBehavior { get; set; }
        public LinkedListNode<Rail> CurrentRailNode { get; set; }
        private int currQuadrant;
        private bool needToCheckQuadrant;
        public bool Moved { get; set; }

        public Cart(Coord coordinates, Track owner)
        {
            this.Coordinates = coordinates;
            this.OnTrack = owner;
            this.CurrentBehavior = CartIntersectionBehavior.Left;
            this.currQuadrant = -1;
            this.needToCheckQuadrant = true;
        }

        public Cart(Coord coordinates, Track owner, CartDirection direction) : this(coordinates, owner)
        {
            this.CurrentDirection = direction;
        }

        public void ResetMovement()
        {
            this.Moved = false;
        }

        private int getCurrentQuadrant()
        {
            if (!needToCheckQuadrant)
                return currQuadrant;

            var reverseLook = CurrentRailNode.Previous;
            var closestPreviousCurveMarker = -1;
            while (reverseLook != null)
            {
                if (reverseLook.Value is Curve)
                {
                    closestPreviousCurveMarker = ((Curve) reverseLook.Value).CurveMarker;
                    break;
                }

                reverseLook = reverseLook.Previous;
            }

            if (closestPreviousCurveMarker > -1)
            {
                this.currQuadrant = closestPreviousCurveMarker;
                this.needToCheckQuadrant = false;
                return closestPreviousCurveMarker;
            }
            else
                throw new InvalidOperationException();
        }

        private RelativeDirection getCurrentRelativeDirection()
        {
            var currQuad = getCurrentQuadrant();
            switch (currQuad)
            {
                case 1:
                    switch (CurrentDirection)
                    {
                        case CartDirection.Left:
                            return RelativeDirection.Left;
                        case CartDirection.Right:
                            return RelativeDirection.Right;
                    }

                    break;
                case 2:
                    switch (CurrentDirection)
                    {
                        case CartDirection.Up:
                            return RelativeDirection.Left;
                        case CartDirection.Down:
                            return RelativeDirection.Right;
                    }

                    break;
                case 3:
                    switch (CurrentDirection)
                    {
                        case CartDirection.Right:
                            return RelativeDirection.Left;
                        case CartDirection.Left:
                            return RelativeDirection.Right;
                    }

                    break;
                case 4:
                    switch (CurrentDirection)
                    {
                        case CartDirection.Down:
                            return RelativeDirection.Left;
                        case CartDirection.Up:
                            return RelativeDirection.Right;
                    }
                    break;
            }

            return RelativeDirection.Error;
        }
        public void Move(Dictionary<Coord, Intersection> intersectionMap)
        {
            if (this.Moved)
                return;
            if (CurrentRailNode == null)
                CurrentRailNode = OnTrack.Rails.Find(new Rail(null, '*',
                    new Coord(this.Coordinates.X, this.Coordinates.Y)));

            //I just realized we can get our "relative" direction (whether we are going forward or backwards in the linked-list)
            //by checking our direction against what "quadrant" we are in on the track, where a quadrant is a section of track between two curves.
            switch (getCurrentRelativeDirection())
            {
                case RelativeDirection.Left:
                    CurrentRailNode = CurrentRailNode.Previous;
                    break;
                case RelativeDirection.Right:
                    CurrentRailNode = CurrentRailNode.Next;
                    break;
            }

            this.Coordinates = CurrentRailNode.Value.Coordinates;
            if (!checkForIntersection(intersectionMap))
                checkForCurves();
            checkForCollision();
            this.Moved = true;
        }

        private void checkForCollision()
        {
            var collisionCoord = this.OnTrack.CartsOnTrack.GroupBy(x => x.Coordinates).Where(g => g.Count() > 1)
                .Select(y => y.Key).FirstOrDefault();
            if (collisionCoord.X == -1)
                return;
            throw new InvalidOperationException($"Collision at {collisionCoord.X} , {collisionCoord.Y}");
        }

        private void checkForCurves()
        {
            if(CurrentRailNode.Value is Curve)
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


        private bool checkForIntersection(Dictionary<Coord, Intersection> intersectionMap)
        {
            if (intersectionMap.ContainsKey(this.Coordinates))
            {
                var otherTrack = intersectionMap[this.Coordinates].Owners.First(x => x.TrackId != this.OnTrack.TrackId);
                var newDirection = getIntersectDirection();
                if (CurrentDirection == newDirection)
                    return false;
                else
                {
                    this.OnTrack.CartsOnTrack.Remove(this);
                    this.OnTrack = otherTrack;
                    this.OnTrack.CartsOnTrack.Add(this);
                    this.CurrentDirection = newDirection;
                    this.cycleBehavior();
                    return true;
                }
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