using System;
using System.Collections.Generic;
using System.Linq;
using Day7_TheSumOfItsParts.Process.Helpers;

namespace Day7_TheSumOfItsParts.Process
{
    public class Step : IEquatable<Step>
    {
        public int RemainingWorkRequired { get; set; }
        public int WorkRequired { get; private set; }

        public bool HasPrerequisites =>
            PreRequisites.Any(x => !x.Key.IsCompleted);

        public bool IsAssigned { get; set; }
        public bool IsCompleted { get; set; }
        public Guid UniqueStepId { get; }
        public Dictionary<Step, bool> PreRequisites { get; }
        public string StepName { get; }
        public bool HasUnmappedPrerequisites => PreRequisites != null && PreRequisites.Any(x => x.Value == false);
        public bool CanProcessMapping => !HasUnmappedPrerequisites;

        private bool _initialized;

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

        public Step Init()
        {
            if (!_initialized)
            {
                RemainingWorkRequired = WorkRequired = StepProcessor.GetWorkTimeForLetter(StepName);
                _initialized = true;
            }

            return this;
        }

        public string Identify()
        {
            return $"{StepName}";
        }

        public string IdentifyWithWorkRemaining()
        {
            return $"{StepName} - {RemainingWorkRequired}";
        }

        public Step AddPrerequisite(Step requiredStep)
        {
            PreRequisites.Add(requiredStep, false);
            return this;
        }

        public bool DoesHaveDependencyOn(Step otherStep)
        {
            return PreRequisites.Keys.Contains(otherStep);
        }

        public void MarkPrerequisiteAsMapped(Step processedStep)
        {
            PreRequisites[processedStep] = true;
        }
        public bool Equals(Step other)
        {
            if (other is null)
                return false;
            if (UniqueStepId == other.UniqueStepId)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return UniqueStepId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Step);
        }
    }
}