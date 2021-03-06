﻿using System.IO;
using Colorful;

namespace Day5_AlchemicalReduction
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region TESTS

            //Console.WriteLine($"{new string('#', 5)} Insert at front tests {new string('#', 5)}", Color.LightSkyBlue);
            //var chain = new PolymerChain();
            //chain.InsertPolymerAtFront('A');
            //var middlePolymer = new Polymer('B');
            //chain.InsertPolymerAtFront(middlePolymer);
            //chain.InsertPolymerAtFront('C');
            //chain.Dump();
            //Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);


            //Console.WriteLine($"{new string('#', 5)} Delete middle node from first chain test {new string('#', 5)}", Color.LightSkyBlue);
            //chain.Delete(middlePolymer);
            //chain.Dump();
            //chain = null;
            //Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);

            //Console.WriteLine($"{new string('#', 5)} Insert last tests {new string('#', 5)}", Color.LightSkyBlue);
            ////test insert last
            //var lastChain = new PolymerChain();
            //lastChain.InsertPolymerAtBack('A');
            //middlePolymer = new Polymer('B');
            //lastChain.InsertPolymerAtBack(middlePolymer);
            //lastChain.InsertPolymerAtBack('C');
            //lastChain.Dump();
            //Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);

            //Console.WriteLine($"{new string('#', 5)} Delete middle node from last chain test {new string('#', 5)}", Color.LightSkyBlue);
            //lastChain.Delete(middlePolymer);
            //lastChain.Dump();
            //lastChain = null;
            //Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);


            //Console.WriteLine($"{new string('#', 5)} reaction test {new string('#', 5)}", Color.LightSkyBlue);
            //var reactChain = new PolymerChain();
            //reactChain.InsertPolymerAtFront('A');
            //reactChain.InsertPolymerAtFront('B');
            //reactChain.InsertPolymerAtFront('b');
            //reactChain.InsertPolymerAtFront('C');
            //reactChain.InsertPolymerAtFront('A');
            //reactChain.InsertPolymerAtFront('B');
            //reactChain.InsertPolymerAtFront('b');
            //reactChain.InsertPolymerAtFront('C');
            //reactChain.InsertPolymerAtFront('A');
            //reactChain.InsertPolymerAtFront('B');
            //reactChain.InsertPolymerAtFront('b');
            //reactChain.InsertPolymerAtFront('C');
            //reactChain.InsertPolymerAtFront('A');
            //reactChain.InsertPolymerAtFront('a');
            //reactChain.InsertPolymerAtFront('B');
            //reactChain.InsertPolymerAtFront('b');
            //reactChain.InsertPolymerAtFront('C');
            //reactChain.Dump();
            //Console.WriteLine("Starting reaction...", Color.LightSkyBlue);
            //reactChain.StartReaction();
            //reactChain.Dump();
            //Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);

            #endregion


            #region actual puzzle

            //var input = File.ReadAllText("input.txt");
            //var puzzleChain = new PolymerChain();
            //foreach (var poly in input)
            //    puzzleChain.InsertPolymerAtBack(poly);

            //Console.WriteLine($"PolymerCount: {puzzleChain.PolymerCount}");
            //var verbose = false;
            //if (input.Length < 100)
            //    verbose = true;

            //puzzleChain.StartReactionSansRecursion(verbose);
            //Console.WriteLine($"PolymerCount: {puzzleChain.PolymerCount}");
            //if (verbose)
            //    puzzleChain.Dump();

            #endregion

            #region part two
            var input = File.ReadAllText("input.txt");
            var manager = new PolymerChainManager(input);
            manager.FindBestCharToRemove();

            

            #endregion
            Console.ReadLine();
        }
    }
}