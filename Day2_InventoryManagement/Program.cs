using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2_InventoryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            //First things first, get the input
            var inputs = File.ReadAllLines("input.txt");

            var twoCounts = 0;
            var threeCounts = 0;

            // Loop through each ID the file
            foreach (var line in inputs)
            {
                // Set up our dictionaries 
                var charDic = new Dictionary<char, int>();

                // Loop through that ID
                foreach (var chr in line)
                {
                    // Process
                    // Yooooo C# 7 in the house
                    charDic.TryGetValue(chr, out int currentValue);
                    charDic[chr] = currentValue + 1;
                }

                // Which characters appear exactly twice
                var charTwice = charDic.Where(x => x.Value == 2);
                // Which characters appear exactly three times
                var charThree = charDic.Where(x => x.Value == 3);

                // If either of our LINQ statements return anything, it means that we've found at least one character that satisfies the condition of exactly 2 or 3 (multiples only count once)
                if (charTwice.Any())
                    twoCounts++;
                if (charThree.Any())
                    threeCounts++;
            }

            var checkSum = twoCounts * threeCounts;
            Console.WriteLine($"Checksum: {checkSum}");
            Console.ReadLine();
        }
    }
}
