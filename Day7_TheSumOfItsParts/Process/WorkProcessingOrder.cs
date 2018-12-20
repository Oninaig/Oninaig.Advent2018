using System;
using System.Collections.Generic;
using System.Linq;
using Day7_TheSumOfItsParts.Process.Helpers;

namespace Day7_TheSumOfItsParts.Process
{
    /// <summary>
    ///     Workpackage conists of an id (int) and a collection of Step objects that are "eligible" to be worked on
    ///     throughout a given "order" of Steps.
    ///     Example: If the order of work is ABC where B and C depend on A to be finished first, your first work package would
    ///     just be 1-A and your second would be 2-BC.
    /// </summary>
    public class WorkProcessingOrder
    {
        public WorkProcessingOrder()
        {
            Packages = new Dictionary<int, WorkPackage>();
        }

        public Dictionary<int, WorkPackage> Packages { get; set; }

        public bool HasWorkToDo => Packages.Values.Any(x => x.EligibleSteps.Any(y => !y.IsCompleted));

        public void AddWorkPackage(int packageProcessOrder, IEnumerable<Step> eligibleSteps)
        {
            Packages.Add(packageProcessOrder, new WorkPackage(packageProcessOrder, eligibleSteps));
        }

        public void DumpPackages(bool orderString = true)
        {
            if (orderString)
                Dumper.WriteLine("The order in which the packages can be completed is:");
            foreach (var kvp in Packages)
                Dumper.WriteLine(
                    $"{kvp.Key}: {string.Join(", ", kvp.Value.EligibleSteps.Select(x => x.IdentifyWithWorkRemaining()))}");
        }

        public void DumpInProgress()
        {
            var inProgress = new List<Step>();
            foreach (var kvp in Packages)
            foreach (var step in kvp.Value.EligibleSteps)
                if (step.IsAssigned && !step.IsCompleted && !inProgress.Contains(step))
                    inProgress.Add(step);
            Dumper.WriteLine(
                $"In progress:{Environment.NewLine}{string.Join("", inProgress.Select(x => $"{x.IdentifyWithWorkRemaining()}{Environment.NewLine}"))}");
        }

        public void FlagStepAsAssigned(Step step)
        {
            if (step == null)
                return;
            Dumper.WriteLine($"Assigning {step.StepName}");
            foreach (var kvp in Packages)
            foreach (var stp in kvp.Value.EligibleSteps)
                if (stp.UniqueStepId == step.UniqueStepId)
                {
                    if (!stp.IsAssigned)
                        stp.RemainingWorkRequired = stp.WorkRequired;
                    stp.IsAssigned = true;
                }
        }

        public void FlagStepAsCompleted(Step step)
        {
            if (step == null)
                return;
            foreach (var kvp in Packages)
            foreach (var stp in kvp.Value.EligibleSteps)
            {
                if (stp.UniqueStepId == step.UniqueStepId)
                {
                    stp.IsCompleted = true;
                    stp.IsAssigned = false;
                    stp.RemainingWorkRequired = 0;
                }

                foreach (var prereq in stp.PreRequisites.Keys)
                    if (prereq.UniqueStepId == step.UniqueStepId)
                    {
                        prereq.IsCompleted = true;
                        prereq.IsAssigned = false;
                    }
            }
        }


        public bool DoSetAmountOfWork(Step step, int workAmount)
        {
            if (step == null)
                return true;
            var isCompleted = false;
            Dumper.WriteLine($"Doing {workAmount} units of work for step {step.StepName}");
            foreach (var kvp in Packages)
            foreach (var stp in kvp.Value.EligibleSteps)
                if (stp.UniqueStepId == step.UniqueStepId)
                {
                    stp.RemainingWorkRequired -= workAmount;
                    if (stp.RemainingWorkRequired == 0)
                    {
                        Dumper.WriteLine($"Step {stp.StepName} completed.");
                        stp.IsCompleted = true;
                        stp.IsAssigned = false;
                        isCompleted = true;
                    }
                }

            return isCompleted;
        }
    }

    public class WorkPackage
    {
        public WorkPackage(int packageProcessOrderId, IEnumerable<Step> eligibleSteps)
        {
            PackageProcessOrderId = packageProcessOrderId;
            EligibleSteps = eligibleSteps;
        }

        public int PackageProcessOrderId { get; set; }
        public IEnumerable<Step> EligibleSteps { get; set; }
    }
}