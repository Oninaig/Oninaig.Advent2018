using System;
using System.Text;
using System.Timers;
using Day7_TheSumOfItsParts.Process;

namespace Day7_TheSumOfItsParts.Production
{
    public class Worker
    {
  
        public int Id { get; }
        public WorkingStep CurrWorkStep { get; private set; }
        public int WorkCount { get; private set; }
        public bool IsBusy => CurrWorkStep.RemainingWorkRequired > 0;

        //Workers typically know where the timeclock so they can punch in/out
        private readonly TimeClockStation TimeClock;

        //unique id just for my own tracking
        private Guid _uniqueId;

        public Worker(int id, TimeClockStation tClock)
        {
            Id = id;
            TimeClock = tClock;
            WorkCount = 0;
            _uniqueId = Guid.NewGuid();
        }

        public void SetWork(WorkingStep workStep)
        {
            CurrWorkStep = workStep;
            CurrWorkStep.Init();
        }

        public void ClockIn()
        {
            Console.WriteLine($"Worker {this} is clocking in!");
            //moving to TimeClockStation TimeClock.TimeClockTimeStep += DoWorkTimeStep;
            TimeClock.WorkerClockIn(this, DoWorkTimeStep);
        }

        public void ClockOut()
        {
            Console.WriteLine($"Worker {this} is clocking out!");
            //TimeClock.TimeClockTimeStep -= DoWorkTimeStep;
            TimeClock.WorkerClockOut(this, DoWorkTimeStep);
        }

        public void DoWorkTimeStep(object sender, ElapsedEventArgs args)
        {
            WorkCount++;
            if (CurrWorkStep.DoWork())
            {
                Console.WriteLine($"Worker {this} is working! Current total work count: {WorkCount}. Current task required work: {CurrWorkStep.WorkRequired}. Remaining work: {CurrWorkStep.RemaingWorkPct()}");
            }
            else
            {
                Console.Write($"Worker {this} is finished his current task. Current work count: {WorkCount}. Clocking out...");
                this.ClockOut();
            }
            
        }

        public override string ToString()
        {
            var splitId = this._uniqueId.ToString().Replace("{", "").Replace("}", "").Split('-');
            var sBuilder = new StringBuilder();
            foreach (var part in splitId)
                sBuilder.Append(part[0]);
            return $"{Id} / {sBuilder.ToString()}";
        }
    }
}