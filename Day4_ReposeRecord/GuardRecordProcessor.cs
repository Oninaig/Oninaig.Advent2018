using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4_ReposeRecord
{
    public class GuardRecordProcessor
    {
        public List<GuardRecord> GuardRecords { get; set; }

        public List<GuardAction> AllActions { get;set; }
        public GuardRecordProcessor(string[] rawData)
        {
            AllActions = new List<GuardAction>();
            GuardRecords = initGuardRecords(rawData);
        }

        public GuardRecord SleepiestGuard()
        {
            //Find guard that sleeps the most
            GuardRecords.Sort((x, y) => x.Stats.MinutesAsleep.CompareTo(y.Stats.MinutesAsleep));
            var sleepyGuard = GuardRecords.Last();
            return sleepyGuard;
        }

        private List<GuardRecord> initGuardRecords(string[] data)
        {
            foreach (var rawGRecord in data)
            {
                //our timestamp is between a left and right bracket
                var tStart = rawGRecord.IndexOf('[');
                var tEnd = rawGRecord.IndexOf(']');
                var tStmp = rawGRecord.Substring(tStart + 1, tEnd - tStart - 1);

                //Everything after the timestamp
                var otherData = rawGRecord.Substring(tEnd + 1, rawGRecord.Length - tEnd - 1);

                //If the rest of the data contains a pound sign, the record is being shift record
                if (otherData.Contains('#'))
                {
                    var guardId = otherData.Split(' ').First(x => x.StartsWith("#"));
                    var newAction = new GuardAction(tStmp, GuardActionType.BeginShift).GetSetGuard(guardId);
                    AllActions.Add(newAction);
                    //var newRecord = new GuardRecord(guardId);
                    //newRecord.AddAction(tStmp, GuardActionType.BeginShift);

                }

                if (otherData.Contains("falls asleep"))
                {
                    var newAction = new GuardAction(tStmp, GuardActionType.FallAsleep);
                    AllActions.Add(newAction);
                }
                else if (otherData.Contains("wakes up"))
                {
                    var newAction = new GuardAction(tStmp, GuardActionType.WakeUp);
                    AllActions.Add(newAction);
                }

            }


            //Sort our action list
            ((List<GuardAction>) AllActions).Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp));

            //Run through the list and set guardIds for all actions based on the most recent guardId found that doesn't equal the current guardId being processed.
            var currGuardId = string.Empty;
            foreach (var act in AllActions)
            {
                if (!act.MissingGuardId && act.GuardId != currGuardId)
                    currGuardId = act.GuardId;
                else
                    act.GuardId = currGuardId;
            }

            //Group our sorted and processed list by GuardId
            var grouped = AllActions.GroupBy(x => x.GuardId);

            //Initialize and fill our list of GuardRecords
            GuardRecords = new List<GuardRecord>();
            foreach (var group in grouped)
            {
                var newRecord = new GuardRecord(group.Key);
                foreach (var value in group)
                {
                    newRecord.AddAction(value);
                }

                newRecord.ProcessStats();
                GuardRecords.Add(newRecord);
            }



            //placeholderx
            return GuardRecords;
        }
    }
}
