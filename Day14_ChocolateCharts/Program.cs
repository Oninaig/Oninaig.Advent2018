using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14_ChocolateCharts
{
    class Program
    {
        static void Main(string[] args)
        {
            //Part1();
            Part2();
        }


        public static void Part1()
        {
            var chart = new ChocolateChart();
            chart.AddRecipe(3);
            chart.AddRecipe(7);
            chart.Dump();
            //var puzzleInput = 9;
            var puzzleInput = 293801;


            var currRecipeCount = chart.Recipes.Count;
            while (currRecipeCount < puzzleInput + 10)
            {
                currRecipeCount += chart.Combine();
                if (currRecipeCount < 100)
                    chart.Dump();

            }

            Console.WriteLine(chart.ScoreAfter(puzzleInput, 10));

            Console.ReadLine();
        }

        public static void Part2()
        {
            var chart = new ChocolateChart();
            chart.AddRecipe(3);
            chart.AddRecipe(7);
            chart.Dump();
            var puzzleInput = "594140";

            var foundResult = false;
            while (!foundResult)
            {
                foundResult = chart.CombinePt2(puzzleInput);
            }
            //var currRecipeCount = chart.Recipes.Count;
            //while (currRecipeCount < puzzleInput + 10)
            //{
            //    currRecipeCount += chart.Combine();
            //    if (currRecipeCount < 100)
            //        chart.Dump();

            //}

            //Console.WriteLine(chart.ScoreAfter(puzzleInput, 10));

            Console.ReadLine();
        }
    }
}
