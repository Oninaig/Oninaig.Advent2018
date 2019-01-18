using System;
using System.Collections.Generic;

namespace Day11_ChronalCharge
{
    public struct FuelCellClusterSlim
    {
        public int ClusterSize { get; set; }
        public Point TopLeft { get; set; }
        public int PowerLevel { get; set; }

        public FuelCellClusterSlim(int size, Point point, int power)
        {
            ClusterSize = size;
            TopLeft = point;
            PowerLevel = power;
        }
    }

    public class FuelCellCluster
    {
        private bool _isFull;

        public FuelCellCluster()
        {
            ClusterSize = 9;
            Cluster = new List<FuelCell>(9);
        }

        public FuelCellCluster(int clusterSquareSize)
        {
            ClusterSize = clusterSquareSize;
            Cluster = new List<FuelCell>(clusterSquareSize);
        }

        public List<FuelCell> Cluster { get; }
        public bool IsFull { get; private set; }

        public int TotalPower { get; set; }

        public int ClusterSize { get; set; }

        public bool AddFuelCell(IEnumerable<FuelCell> cells)
        {
            if (IsFull)
            {
                setTotalPower();
                return true;
            }

            foreach (var cell in cells)
                Cluster.Add(cell);
            if (Cluster.Count > ClusterSize)
                throw new ArgumentException();
            if (Cluster.Count % ClusterSize == 0)
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
            foreach (var cell in Cluster) totalPower += cell.PowerLevel;

            TotalPower = totalPower;
        }

        public bool AddFuelCell(FuelCell cell, bool updatePower = true)
        {
            if (IsFull)
            {
                if (updatePower)
                    setTotalPower();
                return true;
            }

            Cluster.Add(cell);
            if (Cluster.Count > ClusterSize)
                throw new ArgumentException();
            if (Cluster.Count % ClusterSize == 0)
            {
                IsFull = true;
                if (updatePower)
                    setTotalPower();
                return true;
            }

            return false;
        }
    }
}