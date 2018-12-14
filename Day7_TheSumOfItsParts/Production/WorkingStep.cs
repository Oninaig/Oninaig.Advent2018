using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7_TheSumOfItsParts.Production
{
    public class WorkingStep : Step
    {
        public int WorkRequired { get; private set; }

        public WorkingStep(){}

        public WorkingStep(Step baseStep) : base(baseStep)
        {

        }
        public WorkingStep Init()
        {
            this.WorkRequired = StepProcessor.GetWorkTimeForLetter(this.StepName);
            return this;
        }

        /// <summary>
        /// Decrement the work counter. If there is no more work do to, return false.
        /// </summary>
        /// <returns></returns>
        public bool DoWork()
        {
            if (--WorkRequired > 0)
                return true;
            return false;
        }
    }
}
