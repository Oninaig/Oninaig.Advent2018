using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;
namespace Day5_AlchemicalReduction
{
    class Program
    {
        static void Main(string[] args)
        {
            #region TESTS
            
            Console.WriteLine($"{new string('#', 5)} Insert at front tests {new string('#', 5)}", Color.LightSkyBlue);
            var chain = new PolymerChain();
            chain.InsertPolymerAtFront('A');
            var middlePolymer = new Polymer('B');
            chain.InsertPolymerAtFront(middlePolymer);
            chain.InsertPolymerAtFront('C');
            chain.Dump();
            Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);


            Console.WriteLine($"{new string('#', 5)} Delete middle node from first chain test {new string('#', 5)}", Color.LightSkyBlue);
            chain.Delete(middlePolymer);
            chain.Dump();
            chain = null;
            Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);

            Console.WriteLine($"{new string('#', 5)} Insert last tests {new string('#', 5)}", Color.LightSkyBlue);
            //test insert last
            var lastChain = new PolymerChain();
            lastChain.InsertPolymerAtBack('A');
            middlePolymer = new Polymer('B');
            lastChain.InsertPolymerAtBack(middlePolymer);
            lastChain.InsertPolymerAtBack('C');
            lastChain.Dump();
            Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);

            Console.WriteLine($"{new string('#', 5)} Delete middle node from last chain test {new string('#', 5)}", Color.LightSkyBlue);
            lastChain.Delete(middlePolymer);
            lastChain.Dump();
            lastChain = null;
            Console.WriteLine($"{new string('#', 5)} End test {new string('#', 5)}\n\n", Color.LightSkyBlue);

            #endregion
            

            Console.ReadLine();
        }
    }
}
