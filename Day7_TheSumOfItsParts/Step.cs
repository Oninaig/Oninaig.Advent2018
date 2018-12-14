using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Day7_TheSumOfItsParts
{
    public class Step
    {
        public IList<Step> PreRequisites { get; private set; }
        public Guid UniqueStepId { get; private set; }
        public string StepName { get; private set; }
        public bool HasPrerequisites => this.PreRequisites != null && this.PreRequisites.Any();
        public bool CanProcess => !HasPrerequisites;
        public bool Processed { get; set; }
        
        public Step(string name)
        {
            this.PreRequisites = new List<Step>();
            this.UniqueStepId = Guid.NewGuid();
            this.StepName = name;
        }

        public Step AddPrerequisite(Step requiredStep)
        {
            this.PreRequisites.Add(requiredStep);
            return this;
        }

        public bool DependsOn(Step otherStep)
        {
            return this.PreRequisites.Contains(otherStep);
        }

        public void RemovePrerequisite(Step processedStep)
        {
            PreRequisites.Remove(processedStep);
        }
    }
}
