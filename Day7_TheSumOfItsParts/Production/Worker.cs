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
        public Worker(int id)
        {
            this.Id = id;
        }

        public void SetWork(WorkingStep workStep)
        {
            this.CurrWorkStep = workStep;
            this.CurrWorkStep.Init();
        }
    }
}
