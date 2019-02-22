using System;

namespace Day13_MineCartMadness
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var grid = new TrackGrid("puzzle.txt");
            grid.StartMoving();
            Console.ReadLine();
        }
    }
}