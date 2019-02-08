using System.Linq;

namespace Day12_SubterraneanSustainability
{
    public class PotRangeInstruction
    {
        public PotRangeInstruction(string instruction)
        {
            var inputState = instruction.Substring(0, instruction.Length - instruction.IndexOf("=>") + 1);
            Instructions = inputState;
            InstructionAsPots = new Pot[Instructions.Length];
            for (var i = 0; i < inputState.Length; i++)
                InstructionAsPots[i] = new Pot(instruction[i] == '#', int.MinValue);
            Result = instruction.Reverse().First() == '#';
        }

        public string Instructions { get; }
        public Pot[] InstructionAsPots { get; }
        public bool Result { get; }

        public bool Matches(Pot[] otherPots)
        {
            if (otherPots[0].ContainsPlant != InstructionAsPots[0].ContainsPlant)
                return false;
            for (var i = 1; i < otherPots.Length; i++)
                if (otherPots[i].ContainsPlant != InstructionAsPots[i].ContainsPlant)
                    return false;
            return true;
        }
    }
}