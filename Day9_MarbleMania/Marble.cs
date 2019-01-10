namespace Day9_MarbleMania
{
    public class Marble
    {
        public Marble(int value)
        {
            Value = value;
        }

        public bool IsCurrent { get; set; }
        public int Value { get; }
    }
}