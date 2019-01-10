using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Day8_MemoryManeuver.Tree;

namespace Day8_MemoryManeuver
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var test2 = test.Skip(6).First();


            //NodeHelper.InitRepeatingAlphabet();
            //for (int i = 0; i < 52; i++)
            //{
            //    Console.WriteLine(NodeHelper.GetNextChar());
            //}
            //var sampleAnswer = NodeHelper.ReadInputPartOne("sample.txt");
            //var puzzleAnswer = NodeHelper.ReadInputPartOne("puzzle.txt");
            var sampleAnswerPartTwo = NodeHelper.ReadInputPartTwo("sample.txt");
            var puzzleAnswerPartTwo = NodeHelper.ReadInputPartTwo("puzzle.txt");

            Console.ReadLine();
        }
    }
}
