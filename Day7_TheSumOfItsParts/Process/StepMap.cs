using System;
using System.Collections.Generic;
using System.Linq;

namespace Day7_TheSumOfItsParts.Process
{
    public class StepMap
    {
        public StepMap()
        {
            Map = new Dictionary<string, Step>();
        }

        public Dictionary<string, Step> Map { get; }

        public void AddStep(string stepName, string preReqName)
        {
            Step preReqStep;
            Step currStep;

            //If we already have this step in our dictionary, add to it instead of creating a new step.
            if (Map.ContainsKey(stepName))
                currStep = Map[stepName];
            else
                currStep = new Step(stepName);

            if (Map.ContainsKey(preReqName))
                preReqStep = Map[preReqName];
            else
                preReqStep = new Step(preReqName);

            Map[preReqName] = preReqStep;
            Map[stepName] = currStep.AddPrerequisite(preReqStep);
        }

        public void PrintOrder()
        {
            var steps = Map.Values.Where(x => !x.HasUnmappedPrerequisites).OrderBy(x => x.StepName);
            if (!steps.Any())
                return;

            var nextStep = steps.First();
            if (nextStep.CanProcessMapping)
                foreach (var dependent in Map.Values.Where(x => x.DoesHaveDependencyOn(nextStep)).OrderBy(x => x.StepName))
                    dependent.MarkPrerequisiteAsMapped(nextStep);

            Console.Write($"{nextStep.StepName}");
            Map.Remove(nextStep.StepName);
            PrintOrder();
        }

        public bool RemoveStep(Step step)
        {
            if (Map.ContainsKey(step.StepName))
            {
                Map.Remove(step.StepName);
                return true;
            }

            return false;
        }
    }
}