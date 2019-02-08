using System.IO;
using System.Linq;

namespace Day12_SubterraneanSustainability
{
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
            for (var i = 0; i < initialStateLength + buffer; i++)
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
            foreach (var inst in input.Skip(2)) instructions.AddInstruction(inst);
            potRow.InitRowStats();
            var potCave = new PotCave(instructions, potRow);
            return potCave;
        }
    }
}