using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4_ReposeRecord
{
    public class GuardStats
    {
        public GuardStats()
        {
            this.SleepEntries = new List<GuardSleepEntry>();
            this.SleepCountDict = new Dictionary<int, int>();
        }

        public int TopMinuteAsleep { get; set; }
        public int TopMinuteAsleepCount { get; set; }
        public List<GuardSleepEntry> SleepEntries { get; set; }

        public double MillsAwake { get;set; }
        public double MillsAsleep { get; set; }
        public Dictionary<int, int> SleepCountDict{get; set; }
        public double SecondsAwake
        {
            get { return MillsAwake / 1000; }
        }

        public double SecondsAsleep
        {
            get { return MillsAsleep / 1000; }
        }

        public double MinutesAwake
        {
            get { return SecondsAwake / 1000; }
        }

        public double MinutesAsleep
        {
            get { return SecondsAsleep / 1000; }
        }

        public void PostProcessSleepData()
        {
            var topMinute = SleepCountDict.OrderBy(x => x.Value).DefaultIfEmpty(new KeyValuePair<int, int>(-1, -1))
                .LastOrDefault();
            TopMinuteAsleep = topMinute.Key;
            TopMinuteAsleepCount = topMinute.Value;
        }

        public void AddSleepEntry(DateTime start, DateTime end)
        {
            //A dictionary is a reference type, so it is not possible to pass by value, although references to a dictionary are values.
            SleepEntries.Add(new GuardSleepEntry(start, end, SleepCountDict));
        }
    }

    public class GuardSleepEntry
    {
        public DateTime SleepStart{get; set; }
        public DateTime SleepEnd { get; set; }
        public List<DateTime> TimesAsleep { get;set; }

        public GuardSleepEntry()
        {
        }
        
        public GuardSleepEntry(DateTime sleepStart, DateTime sleepEnd, Dictionary<int, int> sleepCountDict)
        {
            SleepStart = sleepStart;
            SleepEnd = sleepEnd;
            initSleepTimes(sleepCountDict);
        }
        private void initSleepTimes(Dictionary<int, int> sleepCountDict)
        {
            if (TimesAsleep == null)
                TimesAsleep = new List<DateTime>();

            for (DateTime timeAsleep = SleepStart; timeAsleep < SleepEnd; timeAsleep = timeAsleep.AddMinutes(1))
            {
                TimesAsleep.Add(timeAsleep);
                if (sleepCountDict.ContainsKey(timeAsleep.Minute))
                    sleepCountDict[timeAsleep.Minute] = sleepCountDict[timeAsleep.Minute] + 1;
                else
                    sleepCountDict[timeAsleep.Minute] = 1;
            }
        }
    }

    public class GuardRecord
    {
        public readonly GuardStats Stats;
        public string GuardId { get; set; }

        public IList<GuardAction> Actions { get;set; }

        public GuardRecord(string guardId)
        {
            GuardId = guardId;
            Actions = new List<GuardAction>();
            Stats = new GuardStats();
        }

        public void AddAction(GuardAction action)
        {
            Actions.Add(action);
        }

        public void AddAction(string timeStamp, GuardActionType actionType)
        {
            Actions.Add(new GuardAction(this.GuardId, timeStamp, actionType));
        }

        public void ProcessStats()
        {
            var currAction = GuardActionType.None;
            var sleepCounter = 0.0;
            var awakeCounter = 0.0;
            var sleepStartTime = DateTime.MinValue;
            var wakeStartTime = DateTime.MinValue;
            foreach (var act in Actions)
            {
                switch (act.ActionType)
                {
                    case GuardActionType.BeginShift:
                        break;
                    case GuardActionType.FallAsleep:
                        sleepStartTime = act.TimeStamp;
                        if (wakeStartTime != DateTime.MinValue)
                        {
                            TimeSpan elapsed = (act.TimeStamp - wakeStartTime);
                            awakeCounter += elapsed.TotalMilliseconds;
                            wakeStartTime = DateTime.MinValue;
                        }
                        break;
                    case GuardActionType.WakeUp:
                        if (sleepStartTime != DateTime.MinValue)
                        {
                            TimeSpan elapsed = (act.TimeStamp - sleepStartTime);
                            sleepCounter += elapsed.TotalMilliseconds;
                            Stats.AddSleepEntry(sleepStartTime, act.TimeStamp);
                            sleepStartTime = DateTime.MinValue;
                        }
                        else
                        {
                            throw new InvalidEnumArgumentException(
                                "Guard cannot wake up without first falling asleep.");
                        }

                        wakeStartTime = act.TimeStamp;
                        break;
                    case GuardActionType.None:
                        throw new InvalidEnumArgumentException(
                            "GuardActionType.None should never be set on a valid action.");
                }
            }

            Stats.MillsAsleep = sleepCounter;
            Stats.MillsAwake = awakeCounter;
            Stats.PostProcessSleepData();
        }
    }

    public class GuardAction
    {
        private string _guardId;
        public string GuardId
        {
            get { return _guardId;}
            set
            {
                _guardId = value;
                MissingGuardId = false;
            }
        }

        public DateTime TimeStamp { get; set; }
        public GuardActionType ActionType { get; set; }

        public bool MissingGuardId { get; set; }

        public GuardAction(string timeStamp, GuardActionType actionType)
        {
            TimeStamp = DateTime.ParseExact(timeStamp, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            ActionType = actionType;
            MissingGuardId = true;
        }

        public GuardAction(string guardId, string timeStamp, GuardActionType actionType)
        {
            this.GuardId = guardId;
            TimeStamp = DateTime.ParseExact(timeStamp, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            ActionType = actionType;
        }

        public GuardAction GetSetGuard(string guardId)
        {
            this.GuardId = guardId;
            this.MissingGuardId = false;
            return this;
        }
    }

    public enum GuardActionType
    {
        None,
        BeginShift,
        FallAsleep,
        WakeUp
    }
}
