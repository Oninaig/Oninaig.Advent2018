using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11_ChronalCharge
{
    class Program
    {
        static void Main(string[] args)
        {
            var testGrid = new FuelCellGrid(18);
            var testResult = testGrid.LargestClusterPower().TotalPower;
            var testResultCoords = testGrid.LargestClusterPower().Cluster[0].Coordinates;

            var testGrid2 = new FuelCellGrid(42);
            var testResult2 = testGrid2.LargestClusterPower().TotalPower;
            var testResultCoords2 = testGrid2.LargestClusterPower().Cluster[0].Coordinates;
            Console.ReadLine();
        }
    }
}
