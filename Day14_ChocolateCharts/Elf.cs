using System;

namespace Day14_ChocolateCharts
{
    public class Elf
    {
        public Elf(int recipeScore)
        {
            Id = Guid.NewGuid();
            CurrIndex = -1;
            this.recipeScore = recipeScore;
        }

        public int recipeScore { get; set; }
        public Guid Id { get; set; }
        public int CurrIndex { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}