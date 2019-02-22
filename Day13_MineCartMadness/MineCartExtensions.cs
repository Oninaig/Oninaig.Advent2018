using Day13_MineCartMadness.Navigation;

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

        public static bool IsTopLeftCurve(this char c)
        {
            if (c == '/')
                return true;
            return false;
        }

        public static bool IsIntersection(this char c)
        {
            if (c == '+')
                return true;
            return false;
        }

        public static bool IsHorizontalRail(this char c)
        {
            return c == '-';
        }

        public static bool IsVerticalRail(this char c)
        {
            return c == '|';
        }

        public static bool IsOppositeCurve(this char c, char c2)
        {
            if (c == '\\')
                return c2 == '/';
            if (c == '/')
                return c2 == '\\';
            return false;
        }

        /// <summary>
        ///     Combines IsVerticalRail, IsCart, and IsIntersection (is c a vertical rail, a cart, or an intersection?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsVertCartInter(this char c)
        {
            return c.IsVerticalRail() || c.IsCart() || c.IsIntersection();
        }

        /// <summary>
        ///     Combines IsHorizontalRail, IsCart, and IsIntersection (is c a vertical rail, a cart, or an intersection?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsHoriCartInter(this char c)
        {
            return c.IsHorizontalRail() || c.IsCart() || c.IsIntersection();
        }

        public static bool IsTopLeftCurve(this char c, char c1, char c2)
        {
            if (c.IsTopLeftCurve() && (c1.IsVertCartInter() || c1.IsOppositeCurve(c)) &&
                (c2.IsHoriCartInter() || c2.IsOppositeCurve(c)))
                return true;
            return false;
        }

        public static CartDirection GetCartDirection(this char c)
        {
            return (CartDirection) c;
        }
    }
}