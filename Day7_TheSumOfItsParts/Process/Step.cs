using System;
using System.Collections.Generic;
using System.Linq;

namespace Day7_TheSumOfItsParts.Process
{
    public class Step
    {
        public Step()
        {
        }

        public Step(string name)
        {
            PreRequisites = new List<Step>();
            UniqueStepId = Guid.NewGuid();
            StepName = name;
        }

        public Step(Step otherStep)
        {
            PreRequisites = otherStep.PreRequisites;
            UniqueStepId = otherStep.UniqueStepId;
            StepName = otherStep.StepName;
        }

        public IList<Step> PreRequisites { get; }
        public Guid UniqueStepId { get; }
        public string StepName { get; }
        public bool HasPrerequisites => PreRequisites != null && PreRequisites.Any();
        public bool CanProcess => !HasPrerequisites;

        public Step AddPrerequisite(Step requiredStep)
        {
            PreRequisites.Add(requiredStep);
            return this;
        }

        public bool DependsOn(Step otherStep)
        {
            return PreRequisites.Contains(otherStep);
        }

        public void RemovePrerequisite(Step processedStep)
        {
            PreRequisites.Remove(processedStep);
        }
    }
}