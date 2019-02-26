using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14_ChocolateCharts
{
    public class ChocolateChart
    {
        private int[] processingRecipes;

        public ChocolateChart()
        {
            Recipes = new List<int>();
            ElfDict = new Dictionary<Elf, int>();
            AllElves = new List<Elf>();
        }

        public List<int> Recipes { get; set; }
        public Dictionary<Elf, int> ElfDict { get; set; }
        public List<Elf> AllElves { get; set; }
        public int TotalRecipes { get; set; }

        public void AddRecipe(int score)
        {
            Recipes.Add(score);
            TotalRecipes++;
            var elf = new Elf(score);
            elf.CurrIndex = Recipes.Count - 1;
            ElfDict[elf] = score;
            AllElves.Add(elf);
        }

        public void AddRecipe(Elf elf, int score)
        {
            Recipes.Add(score);
            ElfDict[elf] = score;
            AllElves.Add(elf);
        }

        public int Combine()
        {
            var runningTotal = 0;
            foreach (var elf in AllElves) runningTotal += ElfDict[elf];

            var ret = 0;
            foreach (var i in runningTotal.ToString().AsNumbers())
            {
                Recipes.Add(i);
                ret++;
                TotalRecipes++;
            }

            foreach (var e in AllElves)
            {
                var currRecipe = ElfDict[e];
                var distanceToMove = currRecipe + 1;
                var newRecipeIndex = moveForward(distanceToMove, e);
                e.CurrIndex = newRecipeIndex;
                ElfDict[e] = Recipes[e.CurrIndex];
            }

            return ret;
        }

        public bool CombinePt2(string input)
        {
            if (processingRecipes == null)
                processingRecipes = new int[input.Length];
            var runningTotal = 0;
            foreach (var elf in AllElves) runningTotal += ElfDict[elf];


            for (var i = 0; i < runningTotal.Count(); i++)
            {
                Recipes.Add(runningTotal.DigitAt(i + 1));
                TotalRecipes++;
            }


            foreach (var e in AllElves)
            {
                var currRecipe = ElfDict[e];
                var distanceToMove = currRecipe + 1;
                var newRecipeIndex = moveForward(distanceToMove, e);
                e.CurrIndex = newRecipeIndex;
                ElfDict[e] = Recipes[e.CurrIndex];
            }


            var counter = input.Length;
            if (counter > TotalRecipes)
                return false;

            for (var i = TotalRecipes - (counter + 4); i < TotalRecipes; i++)
            {
                var result = true;
                if (i < 0)
                    return false;
                for (var j = 0; j < input.Length; j++)
                {
                    if (i + j > TotalRecipes - 1)
                        return false;
                    processingRecipes[j] = Recipes[i + j];
                }

                for (var k = 0; k < input.Length; k++)
                    if (processingRecipes[k] != input[k].CharToInt())
                    {
                        result = false;
                        break;
                    }

                if (result)
                {
                    Console.WriteLine(
                        $"{input} found after {i} recipes. | {string.Join("", Recipes.Skip(i).Take(counter).Select(x => x))}");
                    return result;
                }
            }

            return false;
        }


        public void Dump()
        {
            var elf1Index = Recipes.IndexOf(ElfDict[AllElves[0]]);
            var elf2Index = Recipes.IndexOf(ElfDict[AllElves[1]]);

            for (var i = 0; i < Recipes.Count; i++)
                if (i == elf1Index)
                    Console.Write($"({Recipes[i]}) ");
                else if (i == elf2Index)
                    Console.Write($"[{Recipes[i]}] ");
                else
                    Console.Write($"{Recipes[i]} ");

            Console.WriteLine();
        }

        private int moveForward(int numSpaces, Elf elf)
        {
            var counter = numSpaces;
            var index = elf.CurrIndex;

            for (var i = index; i <= TotalRecipes; i++)
            {
                if (i == TotalRecipes)
                    i = 0;
                if (counter > 0)
                {
                    counter--;
                    continue;
                }

                return i;
            }

            return -1;
        }

        public long ScoreAfter(int index, int numRecipes)
        {
            long runningTotal = 0;
            foreach (var r in Recipes.Skip(index).Take(numRecipes)) runningTotal = 10 * runningTotal + r;

            return runningTotal;
        }
    }
}