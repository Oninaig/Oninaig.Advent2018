using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13_MineCartMadness
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new TrackGrid("sampleinput.txt");
            grid.DumpGrid();
            grid.Tick();
            grid.DumpGrid();
            Console.ReadLine();
        }
    }
}
