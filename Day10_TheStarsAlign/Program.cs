using System;

namespace Day10_TheStarsAlign
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var test = new Stars("puzzle.txt");
            test.DumpStarSystem();
            test.TimeStep();
            Console.ReadLine();
        }
    }
}