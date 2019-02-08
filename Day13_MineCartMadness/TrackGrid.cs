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
            foreach(var cart in AllCarts)
                cart.ResetMovement();
            var currentRow = LinkedGrid.First;
            while (currentRow != null)
            {
                var track = currentRow.Value;
                
                //Move all the carts on the track (top to bottom, left to right)
                var cartsToMove = track.CartsOnTrackRow;
                var cartsToRemove = new List<Cart>();
                foreach (var cart in cartsToMove)
                {
                    if (cart.HasMovedThisTick)
                        continue;
                    var previousDirect = cart.CurrentDirection;
                    var moveResult = track.MoveCart(cart);
                    if(moveResult.HasErrors)
                        throw new InvalidOperationException("Cart was unable to be moved. Does it have a valid direction?");

                    //Unsuccessful movement means we need to jump track rows
                    if (!moveResult.Successful)
                    {
                        switch (moveResult.Cart.CurrentDirection)
                        {
                            case CartDirection.Down:
                                if(currentRow.Next == null)
                                    throw new InvalidOperationException("No lower track row to jump to");
                                var lowerRow = currentRow.Next.Value;
                                //todo:check for collisions
                                lowerRow.InsertCart(moveResult.Cart, moveResult.Cart.PositionIndex);
                                //track.RemoveCart(moveResult.Cart);
                                cartsToRemove.Add(cart);
                                //replace the old track
                                //if(previousDirect == CartDirection.Left)
                                //    track.InsertRail('/', 0);
                                //else if(previousDirect == CartDirection.Right)
                                break;
                            case CartDirection.Up:
                                if (currentRow.Previous == null)
                                    throw new InvalidOperationException("No lower track row to jump to");
                                var upperRow = currentRow.Previous.Value;
                                upperRow.InsertCart(moveResult.Cart, moveResult.Cart.PositionIndex);
                                //track.RemoveCart(moveResult.Cart);
                                cartsToRemove.Add(cart);
                                break;
                        }
                    }
                }

                foreach (var cart in cartsToRemove)
                    track.RemoveCart(cart);
                currentRow = currentRow.Next;
            }
        }

        public void DumpGrid()
        {
            var builder = new StringBuilder();
            foreach (var row in LinkedGrid)
            {
                foreach (var c in row.TracksAndCarts)
                    builder.Append(c);
                builder.AppendLine();
            }

            Console.WriteLine(builder.ToString());
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
        public bool HasMovedThisTick { get; private set; }
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
            this.HasMovedThisTick = true;
        }

        public void MoveTo(int index)
        {
            this.PositionIndex = index;
            this.HasMovedThisTick = true;
        }

        public void ResetMovement()
        {
            this.HasMovedThisTick = false;
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

        public void InsertCart(Cart cart, int index)
        {
            CartsOnTrackRow.Add(cart);
            switch (TracksAndCarts[index])
            {
                case '\\':
                    if(cart.CurrentDirection == CartDirection.Down)
                        cart.SwitchDirection(CartDirection.Right);
                    else
                        cart.SwitchDirection(CartDirection.Left);
                    break;
                case '/':
                    if(cart.CurrentDirection == CartDirection.Down)
                        cart.SwitchDirection(CartDirection.Left);
                    else
                        cart.SwitchDirection(CartDirection.Right);
                    break;
            }
            TracksAndCarts[index] = (char)cart.CurrentDirection;
        }

        public void InsertRail(char rail, int index)
        {
            this.TracksAndCarts[index] = rail;
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

                    if (cart.PositionIndex == 0 && TracksAndCarts[TracksAndCarts.Count - 1] == '\\')
                        TracksAndCarts[cart.PositionIndex] = '/';
                    else if (cart.PositionIndex == 0 && TracksAndCarts[TracksAndCarts.Count - 1] == '/')
                        TracksAndCarts[cart.PositionIndex] = '\\';
                    else
                        TracksAndCarts[cart.PositionIndex] = '-';
                    cart.MoveTo(leftPosition);
                    TracksAndCarts[leftPosition] = (char) cart.CurrentDirection;
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

                    if (cart.PositionIndex == 0 && TracksAndCarts[TracksAndCarts.Count - 1] == '\\')
                        TracksAndCarts[cart.PositionIndex] = '/';
                    else if (cart.PositionIndex == 0 && TracksAndCarts[TracksAndCarts.Count - 1] == '/')
                        TracksAndCarts[cart.PositionIndex] = '\\';
                    else
                        TracksAndCarts[cart.PositionIndex] = '-';
                    cart.MoveTo(rightPosition);
                    TracksAndCarts[rightPosition] = (char)cart.CurrentDirection;
                    return new CartMovementResult(true, cart);

            }
            return new CartMovementResult(false, cart, true);
        }

        public void RemoveCart(Cart moveResultCart)
        {
            //replace the curve that the cart was on
            if (moveResultCart.CurrentDirection == CartDirection.Down)
            {
                if (moveResultCart.PositionIndex == TracksAndCarts.Count - 1)
                    TracksAndCarts[moveResultCart.PositionIndex] = '\\';
                else
                    TracksAndCarts[moveResultCart.PositionIndex] = '/';
            }
            else if (moveResultCart.CurrentDirection == CartDirection.Up)
            {
                if (moveResultCart.PositionIndex == TracksAndCarts.Count - 1)
                    TracksAndCarts[moveResultCart.PositionIndex] = '/';
                else
                    TracksAndCarts[moveResultCart.PositionIndex] = '\\';
            }

            CartsOnTrackRow.Remove(moveResultCart);
        }
    }
}
