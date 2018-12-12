using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5_AlchemicalReduction
{
    class Program
    {
        static void Main(string[] args)
        {
            var chain = new PolymerChain();
            chain.InsertPolymerAtFront('A');
            var middlePolymer = new Polymer('B');
            chain.InsertPolymerAtFront(middlePolymer);
            chain.InsertPolymerAtFront('C');
            chain.Dump();


            Console.WriteLine("Deleting middle node");
            chain.Delete(middlePolymer);
            chain.Dump();

            Console.ReadLine();
        }
    }
}
