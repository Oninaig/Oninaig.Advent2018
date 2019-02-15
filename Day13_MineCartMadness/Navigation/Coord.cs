namespace Day13_MineCartMadness.Navigation
{
    public struct Coord
    {
        private int? _x;
        private int? _y;

        public int X
        {
            get => _x ?? -1;
            set => _x = value;
        }

        public int Y
        {
            get => _y ?? -1;
            set => _y = value;
        }

        public Coord(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}