using System.Collections.Generic;

namespace Day12_SubterraneanSustainability
{
    public class PotInstructionSet
    {
        public PotInstructionSet()
        {
            InstructionsDict = new Dictionary<int, PotRangeInstruction>();
        }

        public List<PotRangeInstruction> Instructions { get; private set; }
        public Dictionary<int, PotRangeInstruction> InstructionsDict { get; }

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
}