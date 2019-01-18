using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
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
        public int MaxSizeIfTopLeft{get; set; }
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

        public FuelCellGrid(int serialNumber, bool partTwo = false)
        {
            this.GridSerialNumber = serialNumber;
            this.Grid = new FuelCell[300, 300];
            _fuelCellClusters = new List<FuelCellCluster>();
            if (!partTwo)
                initGrid();
            else
                initGridPartTwo();
        }

        public FuelCellCluster LargestClusterPower()
        {
            return _fuelCellClusters.First();
        }

        private void initGrid()
        {
            var length = Grid.GetLength(0);
            var width = Grid.GetLength(1);
            var finishedClusters = new List<FuelCellCluster>();
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                for (int x = 0; x < Grid.GetLength(0); x++)
                {
                    if (x + 2 < length && y + 2 < width)
                    {
                        var cluster = new FuelCellCluster();
                        var topLeft = new FuelCell(new Point(x + 1, y + 1), GridSerialNumber);
                        cluster.AddFuelCell(topLeft);
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (i == 0 && j == 0)
                                    continue;
                                cluster.AddFuelCell(new FuelCell(
                                    new Point(topLeft.Coordinates.X + j, topLeft.Coordinates.Y + i), GridSerialNumber));
                            }
                        }

                        finishedClusters.Add(cluster);
                    }
                }
            }

            _fuelCellClusters = finishedClusters.OrderByDescending(x => x.TotalPower).ToList();

        }


        /// <summary>
        /// we can reuse the majority of the code from our original initGrid method, but now we need to expand each topLeft node to its maximum size for each topLeft node we create.
        /// This is probably gonna be hella slow.
        /// </summary>
        private void initGridPartTwo()
        {
            var length = Grid.GetLength(0);
            var width = Grid.GetLength(1);
            var finishedClusters = new List<FuelCellCluster>();
            var slimClusters = new List<FuelCellClusterSlim>();
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    if (x < length && y < width)
                    {

                        var topLeft = new FuelCell(new Point(x + 1, y + 1), GridSerialNumber);
                        topLeft.MaxSizeIfTopLeft = x > y ? length - x : width - y;
                        Grid[x, y] = topLeft;
                    }
                }
            }

            ConcurrentBag<FuelCellClusterSlim> slimClusterBag = new ConcurrentBag<FuelCellClusterSlim>();
            //Parallel.For(0, width, y =>
            //{
            //    for (int x = 0; x < length; x++)
            //    {
            //        var currTopleft = Grid[x, y];
            //        var maxSize = currTopleft.MaxSizeIfTopLeft;
            //        for (int k = 0; k < maxSize; k++)
            //        {
            //            var power = 0;
            //            for (int j = 0; j < k+1; j++)
            //            {
            //                for (int l = 0; l < k+1; l++)
            //                {
            //                    if (x == 89 && y == 268 && k == 15)
            //                    {
            //                        Console.WriteLine("Stop here");
            //                    }
            //                    power += Grid[x + l, y + j].PowerLevel;
            //                }
            //            }

            //            //if (currTopleft.Coordinates.X == 90 && currTopleft.Coordinates.Y == 269)
            //            //{
            //            //    Console.WriteLine("Stop hEre");
            //            //}
            //            if (x == 89 && y == 268)
            //            {
            //                Console.WriteLine("stop here");
            //            }
            //            slimClusterBag.Add(new FuelCellClusterSlim(x,
            //                new Point(currTopleft.Coordinates.X, currTopleft.Coordinates.Y), power));
            //        }

            //        //Console.WriteLine($"{x}, {y}: Max: {currTopleft.MaxSizeIfTopLeft} | Power: {power}");
            //    }
            //});

            var opt = new ParallelOptions();
            opt.MaxDegreeOfParallelism = 6;
            Parallel.For(0, width, opt, y =>
            {
                FuelCellClusterSlim rowMaxCluster = new FuelCellClusterSlim(0, new Point(0,0), -1);

                for (int x = 0; x < length; x++)
                {
                    var currTopleft = Grid[x, y];
                    var maxSize = currTopleft.MaxSizeIfTopLeft;
                    var maxInitialized = false;
                    FuelCellClusterSlim localTopLeftMaxCluster = new FuelCellClusterSlim(0, new Point(0,0), -1);
                    for (int k = 0; k < maxSize; k++)
                    {
                        var power = 0;
                        for (int j = 0; j < k; j++)
                        {
                            for (int l = 0; l < k; l++)
                            {
                               

                                power += Grid[x + l, y + j].PowerLevel;
                            }
                        }
                        //if (x == 89 && y == 268 && k == 16)
                        //{
                        //    Console.WriteLine("Stop here");
                        //}
                        //if (currTopleft.Coordinates.X == 90 && currTopleft.Coordinates.Y == 269)
                        //{
                        //    Console.WriteLine("Stop hEre");
                        //}
                        //if (x == 89 && y == 268)
                        //{
                        //    Console.WriteLine("stop here");
                        //}
                        if (!maxInitialized)
                        {
                            localTopLeftMaxCluster = new FuelCellClusterSlim(k, new Point(currTopleft.Coordinates.X, currTopleft.Coordinates.Y), power);
                            maxInitialized = true;
                        }
                        else if (localTopLeftMaxCluster.PowerLevel < power)
                        {
                            localTopLeftMaxCluster = new FuelCellClusterSlim(k, new Point(currTopleft.Coordinates.X, currTopleft.Coordinates.Y), power);
                        }
                        
                        
                        //slimClusterBag.Add(new FuelCellClusterSlim(k,
                        //    new Point(currTopleft.Coordinates.X, currTopleft.Coordinates.Y), power));
                    }

                    if (localTopLeftMaxCluster.PowerLevel > rowMaxCluster.PowerLevel)
                        rowMaxCluster = localTopLeftMaxCluster;
                    //slimClusterBag.Add(localTopLeftMaxCluster);

                    //Console.WriteLine($"{x}, {y}: Max: {currTopleft.MaxSizeIfTopLeft} | Power: {power}");
                }
                slimClusterBag.Add(rowMaxCluster);
                Console.WriteLine($"{slimClusterBag.Count}");
            });

            //for (int y = 267; y < width; y ++)
            //{
            //    for (int x = 88; x < length; x++)
            //    {
            //        var currTopleft = Grid[x, y];
            //        var maxSize = currTopleft.MaxSizeIfTopLeft;
            //        for (int k = 0; k < maxSize; k++)
            //        {
            //            var power = 0;
            //            for (int j = 0; j < k; j++)
            //            {
            //                for (int l = 0; l < k; l++)
            //                {
            //                    if (x == 89 && y == 268 && k == 16)
            //                    {
            //                        Console.WriteLine("Stop here");
            //                    }
            //                    power += Grid[x + l, y + j].PowerLevel;
            //                }
            //            }

            //            //if (currTopleft.Coordinates.X == 90 && currTopleft.Coordinates.Y == 269)
            //            //{
            //            //    Console.WriteLine("Stop hEre");
            //            //}
            //            if (x == 89 && y == 268)
            //            {
            //                Console.WriteLine("stop here");
            //            }
            //            slimClusterBag.Add(new FuelCellClusterSlim(x,
            //                new Point(currTopleft.Coordinates.X, currTopleft.Coordinates.Y), power));
            //        }

            //        //Console.WriteLine($"{x}, {y}: Max: {currTopleft.MaxSizeIfTopLeft} | Power: {power}");
            //    }
            //}
            var slimClusterBagAsList = slimClusterBag.ToList();
            var testMax = slimClusterBagAsList.OrderByDescending(x => x.PowerLevel).First();
            var test = slimClusterBagAsList.Where(x => x.PowerLevel == 113);
            //for (int y = 0; y < width; y++)
            //{
            //    for (int x = 0; x < length; x++)
            //    {
            //        if (x < length && y < width)
            //        {


            //            var currTopleft = Grid[x, y];
            //            var maxPower = 0;
            //            FuelCellClusterSlim[] maxPowerCluster = new FuelCellClusterSlim[1];
            //            for (int k = 0; k < currTopleft.MaxSizeIfTopLeft; k++)
            //            {
            //                var totalPower = currTopleft.PowerLevel;
            //                if (k == 0)
            //                {
            //                    var newClust = new FuelCellClusterSlim(k + 1, currTopleft.Coordinates, totalPower);
            //                    //slimClusters.Add(newClust);
            //                    if (totalPower > maxPower)
            //                    {
            //                        maxPower = totalPower;
            //                        maxPowerCluster[0] = newClust;
            //                    }

            //                    continue;
            //                }

            //                for (int l = 0; l <= k; l++)
            //                {
            //                    for (int j = 0; j <= k; j++)
            //                    {
            //                        if (l == 0 && j == 0)
            //                            continue;
            //                        totalPower += Grid[x + j, y + k].PowerLevel;
            //                    }
            //                }

            //                var newClust2 = new FuelCellClusterSlim(k, currTopleft.Coordinates, totalPower);
            //                //slimClusters.Add(new FuelCellClusterSlim(k, currTopleft.Coordinates, totalPower));
            //                if (totalPower > maxPower)
            //                {
            //                    maxPower = totalPower;
            //                    maxPowerCluster[0] = newClust2;
            //                }

            //            }

            //            Console.WriteLine($"{x}, {y}");
            //        }

            //    }
            //    //_fuelCellClusters = finishedClusters.OrderByDescending(x => x.TotalPower).ToList();
            //}
        }
    }

    
    
    

    public struct FuelCellClusterSlim
    {
        public int ClusterSize { get; set; }
        public Point TopLeft { get; set; }
        public int PowerLevel { get; set; }
        public FuelCellClusterSlim(int size, Point point, int power)
        {
            this.ClusterSize = size;
            this.TopLeft = point;
            this.PowerLevel = power;
        }
    }

    public class FuelCellCluster
    {
        public List<FuelCell> Cluster { get; private set; }

        private bool _isFull;
        public bool IsFull { get; private set; }

        public int TotalPower{get; private set; }

        public int ClusterSize { get; private set; }
        public FuelCellCluster()
        {
            ClusterSize = 9;
            this.Cluster = new List<FuelCell>(9);
        }
        public FuelCellCluster(int clusterSquareSize)
        {
            this.ClusterSize = clusterSquareSize;
            this.Cluster = new List<FuelCell>(clusterSquareSize);
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
            foreach (var cell in Cluster)
            {
                totalPower += cell.PowerLevel;
            }

            TotalPower = totalPower;
        }
        public bool AddFuelCell(FuelCell cell)
        {
            if (IsFull)
            {
                setTotalPower();
                return true;
            }
            Cluster.Add(cell);
            if(Cluster.Count > ClusterSize)
                throw new ArgumentException();
            if (Cluster.Count % ClusterSize == 0)
            {
                IsFull = true;
                setTotalPower();
                return true;
            }
            return false;
        }
    }
}
