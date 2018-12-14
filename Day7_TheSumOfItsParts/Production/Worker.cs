using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day7_TheSumOfItsParts.Production
{
    public class Worker
    {
        public int Id { get; private set; }
        public WorkingStep CurrWorkStep { get; private set; }

        public bool IsBusy => CurrWorkStep.WorkRequired > 0;
        public Worker(Step workingStep, int id)
        {
            this.CurrWorkStep = (WorkingStep)workingStep;
            this.CurrWorkStep.Init();
            this.Id = id;
        }

        public void SetWork(Step step)
        {
            this.CurrWorkStep = (WorkingStep) step;
            this.CurrWorkStep.Init();
        }
    }
}
