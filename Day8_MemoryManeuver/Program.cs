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
            NodeHelper.InitRepeatingAlphabet();
            for (int i = 0; i < 52; i++)
            {
                Console.WriteLine(NodeHelper.GetNextChar());
            }

            Console.ReadLine();
        }
    }
}
