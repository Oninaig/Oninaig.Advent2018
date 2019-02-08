using System;
using System.Collections.Generic;
using System.Linq;

namespace Day12_SubterraneanSustainability
{
    public class PotRow
    {
        private Dictionary<int, bool> changedPots;

        public PotRow()
        {
            PotDict = new Dictionary<int, Pot>();
            UpperBound = int.MinValue;
            LowerBound = int.MaxValue;
        }

        public Dictionary<int, Pot> PotDict { get; set; }

        public int UpperBound { get; private set; }
        public int LowerBound { get; private set; }
        public int UpperPotWithPlant { get; private set; }
        public int LowerPotWithPlant { get; private set; }

        public void AddPot(Pot pot)
        {
            if (pot.PotNumber > UpperPotWithPlant && pot.ContainsPlant)
                UpperPotWithPlant = pot.PotNumber;
            if (pot.PotNumber < LowerPotWithPlant && pot.ContainsPlant)
                LowerPotWithPlant = pot.PotNumber;
            PotDict[pot.PotNumber] = pot;
        }

        private bool needToExpandRight()
        {
            if (UpperPotWithPlant + 5 > UpperBound)
                return true;
            return false;
        }

        private bool needToExpandLeft()
        {
            if (LowerPotWithPlant - 5 < LowerBound)
                return true;
            return false;
        }

        public void InitRowStats()
        {
            var ordered = PotDict.Keys.OrderBy(x => x).ToArray();
            UpperBound = ordered[ordered.Length - 1];
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
            for (var i = startPotNumber;
                endPotNumber < startPotNumber ? i >= endPotNumber : i <= endPotNumber;
                i += endPotNumber < startPotNumber ? -1 : 1)
                returnSection[potCounter++] = PotDict[i];

            return returnSection;
        }

        public int GetPotNumberSum()
        {
            var sum = 0;
            foreach (var kvp in PotDict.Where(x => x.Value.ContainsPlant)) sum += kvp.Value.PotNumber;

            return sum;
        }

        public void StartUpdate()
        {
            UpperPotWithPlant = int.MinValue;
            LowerPotWithPlant = int.MaxValue;
        }

        public void UpdatePot(int potKey, bool hasPlant)
        {
            PotDict[potKey].ContainsPlant = hasPlant;

            if (hasPlant)
            {
                if (potKey > UpperPotWithPlant)
                    UpperPotWithPlant = potKey;
                if (potKey < LowerPotWithPlant)
                    LowerPotWithPlant = potKey;
            }
        }

        private void addBuffers(bool right, bool left, int bufferSize = 5)
        {
            if (right)
            {
                for (var i = UpperBound; i < UpperBound + bufferSize; i++) AddPot(new Pot(false, i));

                UpperBound = UpperBound + bufferSize - 1;
            }

            if (left)
            {
                for (var i = LowerBound; i > LowerBound - bufferSize; i--)
                    AddPot(new Pot(false, i));
                LowerBound = LowerBound - bufferSize + 1;
            }

            trim();
        }


        private void trim()
        {
            for (var i = LowerBound; i <= LowerPotWithPlant - 5; i++)
            {
                PotDict.Remove(i);
                LowerBound++;
            }

            for (var i = UpperBound; i >= UpperPotWithPlant + 5; i--)
            {
                PotDict.Remove(i);
                UpperBound--;
            }
        }

        public void EndUpdate()
        {
            var needRight = needToExpandRight();
            var needLeft = needToExpandLeft();
            addBuffers(needRight, needLeft);
        }
    }
}