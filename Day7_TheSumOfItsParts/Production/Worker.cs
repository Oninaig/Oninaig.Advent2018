using System;
using System.Text;
using System.Timers;
using Day7_TheSumOfItsParts.Process;

namespace Day7_TheSumOfItsParts.Production
{
    public class Worker
    {
        //Workers typically know where the timeclock so they can punch in/out
        private readonly TimeClockStation TimeClock;

        //unique id just for my own tracking
        private readonly Guid _uniqueId;

        public Worker(int id, TimeClockStation tClock)
        {
            Id = id;
            TimeClock = tClock;
            TotalWorkCount = 0;
            _uniqueId = Guid.NewGuid();
        }

        public int Id { get; private set; }
        public WorkingStep CurrWorkStep { get; private set; }
        public int TotalWorkCount { get; private set; }
        public int WorkCount { get; private set; }
        public bool IsBusy => CurrWorkStep.RemainingWorkRequired > 0;

        public void SetWork(WorkingStep workStep)
        {
            CurrWorkStep = workStep;
            CurrWorkStep.Init();
            WorkCount = 0;
        }

        public void ClockIn()
        {
            Console.WriteLine($"Worker {this} is clocking in!");
            TimeClock.ClockIn(this, DoWorkTimeStep);
        }

        public void ClockOut()
        {
            Console.WriteLine($"Worker {this} is clocking out!");
            TimeClock.ClockOut(this, DoWorkTimeStep);
        }

        public void DoWorkTimeStep(object sender, ElapsedEventArgs args)
        {
            TotalWorkCount++;
            WorkCount++;
            if (CurrWorkStep.DoWork())
            {
                Console.WriteLine(
                    $"Worker {this} is working! Current total work count: {TotalWorkCount}. Current task required work: {CurrWorkStep.WorkRequired}. Remaining work: {CurrWorkStep.RemaingWorkPct()}");
            }
            else
            {
                Console.Write(
                    $"Worker {this} is finished his current task. Current work count: {TotalWorkCount}. Clocking out...");
                ClockOut();
            }
        }

        public override string ToString()
        {
            var splitId = _uniqueId.ToString().Replace("{", "").Replace("}", "").Split('-');
            var sBuilder = new StringBuilder();
            foreach (var part in splitId)
                sBuilder.Append(part[0]);
            return $"{Id} / {sBuilder}";
        }
    }
}