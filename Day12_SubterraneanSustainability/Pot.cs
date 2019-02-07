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

        //public override int GetHashCode()
        //{
        //    throw new Exception("dont use this");
        //    int hash = 17;
        //    hash = hash * 23 + PotNumber.GetHashCode();
        //    hash = hash * 23 + ContainsPlant.GetHashCode();
        //    return hash;
        //}

        public int GetNonUniqueHashCode()
        {
            int hash = 17;
            hash = hash * 23 + ContainsPlant.GetHashCode();
            return hash;
        }

    }

    public class PotRow
    {
        public Dictionary<int, Pot> PotDict { get; set; }

        public int UpperBound { get; private set; }
        public int LowerBound { get; private set; }
        public int UpperPotWithPlant{ get; private set; }
        public int LowerPotWithPlant { get; private set; }
        public PotRow()
        {
            this.PotDict = new Dictionary<int, Pot>();
            UpperBound = int.MinValue;
            LowerBound = int.MaxValue;
        }

        public void AddPot(Pot pot)
        {
            if (pot.PotNumber > UpperPotWithPlant && pot.ContainsPlant)
                UpperPotWithPlant = pot.PotNumber;
            if (pot.PotNumber < LowerPotWithPlant && pot.ContainsPlant)
                LowerPotWithPlant = pot.PotNumber;
            //if (pot.PotNumber > UpperBound)
            //    UpperBound = pot.PotNumber;
            //if (pot.PotNumber < LowerBound)
            //    LowerBound = pot.PotNumber;
            PotDict[pot.PotNumber] = pot;
        }

        //private bool needToExpandRight(int potNumber)
        //{
        //    if (UpperBound != int.MaxValue && LowerBound != int.MinValue)
        //        if (potNumber > UpperBound || potNumber < LowerBound)
        //            return true;
        //    return false;
        //}
        private bool needToExpandRight()
        {
            for(int i = UpperBound; i > UpperBound-5; i--)
                if (PotDict[i].ContainsPlant)
                    return true;
            return false;
        }
        private bool needToExpandLeft()
        {
            for (int i = LowerBound; i < LowerBound + 5; i++)
                if (PotDict[i].ContainsPlant)
                    return true;
            return false;
        }
        public void InitRowStats()
        {
            var ordered = PotDict.Keys.OrderBy(x => x).ToArray();
            UpperBound = ordered[ordered.Length-1];
            LowerBound = ordered[0];
        }

        public Tuple<int, int> GetBounds()
        {
            return new Tuple<int, int>(UpperBound, LowerBound);
        }

        public Pot[] GetPotsInclusive(int startPotNumber, int endPotNumber)
        {
            if (endPotNumber > UpperBound)
                endPotNumber = UpperBound;
            var returnSection = new Pot[Math.Abs(endPotNumber - startPotNumber) + 1];
            var potCounter = 0;
            for (int i = startPotNumber;
                endPotNumber < startPotNumber ? i >= endPotNumber : i <= endPotNumber; i += endPotNumber < startPotNumber ? -1 : 1)
            {
                returnSection[potCounter++] = PotDict[i];
            }

            return returnSection;
        }

        public int GetPotNumberSum()
        {
            var sum = 0;
            foreach (var kvp in PotDict.Where(x => x.Value.ContainsPlant == true))
            {
                sum += kvp.Value.PotNumber;
            }

            return sum;
        }

        private Dictionary<int, bool> changedPots;

        public void StartUpdate()
        {
            //this.changedPots = new Dictionary<int, bool>();
            //foreach (var key in PotDict.Keys)
            //    changedPots[key] = false;
            UpperPotWithPlant = int.MinValue;
            LowerPotWithPlant = int.MaxValue;
        }

        public void UpdatePot(int potKey, Pot newPot)
        {
            //if ((potKey == UpperPotWithPlant || potKey == LowerPotWithPlant))
            //{
            //    if (potKey == UpperPotWithPlant)
            //        UpperPotWithPlant = potKey;
            //    if (potKey == LowerPotWithPlant)
            //        LowerPotWithPlant = potKey;
            //}
               

            PotDict[potKey] = newPot;

            if (newPot.ContainsPlant)
            {
                if (potKey > UpperPotWithPlant)
                    UpperPotWithPlant = potKey;
                if (potKey < LowerPotWithPlant)
                    LowerPotWithPlant = potKey;
            }
            //changedPots[potKey] = true;
        }

        private void addBuffers(bool right, bool left, int bufferSize = 5)
        {
            if (right)
            {
                for (int i = UpperBound; i < UpperBound + bufferSize; i++)
                {
                   AddPot(new Pot(false, i));
                }

                UpperBound = UpperBound + bufferSize - 1;
            }

            if (left)
            {
                for (int i = LowerBound; i > LowerBound - bufferSize; i--)
                    AddPot(new Pot(false, i));
                LowerBound = LowerBound - bufferSize + 1;
            }

            trim();
            //InitRowStats();
        }


        private void trim()
        {
            //var ordered = PotDict.OrderBy(x => x.Key).Where(x => x.Value.ContainsPlant).ToList();
            //var firstPot = ordered[0];
            //var lowerLimit = firstPot.Key - 5;
            //var lastPot = ordered.Last();
            //var upperLimit = lastPot.Key + 5;
            for (int i = LowerBound; i <= LowerPotWithPlant - 5; i++)
            {
                PotDict.Remove(i);
                LowerBound++;
            }

            for (int i = UpperBound; i >= UpperPotWithPlant + 5; i--)
            {
                PotDict.Remove(i);
                UpperBound--;
            }
        }
        public void EndUpdate()
        {
            //foreach (var kvp in changedPots.Where(x => x.Value == false))
            //{
            //    var newPot = new Pot(false, PotDict[kvp.Key].PotNumber);
            //    PotDict[kvp.Key] = newPot;
            //    //if (kvp.Key == UpperPotWithPlant)
            //    //    UpperPotWithPlant = int.MinValue;
            //    //if (kvp.Key == LowerPotWithPlant)
            //    //    LowerPotWithPlant = int.MaxValue;
            //}

            //var keys = new StringBuilder();
            //var values = new StringBuilder();
            //foreach (var kvp in PotDict.Keys.OrderBy(x => x))
            //{
            //    keys.Append($"{kvp} ");
            //    values.Append($"{(PotDict[kvp].ContainsPlant ? "#" : ".")}");
            //}

            //Debug.WriteLine(keys.ToString());
            //Debug.WriteLine(values.ToString());

            //Debug.WriteLine("");
            //changedPots.Clear();
            var needRight = needToExpandRight();
            var needLeft = needToExpandLeft();
            addBuffers(needRight, needLeft);
        }
    }
       


    public class PotInstructionSet
    {
        public List<PotRangeInstruction> Instructions { get; private set; }
        public Dictionary<int, PotRangeInstruction> InstructionsDict { get; private set; }


        public PotInstructionSet()
        {
            this.InstructionsDict = new Dictionary<int, PotRangeInstruction>();
        }

        public PotRangeInstruction GetInstruction(int key)
        {
            return InstructionsDict[key];
        }

        public void AddInstruction(string instruction)
        {
            if (Instructions == null)
                Instructions = new List<PotRangeInstruction>();
            var inst = new PotRangeInstruction(instruction);
            Instructions.Add(inst);
            InstructionsDict.Add(inst.InstructionAsPots.SimplePotRangeHash(), inst);
        }

    }

    public class PotRangeInstruction
    {
        public string Instructions { get; private set; }
        public Pot[] InstructionAsPots { get; private set; }
        public bool Result { get; private set; }
        public PotRangeInstruction(string instruction)
        {
            
            var inputState = instruction.Substring(0, instruction.Length - instruction.IndexOf("=>") + 1);
            this.Instructions = inputState;
            this.InstructionAsPots = new Pot[Instructions.Length];
            for (int i = 0; i < inputState.Length; i++)
                this.InstructionAsPots[i] = new Pot(instruction[i] == '#', int.MinValue);
            this.Result = instruction.Reverse().First() == '#';

        }

        public bool Matches(Pot[] otherPots)
        {
            if (otherPots[0].ContainsPlant != InstructionAsPots[0].ContainsPlant)
                return false;
            for (int i = 1; i < otherPots.Length; i++)
                if (otherPots[i].ContainsPlant != InstructionAsPots[i].ContainsPlant)
                    return false;
            return true;
        }
    }

    public class PotCave
    {
        public PotInstructionSet Instructions { get; private set; }
        public PotRow RowOfPots { get; private set; }
        public int ProcessingWindowSize { get; private set; }
        public PotCave(PotInstructionSet instructions, PotRow row, int processingWindowSize = 5)
        {
            this.Instructions = instructions;
            this.RowOfPots = row;
            this.ProcessingWindowSize = processingWindowSize;
        }

        public void ProcessGenerations(long numGenerations)
        {
            var timer = new Stopwatch();
            timer.Start();
            for (long i = 0; i < numGenerations; i++)
            {
                processGeneration();
            }
            timer.Stop();
            Trace.WriteLine($"{numGenerations} total generations finished in {timer.Elapsed.TotalSeconds} seconds. This results in around {(numGenerations/(double)timer.Elapsed.TotalSeconds).ToString("N2")} generations per second.");
        }

        private void processGeneration()
        {
            var totalPots = RowOfPots.PotDict.Count;
            var resultDict = new Dictionary<int, bool>();
            for (int i = RowOfPots.LowerBound; i <= RowOfPots.UpperBound; i++)
            {
                var currWindow = RowOfPots.GetPotsInclusive(i, i + ProcessingWindowSize - 1);
                var currWindowKey = currWindow.SimplePotRangeHash();
                if (Instructions.InstructionsDict.ContainsKey(currWindowKey))
                {
                    var foundInstruction = Instructions.GetInstruction(currWindowKey);
                    resultDict[currWindow[2].PotNumber] = foundInstruction.Result;
                }
            }

            //Update our row dictionary
            RowOfPots.StartUpdate();
            //todo: testing a new method
            var tempDict = new Dictionary<int, Pot>(RowOfPots.PotDict);
            foreach (var kvp in tempDict)
            {
                var currentPlant = RowOfPots.PotDict[kvp.Key];
                if (resultDict.ContainsKey(kvp.Key))
                {
                    var newPot = new Pot(resultDict[kvp.Key], currentPlant.PotNumber);
                    RowOfPots.UpdatePot(kvp.Key, newPot);
                }
                else
                {
                    var newPot = new Pot(false, currentPlant.PotNumber);
                    RowOfPots.UpdatePot(kvp.Key, newPot);
                }
            }

            //foreach (var key in resultDict.Keys)
            //{
            //    var currentPlant = RowOfPots.PotDict[key];
            //    var newPot = new Pot(resultDict[key], currentPlant.PotNumber);
            //    RowOfPots.UpdatePot(key, newPot);
            //}

            RowOfPots.EndUpdate();
        }

        public int GetResult()
        {
            return RowOfPots.GetPotNumberSum();
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
        public static PotCave InitPotCave(string inputFile, int buffer = 5)
        {
            var input = File.ReadAllLines(inputFile);
            //Get our initial state
            var initialState = new string(input[0].Skip(15).Select(x => x).ToArray());

            //Build our initial row of pots.
            var potRow = new PotRow();


            var initialStateLength = initialState.Length;
            for (int i = 0; i < initialStateLength + buffer; i++)
            {
                if (i < initialStateLength)
                    potRow.AddPot(new Pot(initialState[i] == '#', i));
                else
                    potRow.AddPot(new Pot(false, i));
                if (i > 0)
                    potRow.AddPot(new Pot(false, i * -1));
            }

            //Get and build our instructions
            var instructions = new PotInstructionSet();
            foreach (var inst in input.Skip(2))
            {
                instructions.AddInstruction(inst);
            }
            potRow.InitRowStats();
            var potCave = new PotCave(instructions, potRow);
            return potCave;
        }


    }
}
