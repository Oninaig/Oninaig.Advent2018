using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
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

        public static bool IsIntersection(this char c)
        {
            if (c == '+')
                return true;
            return false;
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

    public enum CartIntersectionBehavior
    {
        Left,
        Straight,
        Right
    }
}
