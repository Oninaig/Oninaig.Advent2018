using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Day13_MineCartMadness
{
    public static class MineCartExtensions
    {
        public static bool IsCart(this char c)
        {
            if (c == '<' || c == '>' || c == '^' || c == 'v')
                return true;
            return false;
        }

        public static bool IsCurve(this char c)
        {
            if (c == '/' || c == '\\')
                return true;
            return false;
        }
    }

    public class Conductor
    {
        public TrackGrid Grid { get; private set; }

        public Conductor(string inputPath)
        {
            this.Grid = new TrackGrid(inputPath);
        }

        
    }

    public enum CartDirection
    {
        Up = '^',
        Down = 'v',
        Left = '<',
        Right = '>',
        Error = '#'
    }

    public class TrackGrid
    {
        public LinkedList<TrackRow> LinkedGrid { get; private set; }
        public List<Cart> AllCarts { get; private set; }
        public TrackGrid(string inputPath)
        {
            initGrid(inputPath);
        }
      
        private void initGrid(string path)
        {
            var input = File.ReadAllLines(path);
            this.LinkedGrid = new LinkedList<TrackRow>();
            this.AllCarts = new List<Cart>();
            foreach (var line in input)
            {
                var row = new TrackRow();
                for (int i = 0; i < line.Length; i++)
                {
                    var c = line[i];
                    row.AddToTrack(c);
                    if (c.IsCart())
                    {
                        var newCart = new Cart(c);
                        newCart.SetTrackRow(row, i);
                        row.AddCartToTrack(newCart);
                        AllCarts.Add(newCart);
                    }
                }

                if (!LinkedGrid.Any())
                    LinkedGrid.AddFirst(row);
                else
                    LinkedGrid.AddLast(row);
            }
        }
        public void Tick()
        {
            var head = LinkedGrid.First;
            while (head != null)
            {
                var track = head.Value;
                
                //Move all the carts on the track (top to bottom, left to right)
                var cartsToMove = track.CartsOnTrackRow;
                foreach (var cart in cartsToMove)
                {
                    var moveResult = track.MoveCart(cart);
                    if(moveResult.HasErrors)
                        throw new InvalidOperationException("Cart was unable to be moved. Does it have a valid direction?");
                    if(!moveResult.Successful)
                }
            }
        }
    }

    public class CartMovementResult
    {
        public bool Successful { get; private set; }
        public Cart Cart { get; private set; }
        public bool HasErrors { get; private set; }

        public CartMovementResult(bool successful, Cart cart, bool hasErrors = false)
        {
            Successful = successful;
            Cart = cart;
            HasErrors = hasErrors;
        }
    }

    public class Cart
    {
        public TrackRow OnTrackRow { get; private set; }
        public Guid Id { get; private set; }
        public int PositionIndex { get; private set; }
        public CartDirection CurrentDirection { get; private set; }
        public Cart(char direction)
        {
            this.CurrentDirection = (CartDirection) direction;
            this.Id = Guid.NewGuid();
        }

        public void SetTrackRow(TrackRow track, int position)
        {
            this.OnTrackRow = track;
            this.PositionIndex = position;
        }

        public void SwitchDirection(CartDirection direction)
        {
            this.CurrentDirection = direction;
        }

        public void MoveTo(int index)
        {
            this.PositionIndex = index;
        }
    }

    public class TrackRow
    {
        public List<char> TracksAndCarts { get; private set; }
        public List<Cart> CartsOnTrackRow { get; private set; }
        public Guid TrackRowId { get; private set; }
        public TrackRow()
        {
            this.TracksAndCarts = new List<char>();
            this.CartsOnTrackRow = new List<Cart>();
            this.TrackRowId = Guid.NewGuid();
        }

        public void AddToTrack(char newRail)
        {
            TracksAndCarts.Add(newRail);
        }

        public void AddCartToTrack(Cart cart)
        {
            this.CartsOnTrackRow.Add(cart);
        }

        public CartMovementResult MoveCart(Cart cart)
        {
            //first lets check to see if the cart is currently on a curve, meaning it will need to be  moved to another row.
            var newTrack = '#';
            switch (cart.CurrentDirection)
            {
                case CartDirection.Up:
                case CartDirection.Down:
                    return new CartMovementResult(false, cart);
                case CartDirection.Left:
                    var leftPosition = cart.PositionIndex - 1;
                    var leftMovingTo = TracksAndCarts[leftPosition];
                    if (leftMovingTo.IsCurve())
                    {
                        switch (leftMovingTo)
                        {
                            case '\\':
                                cart.SwitchDirection(CartDirection.Up);
                                break;
                            case '/':
                                cart.SwitchDirection(CartDirection.Down);
                                break;
                        }
                    }

                    cart.MoveTo(leftPosition);
                    return new CartMovementResult(true, cart);
                case CartDirection.Right:
                    var rightPosition = cart.PositionIndex + 1;
                    var rightMovingTo = TracksAndCarts[rightPosition];
                    if (rightMovingTo.IsCurve())
                    {
                        switch (rightMovingTo)
                        {
                            case '\\':
                                cart.SwitchDirection(CartDirection.Down);
                                break;
                            case '/':
                                cart.SwitchDirection(CartDirection.Up);
                                break;
                        }
                    }

                    cart.MoveTo(rightPosition);
                    return new CartMovementResult(true, cart);

            }
            return new CartMovementResult(false, cart, true);
        }
        
    }
}
