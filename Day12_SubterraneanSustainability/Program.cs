using System.Diagnostics;

namespace Day12_SubterraneanSustainability
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
                var test = PotTools.InitPotCave("puzzleinput.txt", 5);
                test.ProcessGenerations(10000);
                var result = test.GetResult();
                Debug.WriteLine($"Result {result} | Size: {test.RowOfPots.PotDict.Count}");
        }
    }
}