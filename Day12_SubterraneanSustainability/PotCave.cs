using System.Collections.Generic;
using System.Diagnostics;

namespace Day12_SubterraneanSustainability
{
    public class PotCave
    {
        public PotCave(PotInstructionSet instructions, PotRow row, int processingWindowSize = 5)
        {
            Instructions = instructions;
            RowOfPots = row;
            ProcessingWindowSize = processingWindowSize;
        }

        public PotInstructionSet Instructions { get; }
        public PotRow RowOfPots { get; }
        public int ProcessingWindowSize { get; }

        public void ProcessGenerations(long numGenerations)
        {
            var prevGeneration = 0;
            var prevIncrement = 0;
            for (long i = 0; i < numGenerations; i++)
            {
                processGeneration();
                var currGeneration = RowOfPots.GetPotNumberSum();
                var currIncrement = currGeneration - prevGeneration;
                if (currIncrement == prevIncrement)
                {
                    Debug.WriteLine(
                        $"Increment stable at {currIncrement} at generation {i + 1}. Current total at this generation is {currGeneration}");
                    break;
                }

                prevGeneration = currGeneration;
                prevIncrement = currIncrement;
            }
        }

        private void processGeneration()
        {
            var resultDict = new Dictionary<int, bool>();
            for (var i = RowOfPots.LowerBound; i <= RowOfPots.UpperBound; i++)
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
            foreach (var kvp in RowOfPots.PotDict)
                if (resultDict.ContainsKey(kvp.Key))
                    RowOfPots.UpdatePot(kvp.Key, resultDict[kvp.Key]);
                else
                    RowOfPots.UpdatePot(kvp.Key, false);

            RowOfPots.EndUpdate();
        }

        public int GetResult()
        {
            return RowOfPots.GetPotNumberSum();
        }
    }
}