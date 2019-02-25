using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Day14_ChocolateCharts
{
    public class Elf
    {
        public Guid RecipeId { get; set; }
        public Guid Id { get; set; }
        public int CurrIndex { get; set; }
        public Elf(Guid recipeId)
        {
            RecipeId = recipeId;
            Id = Guid.NewGuid();
            CurrIndex = -1;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Recipe
    {
        public int Score { get; set; }
        public Guid Id { get; set; }
        public Recipe(int score)
        {
            Score = score;
            Id = Guid.NewGuid();
        }
    }

    public class ChocolateChart
    {
        public List<Recipe> Recipes { get; set; }
        public Dictionary<Elf, Recipe> ElfDict { get; set; }
        public List<Elf> AllElves { get; set; }
        public int TotalRecipes { get; set; }
        public ChocolateChart()
        {
            Recipes = new List<Recipe>();
            ElfDict = new Dictionary<Elf, Recipe>();
            AllElves = new List<Elf>();
        }

        public void AddRecipe(int score)
        {
            var rec = new Recipe(score);
            Recipes.Add(rec);
            TotalRecipes++;
            var elf = new Elf(rec.Id);
            elf.CurrIndex = Recipes.Count - 1;
            ElfDict[elf] = rec;
            AllElves.Add(elf);
        }

        public void AddRecipe(Elf elf, int score)
        {
            var rec = new Recipe(score);
            Recipes.Add(rec);
            ElfDict[elf] = rec;
            AllElves.Add(elf);
        }

        public int Combine()
        {
            var runningTotal = 0;
            foreach (var elf in AllElves)
            {
                runningTotal += ElfDict[elf].Score;
            }

            var ret = 0;
            foreach (var i in runningTotal.ToString().AsNumbers())
            {
                Recipes.Add(new Recipe(i));
                ret++;
                TotalRecipes++;
            }

            foreach (var e in AllElves)
            {
                var currRecipe = ElfDict[e];
                var distanceToMove = currRecipe.Score + 1;
                var newRecipeIndex = moveForward(distanceToMove, e);
                e.CurrIndex = newRecipeIndex;
                ElfDict[e] = Recipes[e.CurrIndex];
            }

            return ret;
        }

        private int[] processingRecipes;
        public bool CombinePt2(string input)
        {
            if (processingRecipes == null)
                processingRecipes = new int[input.Length];
            var runningTotal = 0;
            foreach (var elf in AllElves)
            {
                runningTotal += ElfDict[elf].Score;
            }


            for (int i = 0; i < runningTotal.Count(); i++)
            {
                Recipes.Add(new Recipe(runningTotal.DigitAt(i + 1)));
                TotalRecipes++;
            }
            //foreach (var i in runningTotal.ToString().AsNumbers())
            //{
            //    Recipes.Add(new Recipe(i));
            //    ret++;
            //    TotalRecipes++;
            //}

            foreach (var e in AllElves)
            {
                var currRecipe = ElfDict[e];
                var distanceToMove = currRecipe.Score + 1;
                var newRecipeIndex = moveForward(distanceToMove, e);
                e.CurrIndex = newRecipeIndex;
                ElfDict[e] = Recipes[e.CurrIndex];
            }

            
            var counter = input.Length;
            if (counter > TotalRecipes)
                return false;

            
            for (int i = TotalRecipes - (counter+4); i < TotalRecipes; i++)
            {
                if (i < 0)
                    return false;
                for (int j = 0; j < input.Length; j++)
                {
                    if (i + j > TotalRecipes - 1)
                        return false;
                    processingRecipes[j] = Recipes[i + j].Score;
                }

                for (int k = 0; k < input.Length; k++)
                {
                    if (processingRecipes[k] != input[k].CharToInt())
                        return false;
                    //if (processingRecipes[k] != int.Parse(input[k].ToString()))
                    //    return false;
                }
                Console.WriteLine($"{input} found after {i} recipes.");
                return true;
                //if (string.Join("", processingRecipes) == input)
                //{
                //    Console.WriteLine($"{input} found after {i} recipes.");
                //    return true;


                //}
            }
            //for (int i = 0; i < 4; i++)
            //{
            //    var slice = string.Join("", Recipes.Skip(TotalRecipes - input.Length - i).Take(input.Length).Select(x => x.Score));
            //    //var slice = string.Join("",Recipes.AsEnumerable().Reverse().Skip(i).Take(input.Length).Reverse().Select(x=>x.Score));
            //    if (slice.Length < input.Length)
            //        return false;
            //    if (slice == input)
            //    {
            //        Console.WriteLine($"{input} found after {TotalRecipes-input.Length-i} recpies.");
            //        return true;
            //    }
            //}

            return false;
        }


        public void Dump()
        {
            var elf1Index = Recipes.IndexOf(ElfDict[AllElves[0]]);
            var elf2Index = Recipes.IndexOf(ElfDict[AllElves[1]]);

            for (int i = 0; i < Recipes.Count; i++)
            {
                if(i == elf1Index)
                    Console.Write($"({Recipes[i].Score}) ");
                else if(i == elf2Index)
                    Console.Write($"[{Recipes[i].Score}] ");
                else
                    Console.Write($"{Recipes[i].Score} ");
            }

            Console.WriteLine();
        }
        private int moveForward(int numSpaces, Elf elf)
        {
            var counter = numSpaces;
            var index = elf.CurrIndex;

            for (int i = index; i <= TotalRecipes; i++)
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
            //var infiniteRecipes = new InfiniteEnumerable<Recipe>(Recipes).Skip(index);
            //foreach (var r in infiniteRecipes)
            //{
            //    if (counter > 0)
            //    {
            //        counter--;
            //        continue;
            //    }

            //    return r;
            //}

            return -1;
        }

        public long ScoreAfter(int index, int numRecipes)
        {
            long runningTotal = 0;
            foreach (var r in Recipes.Skip(index).Take(numRecipes))
            {
                runningTotal = 10 * runningTotal + r.Score;
            }

            return runningTotal;
        }
    }

    public static class Extensions
    {
        public static IEnumerable<int> AsNumbers(this string input)
        {
            foreach (char c in input)
            {
                yield return int.Parse(c.ToString());
            }
        }

        public static int Count(this int variable)
        {
            return ((variable == 0) ? 1 : ((int)Math.Floor(Math.Log10(Math.Abs(variable))) + 1));
        }

        public static int DigitAt(this int number, int index)
        {
            while (number >= (10 * index))
                number /= 10;

            return number % 10;
        }

        public static int UnsafeParse(string s)
        {
            int value = 0;
            for (var i = 0; i < s.Length; i++)
            {
                value = value * 10 + (s[i] - '0');
            }

            return value;
        }

        public static int CharToInt(this char input)
        {
            int result = -1;
            if (input >= 48 && input <= 57)
                result = input - '0';
            return result;
        }
    }
}
