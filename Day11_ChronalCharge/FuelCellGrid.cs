using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Day11_ChronalCharge
{
    public class FuelCellGrid
    {
        private List<FuelCellCluster> _fuelCellClusters;

        public FuelCellGrid(int serialNumber, bool partTwo = false)
        {
            GridSerialNumber = serialNumber;
            Grid = new FuelCell[300, 300];
            _fuelCellClusters = new List<FuelCellCluster>();
            if (!partTwo)
                initGrid();
            else
                initGridPartTwo();
        }

        public FuelCell[,] Grid { get; }
        public int GridSerialNumber { get; }

        public FuelCellCluster LargestClusterPower()
        {
            return _fuelCellClusters.First();
        }

        private void initGrid()
        {
            var length = Grid.GetLength(0);
            var width = Grid.GetLength(1);
            var finishedClusters = new List<FuelCellCluster>();
            for (var y = 0; y < Grid.GetLength(1); y++)
            for (var x = 0; x < Grid.GetLength(0); x++)
                if (x + 2 < length && y + 2 < width)
                {
                    var cluster = new FuelCellCluster();
                    var topLeft = new FuelCell(new Point(x + 1, y + 1), GridSerialNumber);
                    cluster.AddFuelCell(topLeft);
                    for (var i = 0; i < 3; i++)
                    for (var j = 0; j < 3; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        cluster.AddFuelCell(new FuelCell(
                            new Point(topLeft.Coordinates.X + j, topLeft.Coordinates.Y + i), GridSerialNumber));
                    }

                    finishedClusters.Add(cluster);
                }

            _fuelCellClusters = finishedClusters.OrderByDescending(x => x.TotalPower).ToList();
        }


        /// <summary>
        ///     we can reuse the majority of the code from our original initGrid method, but now we need to expand each topLeft
        ///     node to its maximum size for each topLeft node we create.
        ///     This is probably gonna be hella slow.
        /// </summary>
        private void initGridPartTwo()
        {
            var length = Grid.GetLength(0);
            var width = Grid.GetLength(1);
            var finishedClusters = new List<FuelCellCluster>();
            var slimClusters = new List<FuelCellClusterSlim>();
            for (var y = 0; y < width; y++)
            for (var x = 0; x < length; x++)
                if (x < length && y < width)
                {
                    var topLeft = new FuelCell(new Point(x + 1, y + 1), GridSerialNumber);
                    topLeft.MaxSizeIfTopLeft = x > y ? length - x : width - y;
                    Grid[x, y] = topLeft;
                }

            var slimClusterBag = new ConcurrentBag<FuelCellClusterSlim>();

            var opt = new ParallelOptions();
            opt.MaxDegreeOfParallelism = 8;
            var count = 0;
            Parallel.For(0, width, opt, y =>
            {
                var rowMaxCluster = new FuelCellClusterSlim(0, new Point(0, 0), -1);
                for (var x = 0; x < length; x++)
                {
                    var currTopleft = Grid[x, y];
                    var maxSize = currTopleft.MaxSizeIfTopLeft;
                    var maxInitialized = false;
                    var localTopLeftMaxCluster = new FuelCellClusterSlim(0, new Point(0, 0), -1);
                    for (var k = 0; k < maxSize; k++)
                    {
                        var power = 0;
                        for (var j = 0; j < k; j++)
                        for (var l = 0; l < k; l++)
                            power += Grid[x + l, y + j].PowerLevel;
                        if (!maxInitialized)
                        {
                            localTopLeftMaxCluster = new FuelCellClusterSlim(k,
                                new Point(currTopleft.Coordinates.X, currTopleft.Coordinates.Y), power);
                            maxInitialized = true;
                        }
                        else if (localTopLeftMaxCluster.PowerLevel < power)
                        {
                            localTopLeftMaxCluster = new FuelCellClusterSlim(k,
                                new Point(currTopleft.Coordinates.X, currTopleft.Coordinates.Y), power);
                        }
                    }

                    if (localTopLeftMaxCluster.PowerLevel > rowMaxCluster.PowerLevel)
                        rowMaxCluster = localTopLeftMaxCluster;
                }

                slimClusterBag.Add(rowMaxCluster);
                Interlocked.Increment(ref count);
                Console.WriteLine(count);
            });


            var slimClusterBagAsList = slimClusterBag.ToList();
            var testMax = slimClusterBagAsList.OrderByDescending(x => x.PowerLevel).First();
            var fcc = new FuelCellCluster(testMax.ClusterSize);
            fcc.TotalPower = testMax.PowerLevel;
            fcc.AddFuelCell(new FuelCell(testMax.TopLeft, GridSerialNumber), false);
            _fuelCellClusters.Add(fcc);
        }
    }


    
}