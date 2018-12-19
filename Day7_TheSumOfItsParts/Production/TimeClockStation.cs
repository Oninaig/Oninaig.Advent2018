using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Day7_TheSumOfItsParts.Production
{
    public class TimeClockStation
    {
        private readonly Timer _timeClock;
        private readonly Guid _timerId;

        public TimeClockStation(double interval = 1000.0)
        {
            _timeClock = new Timer(interval);
            _timeClock.Elapsed += MasterElapsed;
            _timerId = Guid.NewGuid();
            ClockedInWorkers = new List<Worker>();
        }

        public List<Worker> ClockedInWorkers { get; }


        public event ElapsedEventHandler TimeClockTimeStep
        {
            add => _timeClock.Elapsed += value;
            remove => _timeClock.Elapsed -= value;
        }

        /// <summary>
        /// This event will always be the first to subscribe to the timer elapsed event so we always have an up-to-date status of the timer as the first handler thats called when the timer elapses.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MasterElapsed(object sender, ElapsedEventArgs args)
        {
            Console.WriteLine($"Timer {this} current clocked in workers: {ClockedInWorkers.Count}");
        }

        public void ClockIn(Worker worker, ElapsedEventHandler act)
        {
            if (ClockedInWorkers.Contains(worker))
                throw new ArgumentException(
                    $"Worker {worker} can't clock in more than once before clocking out in between!");
            ClockedInWorkers.Add(worker);
            TimeClockTimeStep += act;
        }

        public void ClockOut(Worker worker, ElapsedEventHandler act)
        {
            if (!ClockedInWorkers.Contains(worker))
                throw new ArgumentException($"Worker {worker} can't clock out without clocking in first!");
            ClockedInWorkers.Remove(worker);
            TimeClockTimeStep -= act;
        }

        public void Start()
        {
            Console.WriteLine($"Timeclock {this} is starting!");
            _timeClock.Start();
        }

        public void Stop()
        {
            _timeClock.Stop();
        }

        public void Restart()
        {
            _timeClock.Enabled = false;
            _timeClock.Enabled = true;
        }

        public override string ToString()
        {
            var splitId = _timerId.ToString().Replace("{", "").Replace("}", "").Split('-');
            var sBuilder = new StringBuilder();
            foreach (var part in splitId)
                sBuilder.Append(part[0]);
            return sBuilder.ToString();
        }
    }
}