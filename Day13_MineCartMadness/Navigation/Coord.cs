namespace Day13_MineCartMadness.Navigation
{
    public struct Coord
    {
        int? _x;
        int? _y;
        public int X
        {
            get { return _x ?? -1; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y ?? -1; }
            set { _y = value; }
        }

        public Coord(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}