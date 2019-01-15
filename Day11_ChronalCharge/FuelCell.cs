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

    public class FuelCellGrid
    {
        public FuelCell[,] Grid { get; private set; }
        public int GridSerialNumber { get; private set; }

        private List<FuelCellCluster> _fuelCellClusters;

        public FuelCellGrid(int serialNumber)
        {
            this.GridSerialNumber = serialNumber;
            this.Grid = new FuelCell[300, 300];
            _fuelCellClusters = new List<FuelCellCluster>();
            initGrid();
        }

        public FuelCellCluster LargestClusterPower()
        {
            return _fuelCellClusters.First();
        }

        private void initGrid()
        {
            var length = Grid.GetLength(0);
            var width = Grid.GetLength(1);
            //todo: better off letting each point be the top-left corner of its own 3x3 instead of trying to preinit a bunch of 1x3 that are gradually filled into 2x3 and then 3x3.

            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                for (int x = 0; x < Grid.GetLength(0); x++)
                {

                }
            }

        }

        
    }

    public class FuelCellCluster
    {
        public List<FuelCell> Cluster { get; private set; }
        public bool IsFull { get; private set; }
        public int TotalPower{get; private set; }
        public FuelCellCluster()
        {
            this.Cluster = new List<FuelCell>(9);
        }

        public bool AddFuelCell(IEnumerable<FuelCell> cells)
        {
            if (IsFull)
            {
                setTotalPower();
                return true;
            }
            foreach (var cell in cells)
                Cluster.Add(cell);
            if (Cluster.Count > 9)
                throw new ArgumentException();
            if (Cluster.Count % 9 == 0)
            {
                IsFull = true;
                setTotalPower();
                return true;
            }
            return false;
        }

        private void setTotalPower()
        {
            var totalPower = 0;
            foreach (var cell in Cluster)
            {
                totalPower += cell.PowerLevel;
            }

            TotalPower = totalPower;
        }
        public bool AddFuelCell(FuelCell cell)
        {
            if (IsFull)
                return true;
            Cluster.Add(cell);
            if (Cluster.Count % 9 == 0)
            {
                IsFull = true;
                return true;
            }
            return false;
        }
    }
}
