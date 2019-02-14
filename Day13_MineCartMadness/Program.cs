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
            //var grid = new TrackGrid("sampleinput.txt");
            //while (true)
            //{
            //    grid.Tick();
            //}

            var grid = new TrackGrid("sampleinput.txt");
            foreach (var c in grid.Tracks)
            {
                foreach(var ca in c.CartsOnTrack)
                    ca.Move();
            }
            Console.ReadLine();
        }
    }
}
