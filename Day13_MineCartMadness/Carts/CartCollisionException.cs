using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13_MineCartMadness.Carts
{
    public class CartCollisionException : Exception
    {
        public Cart Cart1 { get; set; }
        public Cart Cart2 { get; set; }
        public CartCollisionException() { }

        public CartCollisionException(string message, Cart cart1, Cart cart2) : base(message)
        {
            this.Cart1 = cart1;
            this.Cart2 = cart2;
        }

        public CartCollisionException(string message, Exception inner, Cart cart1, Cart cart2) : base(message, inner)
        {
            this.Cart1 = cart1;
            this.Cart2 = cart2;
        }
    }
}
