using System;

namespace Day12_SubterraneanSustainability
{
    public class Pot : IEquatable<Pot>
    {
        public Pot(bool containsPlant, int potNum)
        {
            PotNumber = potNum;
            ContainsPlant = containsPlant;
        }

        public int PotNumber { get; set; }
        public bool ContainsPlant { get; set; }

        public bool Equals(Pot otherPot)
        {
            if (otherPot.ContainsPlant == ContainsPlant)
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pot))
                return false;
            var otherPot = (Pot) obj;
            if (otherPot.ContainsPlant == ContainsPlant)
                return true;
            return false;
        }

        public override string ToString()
        {
            return $"{(ContainsPlant ? "#" : ".")}";
        }

        public int GetNonUniqueHashCode()
        {
            var hash = 17;
            hash = hash * 23 + ContainsPlant.GetHashCode();
            return hash;
        }
    }
}