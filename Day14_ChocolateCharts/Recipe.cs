using System;

namespace Day14_ChocolateCharts
{
    public class Recipe
    {
        public Recipe(int score)
        {
            Score = score;
            Id = Guid.NewGuid();
        }

        public int Score { get; set; }
        public Guid Id { get; set; }
    }
}