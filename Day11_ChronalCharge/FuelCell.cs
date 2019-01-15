using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11_ChronalCharge
{
    public class FuelCell
    {
        public int RackId { get; private set; }
        public int PowerLevel { get; private set; }
        public Point Coordinates { get; set; }

        public int GridSerialNumber { get; private set; }
        public FuelCell(Point coordinates, int gridSerialNumber)
        {
            this.Coordinates = coordinates;
            this.GridSerialNumber = gridSerialNumber;
            initPowerLevel();
        }

        private void initPowerLevel()
        {
            this.RackId = Coordinates.X + 10;
            var powerLevel = RackId * Coordinates.Y;
            powerLevel += GridSerialNumber;
            powerLevel *= RackId;
            powerLevel = Math.Abs(powerLevel / 100 % 10);
            powerLevel = powerLevel < 10 ? powerLevel : 0;
            powerLevel -= 5;
            PowerLevel = powerLevel;
        }

    }

    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
