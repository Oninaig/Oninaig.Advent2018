using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4_ReposeRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("input.txt");
            var processor = new GuardRecordProcessor(data);
            Console.ReadLine();
        }
    }
}
