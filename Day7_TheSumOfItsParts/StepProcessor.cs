using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace Day7_TheSumOfItsParts
{
    public static class StepProcessor
    {
        public static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static readonly int WorkConstant = 60;
        /// <summary>
        /// We know from the samples that each step, when split on whitespaces, contains the current step name at index 7 and its prerequisite at index 1.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static StepMap GenerateStepMapFromFile(string fileName)
        {
            var input = File.ReadAllLines(fileName);
            //var stepMap = new Dictionary<string, Step>();
            var stepMap = new StepMap();

            foreach (var line in input)
            {
                var lineSplit = line.Split(' ');
                var stepName = lineSplit[7];
                var preReqName = lineSplit[1];
                stepMap.AddStep(stepName, preReqName);
            }

            return stepMap;
        }

        public static int GetWorkTimeForLetter(string letter)
        {
            return Alphabet.IndexOf(letter) + 1 + WorkConstant;
        }

    }
}
