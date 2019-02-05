using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Day12_SubterraneanSustainability
{
    public struct Pot
    {
        public int PotNumber { get; set; }
        public bool ContainsPlant { get; set; }

        public Pot(bool containsPlant, int potNum)
        {
            this.PotNumber = potNum;
            this.ContainsPlant = containsPlant;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pot))
                return false;
            var otherPot = (Pot) obj;
            if (otherPot.ContainsPlant == this.ContainsPlant)
                return true;
            return false;
        }

        public override string ToString()
        {
            //return $"Plant {PotNumber} | HasPlant: {ContainsPlant}";
            return $"{(ContainsPlant ? "#" : ".")}";
        }
    }

    public class PotRow
    {
        public List<Pot> Row { get; set; }

        public PotRow(int rowLength = 0, bool doubleLength = true)
        {
           this.Row = new List<Pot>(rowLength*2);
        }
        public PotRow(PotRow otherRow)
        {
            this.Row = new List<Pot>(otherRow.Row);
        }

        public override bool Equals(object obj)
        {
            var otherRow = obj as PotRow;
            if (otherRow == null)
                return false;
            for (int i = 0; i < 5; i++)
            {
                if (!otherRow.Row[i].Equals(this.Row[i]))
                    return false;
            }

            return true;
        }

        public void Dump(bool includeRowNumbers = false)
        {
            var sb = new StringBuilder();
            var sbPotNum = new StringBuilder();
            var sbFinal = new StringBuilder();
            foreach (var pot in Row)
            {
                
                sbPotNum.Append($"{pot.PotNumber} ");
                if (pot.PotNumber.ToString().Length > 1)
                {
                    sb.Append(new string(' ', pot.PotNumber.ToString().Length -1));
                }
                sb.Append($"{pot.ToString()} ");
            }

            if(includeRowNumbers)
                sbFinal.AppendLine(sbPotNum.ToString());    
            sbFinal.AppendLine(sb.ToString());
            Console.WriteLine(sbFinal.ToString());
        }

        public void AddBuffers(int processingWindowSize)
        {
            var firstPotNum = this.Row[0].PotNumber;
            var lastPotNum = this.Row.Last().PotNumber;
            var needToExpandBeginning = Row.Take(PotCave.ProcessingWindowSize).Any(x => x.ContainsPlant);
            var needToExpandEnd = Row.AsEnumerable().Reverse().Take(PotCave.ProcessingWindowSize)
                .Any(x => x.ContainsPlant);
            for (int i = 0; i < processingWindowSize; i++)
            {
                if (needToExpandBeginning)
                    this.Row.Insert(0, new Pot(false, --firstPotNum));
                if(needToExpandEnd)
                    this.Row.Add(new Pot(false, ++lastPotNum));
            }

            Trim();
        }

        public void Trim()
        {
            var canTrimBeginning = !Row.Take(PotCave.ProcessingWindowSize + 1).Any(x => x.ContainsPlant);
            if (canTrimBeginning)
            {
                var firstPlant = Row.FindIndex(x => x.ContainsPlant);
                if (firstPlant > 0)
                {
                    var newRow = Row.Skip(firstPlant - PotCave.ProcessingWindowSize).ToList();
                    Row = newRow;
                }
            }
        }
    }

    public class PotInstructionSet
    {
        public List<PotInstruction> Instructions { get; set; }

        public PotInstructionSet()
        {
            this.Instructions = new List<PotInstruction>();
        }

        public void AddInstruction(string rawInstruction)
        {
            var note = new PotInstruction(rawInstruction);
            this.Instructions.Add(note);
        }
    }

    public class PotInstruction
    {
        public string Instructions { get; set; }
        public Pot[] InstructionsAsPots { get; set; }
        public char Result { get; private set; }
        public bool CurrentPotHasPlant { get; set; }
        public PotInstruction(string inst)
        {
            this.Instructions = inst;
            this.CurrentPotHasPlant = inst[2] == '#';
            this.Result = inst.Reverse().First();
            this.InstructionsAsPots = new Pot[5];
            for (int i = 0; i < 5; i++)
            {
                this.InstructionsAsPots[i] = new Pot(Instructions[i] == '#', 0);
            }
        }

        public bool Matches(Pot[] otherPots)
        {
            for (int i = 0; i < otherPots.Length; i ++)
            {
                if (!otherPots[i].Equals(InstructionsAsPots[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class PotCave
    {
        public PotInstructionSet PotInstructions { get; set; }
        public PotRow Pots { get; set; }
        public PotCave(PotInstructionSet instructions, PotRow rowOfPots)
        {
            this.PotInstructions = instructions;
            this.Pots = rowOfPots;
            Pots.Dump(true);
        }

        public void ProcessGenerations(int numGenerations)
        {
            var startRowSize = Pots.Row.Count;
            for (int i = 0; i < numGenerations; i++)
            {
                ProcessGeneration();
            }

            var endSize = Pots.Row.Count;
            var totalCount = 0;
            foreach(var pot in Pots.Row)
                if (pot.ContainsPlant)
                    totalCount += pot.PotNumber;
            Console.WriteLine($"Total number: {totalCount}");
            Console.Write($"After {numGenerations} generations, the row of pots grew from {startRowSize} pots to {endSize} pots, a difference of {endSize - startRowSize} pots." +
                          $" This results in an average of {((double)(endSize - startRowSize))/(double)numGenerations} pots added per generation.");
        }

        public static readonly int ProcessingWindowSize = 5;
        private bool needToExpand(List<Pot> potList)
        {
            if((potList.AsEnumerable().Reverse().Take(ProcessingWindowSize).Any(x=>x.ContainsPlant)) || potList.AsEnumerable().Take(ProcessingWindowSize).Any(x=>x.ContainsPlant))
                return true;
            return false;
        }

        public void ProcessGeneration()
        {
            var dumpRowNums = false;
            //add buffers on each end equal to the size of our window
            if (needToExpand(Pots.Row))
            {
                dumpRowNums = true;
                Pots.AddBuffers(ProcessingWindowSize);
            }
            var tempPots = new PotRow(Pots);

            for (int i = 0; i < Pots.Row.Count; i++)
            {

                var processingWindow = Pots.Row.Skip(i).Take(ProcessingWindowSize).ToArray();
                if (processingWindow.Length < 5)
                {
                    if (i + 2 < Pots.Row.Count)
                    {
                        tempPots.Row[i + 2] = new Pot(false, tempPots.Row[i + 2].PotNumber);
                    }
                    continue;
                }
                var firstMatch = PotInstructions.Instructions.FirstOrDefault(x => x.Matches(processingWindow));
                if (firstMatch == null)
                {
                    tempPots.Row[i + 2] = new Pot(false, tempPots.Row[i + 2].PotNumber);
                    continue;
                }

                processingWindow[2].ContainsPlant = firstMatch.Result == '#';
                tempPots.Row[i + 2] = new Pot(processingWindow[2].ContainsPlant, tempPots.Row[i + 2].PotNumber);



            }

            Pots.Row = new List<Pot>(tempPots.Row);
            Pots.Dump(dumpRowNums);
        }
       
    }

    /// <summary>
    /// Sidenote on how to calculate sequential combinations (from math.stackexchange):
    /// Assume that i is the first of the elements in such a choice.
    /// As the rest needs to be neighbors, they have already been fixed as i+1,i+2,...,i+(N−1).
    /// Thus the only choise we can make when choosing a premutation is to choose the smallest part.
    /// ***This may be done in K−(N−1) ways, where K are the number of elements in your set and N are the number of elements you should choose.***
    /// We need to subtract N−1 as the last N−1 elements may not be the first element if we need to choose N elements.
    /// 
    /// </summary>
    public static class PotTools
    {
        public static PotCave InitPotCave(string inputFile)
        {
            var input = File.ReadAllLines(inputFile);
            //Get our initial state
            var initialState = new string(input[0].Skip(15).Select(x=>x).ToArray());
       
            //Build our initial row of pots.
            var potRow = new PotRow(initialState.Length);
            
            //init the negative rows
            var negativeCounter = 0; 
            for (int i = initialState.Length-1; i > 0; i--)
            {
                potRow.Row.Add(new Pot(false, i * -1));
                negativeCounter++;
            }

            //init the positive row

            var positiveCounter = 0;
            for (int i = negativeCounter; i < potRow.Row.Capacity; i++)
            {
                if (positiveCounter >= initialState.Length)
                {
                    potRow.Row.Add(new Pot(false, positiveCounter));

                }
                else
                {
                    var containsPlant = initialState[positiveCounter] == '#';
                    potRow.Row.Add(new Pot(containsPlant, positiveCounter));
                }
                
                positiveCounter++;
            }

            //Get and build our instructions
            var instructions = new PotInstructionSet();
            foreach (var inst in input.Skip(2))
            {
                instructions.AddInstruction(inst);
            }
            potRow.AddBuffers(PotCave.ProcessingWindowSize);
            var potCave = new PotCave(instructions, potRow);
            return potCave;
        }

      
    }
}
