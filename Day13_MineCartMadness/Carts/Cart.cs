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
        private int currQuadrant;
        private bool needToCheckQuadrant;

        public Cart(Coord coordinates, Track owner)
        {
            Coordinates = coordinates;
            OnTrack = owner;
            CurrentBehavior = CartIntersectionBehavior.Left;
            currQuadrant = -1;
            needToCheckQuadrant = true;
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

        public void ResetMovement()
        {
            Moved = false;
        }

        private int getCurrentQuadrant()
        {
            if (!needToCheckQuadrant)
                return currQuadrant;

            var reverseLook = CurrentRailNode.Previous;
            var closestPreviousCurveMarker = -1;
            if (CurrentRailNode.Value is Curve)
                closestPreviousCurveMarker = ((Curve) CurrentRailNode.Value).CurveMarker;
            else
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
                currQuadrant = closestPreviousCurveMarker;
                needToCheckQuadrant = false;
                return closestPreviousCurveMarker;
            }

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
                        case CartDirection.Down:
                        case CartDirection.Left:
                            return RelativeDirection.Left;
                        case CartDirection.Right:
                            return RelativeDirection.Right;
                    }

                    break;
                case 2:
                    switch (CurrentDirection)
                    {
                        case CartDirection.Left:
                        case CartDirection.Up:
                            return RelativeDirection.Left;
                        case CartDirection.Down:
                            return RelativeDirection.Right;
                    }

                    break;
                case 3:
                    switch (CurrentDirection)
                    {
                        case CartDirection.Up:
                        case CartDirection.Right:
                            return RelativeDirection.Left;
                        case CartDirection.Left:
                            return RelativeDirection.Right;
                    }

                    break;
                case 4:
                    switch (CurrentDirection)
                    {
                        case CartDirection.Right:
                        case CartDirection.Down:
                            return RelativeDirection.Left;
                        case CartDirection.Up:
                            return RelativeDirection.Right;
                    }

                    break;
            }

            return RelativeDirection.Error;
        }

        public CartMoveResult Move(Dictionary<Coord, Intersection> intersectionMap)
        {
            if (Moved)
                return new CartMoveResult(true, false);
            if (CurrentRailNode == null)
                CurrentRailNode = OnTrack.Rails.Find(new Rail(null, '*',
                    new Coord(Coordinates.X, Coordinates.Y)));

            //I just realized we can get our "relative" direction (whether we are going forward or backwards in the linked-list)
            //by checking our direction against what "quadrant" we are in on the track, where a quadrant is a section of track between two curves.
            switch (getCurrentRelativeDirection())
            {
                case RelativeDirection.Left:
                    CurrentRailNode = CurrentRailNode.Previous ?? CurrentRailNode.List.Last;
                    break;
                case RelativeDirection.Right:
                    CurrentRailNode = CurrentRailNode.Next ?? CurrentRailNode.List.First;
                    break;
                case RelativeDirection.Error:
                    throw new InvalidOperationException("");
            }

            Coordinates = CurrentRailNode.Value.Coordinates;
            if (!checkForIntersection(intersectionMap))
                checkForCurves();
            Moved = true;
            return checkForCollision();
        }

        private CartMoveResult checkForCollision()
        {
            try
            {
                var collidingCarts = OnTrack.CartsOnTrack.GroupBy(x => x.Coordinates).Where(g => g.Count() > 1);
                if (collidingCarts.Any())
                {
                    var collidingCartsList = collidingCarts.SelectMany(x => x).ToList();
                    //var collidingCartsList = collidingCarts.Select(y => y.Select(x=>x));
                    var collisionCoord = collidingCartsList[0].Coordinates;
                    if (collisionCoord.X == -1)
                        return new CartMoveResult(true, false);

                    throw new CartCollisionException($"Collision at {collisionCoord.Y} , {collisionCoord.X}",
                        collidingCartsList[0], collidingCartsList[1]);
                }
               
            }
            catch (CartCollisionException cartEx)
            {
                return new CartMoveResult(false, true, cartEx.Cart1, cartEx.Cart2);
            }

            return new CartMoveResult(true, false);
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

                needToCheckQuadrant = true;
            }
        }


        private bool checkForIntersection(Dictionary<Coord, Intersection> intersectionMap)
        {
            if (intersectionMap.ContainsKey(Coordinates))
            {
                var otherTrack = intersectionMap[Coordinates].Owners.First(x => x.TrackId != OnTrack.TrackId);
                var newDirection = getIntersectDirection();
                if (CurrentDirection == newDirection)
                {
                    cycleBehavior();
                    return false;
                }

                OnTrack.CartsOnTrack.Remove(this);
                OnTrack = otherTrack;
                OnTrack.CartsOnTrack.Add(this);
                CurrentRailNode = OnTrack.Rails.Find(new Rail(null, '*',
                    new Coord(Coordinates.X, Coordinates.Y)));
                CurrentDirection = newDirection;
                cycleBehavior();
                needToCheckQuadrant = true;
                return true;
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

    public class CartMoveResult
    {
        public bool Success { get; }
        public bool Deleted { get; }
        public Cart DeletedCartA { get; }
        public Cart DeletedCartB { get; }
        public CartMoveResult(bool success, bool deleted)
        {
            Success = success;
            Deleted = deleted;
        }

        public CartMoveResult(bool success, bool deleted, Cart deletedCartA, Cart deletedCartB)
        {
            Success = success;
            Deleted = deleted;
            DeletedCartA = deletedCartA;
            DeletedCartB = deletedCartB;
        }
    }
}