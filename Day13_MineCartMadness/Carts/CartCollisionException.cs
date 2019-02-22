using System;

namespace Day13_MineCartMadness.Carts
{
    public class CartCollisionException : Exception
    {
        public CartCollisionException()
        {
        }

        public CartCollisionException(string message, Cart cart1, Cart cart2) : base(message)
        {
            Cart1 = cart1;
            Cart2 = cart2;
        }

        public CartCollisionException(string message, Exception inner, Cart cart1, Cart cart2) : base(message, inner)
        {
            Cart1 = cart1;
            Cart2 = cart2;
        }

        public Cart Cart1 { get; set; }
        public Cart Cart2 { get; set; }
    }
}