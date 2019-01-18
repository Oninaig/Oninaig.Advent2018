using System;

namespace Day11_ChronalCharge
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var testGrid = new FuelCellGrid(18);
            //var testResult = testGrid.LargestClusterPower().TotalPower;
            //var testResultCoords = testGrid.LargestClusterPower().Cluster[0].Coordinates;

            //var testGrid2 = new FuelCellGrid(42);
            //var testResult2 = testGrid2.LargestClusterPower().TotalPower;
            //var testResultCoords2 = testGrid2.LargestClusterPower().Cluster[0].Coordinates;


            var puzzleGrid = new FuelCellGrid(5719, true);
            //var puzzleResult = puzzleGrid.LargestClusterPower().TotalPower;
            //var puzzleResultCoords = puzzleGrid.LargestClusterPower().Cluster[0].Coordinates;
            //Console.WriteLine($"{puzzleResultCoords.X}, {puzzleResultCoords.Y}");
            Console.ReadLine();
        }
    }
}