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
    public struct Pot : IEquatable<Pot>
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
            var otherPot = (Pot)obj;
            if (otherPot.ContainsPlant == this.ContainsPlant)
                return true;
            return false;
        }

        public bool Equals(Pot otherPot)
        {
            if (otherPot.ContainsPlant == this.ContainsPlant)
                return true;
            return false;
        }

        public override string ToString()
        {
            return $"{(ContainsPlant ? "#" : ".")}";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + PotNumber.GetHashCode();
            hash = hash * 23 + ContainsPlant.GetHashCode();
            return hash;
        }

        public int GetSimpleHashCode()
        {
            return this.PotNumber;
        }

    }

    public class PotRow
    {
        public List<Pot> Row { get; set; }
        public Dictionary<int, Pot> PotDict { get; set; }

        public PotRow(int rowLength = 0, bool doubleLength = true)
        {
            this.Row = new List<Pot>(rowLength * 2);
            this.PotDict = new Dictionary<int, Pot>();
        }
        public PotRow(PotRow otherRow)
        {
            this.Row = new List<Pot>(otherRow.Row);
            this.PotDict = new Dictionary<int, Pot>(otherRow.PotDict);
        }

        public void AddPot(Pot pot)
        {
            Row.Add(pot);
            PotDict[pot.GetSimpleHashCode()] = pot;
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
                    sb.Append(new string(' ', pot.PotNumber.ToString().Length - 1));
                }
                sb.Append($"{pot.ToString()} ");
            }

            if (includeRowNumbers)
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
                {
                    var newPot = new Pot(false, --firstPotNum);
                    this.Row.Insert(0, newPot);
                    this.PotDict[newPot.GetHashCode()] = newPot;
                }
                if (needToExpandEnd)
                {
                    var newPot = new Pot(false, ++lastPotNum);
                    AddPot(newPot);
                }
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
                    PotDict.Clear();
                    foreach (var newPot in Row)
                        PotDict[newPot.GetHashCode()] = newPot;
                }
            }
        }

        public void FinishGeneration(Dictionary<int, Pot> processedDict)
        {
            var newPotDict = new Dictionary<int, Pot>();
            foreach (var key in PotDict.Keys)
            {
                var currentPot = PotDict[key];
                Pot processedPot = new Pot(false, int.MinValue);
                if (processedDict.TryGetValue(key, out processedPot))
                {
                    var changedPot = new Pot(true, currentPot.PotNumber);
                    newPotDict[changedPot.GetHashCode()] = changedPot;
                }
                else
                {
                    var changedPot = new Pot(false, currentPot.PotNumber);
                    newPotDict[changedPot.GetHashCode()] = changedPot;
                }
            }
            PotDict.Clear();
            PotDict = new Dictionary<int, Pot>(newPotDict);
            Row.Clear();
            foreach (var pot in PotDict.Values.OrderBy(x => x.PotNumber))
            {
                Row.Add(pot);
            }
        }
    }

    public class PotInstructionSet
    {
        public List<PotInstruction> Instructions { get; set; }
        public Dictionary<int, PotInstruction> InstructionsDict { get; set; }
        public PotInstructionSet()
        {
            this.Instructions = new List<PotInstruction>();
            this.InstructionsDict = new Dictionary<int, PotInstruction>();
        }

        public void AddInstruction(string rawInstruction)
        {
            var note = new PotInstruction(rawInstruction);
            InstructionsDict[note.GetHashCode()] = note;
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

        public override int GetHashCode()
        {
            return InstructionsAsPots.PotRangeHash(true);
        }

        public bool Matches(List<Pot> otherPots, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (!otherPots[i].Equals(InstructionsAsPots[i]))
                {
                    return false;
                }
            }

            return true;
        }
        public bool Matches(Pot[] otherPots)
        {
            if (!otherPots[0].Equals(InstructionsAsPots[0]))
                return false;
            for (int i = 0; i < otherPots.Length; i++)
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

        public double ProcessGenerations(long numGenerations)
        {
            var startRowSize = Pots.Row.Count;
            var timer = new Stopwatch();
            timer.Start();
            for (long i = 0; i < numGenerations; i++)
            {
                ProcessGeneration();
                if (i % 10000 == 0)
                {
                    //Console.WriteLine($"Finished generation {i.ToString("N0")}");
                }
            }
            timer.Stop();
            var endSize = Pots.Row.Count;
            var totalCount = 0;
            foreach (var pot in Pots.Row)
                if (pot.ContainsPlant)
                    totalCount += pot.PotNumber;
            Debug.WriteLine($"Total number: {totalCount}");
            Debug.WriteLine($"After {numGenerations} generations and approx {timer.Elapsed.TotalSeconds.ToString("N0")} seconds ({timer.ElapsedMilliseconds / (1000.00)} s), the row of pots grew from {startRowSize} pots to {endSize} pots, a difference of {endSize - startRowSize} pots." +
                          $" This results in an average of {((double)(endSize - startRowSize)) / (double)numGenerations} pots added per generation.");
            return timer.ElapsedMilliseconds;
        }

        public static readonly int ProcessingWindowSize = 5;
        private bool needToExpand(List<Pot> potList)
        {
            if ((potList.AsEnumerable().Reverse().Take(ProcessingWindowSize).Any(x => x.ContainsPlant)) || potList.AsEnumerable().Take(ProcessingWindowSize).Any(x => x.ContainsPlant))
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

            //new plan. Why dont we just generate a collection of "slices" to process for pots, and then afterwards set the appropriate
            //indexes to "hasPlant" in our main collection?
            Queue<Pot[]> toBeProcessed = new Queue<Pot[]>();
            var max = Pots.Row.Count;
            for (int i = 0; i < max; i++)
            {
                var currProcessingWindow = new Pot[ProcessingWindowSize];
                if (max - i < ProcessingWindowSize)
                {
                    var newSize = max - i;
                    currProcessingWindow = new Pot[newSize];
                    for (int j = 0; j < newSize; j++)
                        currProcessingWindow[j] = Pots.Row[i + j];
                }
                else
                {
                    for (int j = 0; j < ProcessingWindowSize; j++)
                    {
                        currProcessingWindow[j] = Pots.Row[i + j];
                    }
                }
                toBeProcessed.Enqueue(currProcessingWindow);
             
            }

            var processedDict = new Dictionary<int, Pot>();
            while (toBeProcessed.Any())
            {
                var currWindow = toBeProcessed.Dequeue();
                PotInstruction firstMatch = null;
                PotInstructions.InstructionsDict.TryGetValue(currWindow.PotRangeHash(true), out firstMatch);
                if (firstMatch != null)
                {
                    var newPot = new Pot(true, currWindow[2].PotNumber);
                    //use old pot for key
                    processedDict[currWindow[2].GetHashCode()] = newPot;
                }
            }

            Pots.FinishGeneration(processedDict);
            //Pots.Row = new List<Pot>(tempPots.Row);
        }

    }

    /// <summary>
    /// Sidenote on how to calculate sequential combinations (from math.stackexchange):
    /// Assume that i is the first of the elements in such a choice.
    /// As the rest needs to be neighbors, they have already been fixed as i+1,i+2,...,i+(N−1).
    /// Thus the only choise we can make when choosing a premutation is to choose the smallest part.
    /// ***This may be done in K−(N−1) ways, where K are the number of elements in your set and N are the number of elements you should choose.***
    /// We need to subtract N−1 as the last N−1 elements may not be the first element if we need to choose N elements.
    /// </summary>
    public static class PotTools
    {
        public static PotCave InitPotCave(string inputFile)
        {
            var input = File.ReadAllLines(inputFile);
            //Get our initial state
            var initialState = new string(input[0].Skip(15).Select(x => x).ToArray());

            //Build our initial row of pots.
            var potRow = new PotRow(initialState.Length);

            //init the negative rows
            var negativeCounter = 0;
            for (int i = initialState.Length - 1; i > 0; i--)
            {
                potRow.AddPot( new Pot(false, i * -1));
                negativeCounter++;
            }

            //init the positive row
            var positiveCounter = 0;
            for (int i = negativeCounter; i < potRow.Row.Capacity; i++)
            {
                if (positiveCounter >= initialState.Length)
                {
                    potRow.AddPot(new Pot(false, positiveCounter));

                }
                else
                {
                    var containsPlant = initialState[positiveCounter] == '#';
                    potRow.AddPot(new Pot(containsPlant, positiveCounter));
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
