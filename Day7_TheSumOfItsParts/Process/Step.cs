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
            PreRequisites = new Dictionary<Step, bool>();
            UniqueStepId = Guid.NewGuid();
            StepName = name;
        }

        public Step(Step otherStep)
        {
            PreRequisites = otherStep.PreRequisites;
            UniqueStepId = otherStep.UniqueStepId;
            StepName = otherStep.StepName;
        }

        public bool IsAssigned { get; set; }
        public bool IsCompleted { get; set; }
        public Guid UniqueStepId { get; }
        public Dictionary<Step, bool> PreRequisites { get; }
        public string StepName { get; }

        public bool HasUnmappedPrerequisites => PreRequisites != null && PreRequisites.Any(x => x.Value == false);

        public bool CanProcess => !HasUnmappedPrerequisites;

        public virtual object AddPrerequisite(Step requiredStep)
        {
            PreRequisites.Add(requiredStep, false);
            return this;
        }

        public virtual bool DependsOn(Step otherStep)
        {
            return PreRequisites.Keys.Contains(otherStep);
        }

        public virtual void MarkPrerequisiteAsMapped(Step processedStep)
        {
            PreRequisites[processedStep] = true;
        }
    }
}