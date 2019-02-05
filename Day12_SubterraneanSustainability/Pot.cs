using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
           this.Row = new List<Pot>(rowLength);
        }
        public PotRow(List<Pot> otherRow)
        {
            this.Row = new List<Pot>(otherRow);
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
            for (int i = 0; i < numGenerations; i++)
            {
                ProcessGeneration();
            }

            var totalCount = 0;
            foreach(var pot in Pots.Row)
                if (pot.ContainsPlant)
                    totalCount += pot.PotNumber;
            Console.WriteLine($"Total number: {totalCount}");
        }

        public void ProcessGeneration()
        {
            var tempPots = new PotRow(Pots.Row);
            //Array.Copy(Pots.Row,0, tempPots.Row,0, Pots.Row.Length);
            for (int i = 0; i < Pots.Row.Count; i++)
            {
                
                var processingWindow = Pots.Row.Skip(i).Take(5).ToArray();
                if (processingWindow.Length < 5)
                {
                    tempPots.Row[i]= new Pot(false, tempPots.Row[i].PotNumber);
                    continue;
                    //var backFilled = new Pot[5];
                    //Array.Copy(processingWindow, backFilled, processingWindow.Length);
                    //processingWindow = backFilled;
                }
                var firstMatch = PotInstructions.Instructions.FirstOrDefault(x => x.Matches(processingWindow));
                if (firstMatch == null)
                {
                    //tempPots.Row[i + 2].ContainsPlant = false;
                    tempPots.Row[i + 2]= new Pot(false, tempPots.Row[i+2].PotNumber);

                    continue;
                }

                processingWindow[2].ContainsPlant = firstMatch.Result == '#';
                //tempPots.Row[i + 2].ContainsPlant = processingWindow[2].ContainsPlant;
                tempPots.Row[i + 2] = new Pot(processingWindow[2].ContainsPlant, tempPots.Row[i+2].PotNumber);

            }

            //Array.Copy(tempPots.Row, Pots.Row, tempPots.Row.Length);
            Pots.Row = new List<Pot>(tempPots.Row);
            Pots.Dump();
        }
    }

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
                //var containsPlant = initialState[i] == '#';
                potRow.Row[negativeCounter] = new Pot(false, i * -1);
                negativeCounter++;
            }

            //init the positive row

            var positiveCounter = 0;
            for (int i = negativeCounter; i < potRow.Row.Count; i++)
            {
                if (positiveCounter >= initialState.Length)
                {
                    potRow.Row[i] = new Pot(false, positiveCounter);
                }
                else
                {
                    var containsPlant = initialState[positiveCounter] == '#';
                    potRow.Row[i] = new Pot(containsPlant, positiveCounter);
                }
                
                positiveCounter++;
            }

            //Get and build our instructions
            var instructions = new PotInstructionSet();
            foreach (var inst in input.Skip(2))
            {
                instructions.AddInstruction(inst);
            }

            var potCave = new PotCave(instructions, potRow);
            return potCave;
        }

      
    }
}
