using System;

namespace Day11_ChronalCharge
{
    public class FuelCell
    {
        public FuelCell(Point coordinates, int gridSerialNumber)
        {
            Coordinates = coordinates;
            GridSerialNumber = gridSerialNumber;
            initPowerLevel();
        }

        public int RackId { get; private set; }
        public int PowerLevel { get; private set; }
        public Point Coordinates { get; set; }
        public int MaxSizeIfTopLeft { get; set; }
        public int GridSerialNumber { get; }

        private void initPowerLevel()
        {
            RackId = Coordinates.X + 10;
            var powerLevel = RackId * Coordinates.Y;
            powerLevel += GridSerialNumber;
            powerLevel *= RackId;
            powerLevel = Math.Abs(powerLevel / 100 % 10);
            powerLevel = powerLevel < 10 ? powerLevel : 0;
            powerLevel -= 5;
            PowerLevel = powerLevel;
        }
    }
}