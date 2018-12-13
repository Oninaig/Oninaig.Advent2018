using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6_ChronalCoordinates
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = File.ReadAllLines("puzzleinput.txt");
                        
            var manager= new ChronalCoordinateManager();
            foreach (var line in input)
                manager.AddCoordinate(line.Split(',')[0], line.Split(',')[1]);

            manager.InitMasterGrid();
            Console.WriteLine($"Master Grid Area: {manager.GridMeta.MaxArea()}");
            Console.ReadLine();
        }
    }
}
