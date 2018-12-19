using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7_TheSumOfItsParts.Process
{
    /// <summary>
    /// Workpackage conists of an id (int) and a collection of WorkingStep objects that are "eligible" to be worked on throughout a given "order" of WorkingSteps.
    /// Example: If the order of work is ABC where B and C depend on A to be finished first, your first work package would just be 1-A and your second would be 2-BC.
    /// </summary>
    public class WorkPackages
    {

        public Dictionary<int, WorkPackage> Packages { get; set; }

        public WorkPackages()
        {
            this.Packages = new Dictionary<int, WorkPackage>();
        }

        public void AddWorkPackage(int packageProcessOrder, IEnumerable<WorkingStep> eligibleSteps)
        {
            this.Packages.Add(packageProcessOrder, new WorkPackage(packageProcessOrder, eligibleSteps));
        }
    }

    public class WorkPackage
    {
        public int PackageProcessOrderId { get; set; }
        public IEnumerable<WorkingStep> EligibleSteps { get; set; }

        public WorkPackage(int packageProcessOrderId, IEnumerable<WorkingStep> eligibleSteps)
        {
            PackageProcessOrderId = packageProcessOrderId;
            EligibleSteps = eligibleSteps;
        }
    }
}
