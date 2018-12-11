using System;
using System.IO;
using System.Linq;

namespace Day4_ReposeRecord
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var data = File.ReadAllLines("input.txt");
            var processor = new GuardRecordProcessor(data);
            var sleepiest = processor.SleepiestGuard();
            var sleepiestMinute = sleepiest.Stats.SleepCountDict.OrderBy(x => x.Value).Last();


            Console.WriteLine(
                $"---PART ONE---{Environment.NewLine}Sleepiest Guard: {sleepiest.GuardId} | Sleepiest Minute: {sleepiestMinute.Key} | ID * SleepiestMinute = {Convert.ToInt32(sleepiest.GuardId.TrimStart('#')) * sleepiestMinute.Key}{Environment.NewLine}");

            var consistentlySleepyGuard = processor.ConsistentSleepyGuard();
            Console.WriteLine(
                $"---PART TWO---{Environment.NewLine}Consisnently Sleepy Guard: {consistentlySleepyGuard.GuardId} " +
                $"| Sleepiest Minute with Count: {consistentlySleepyGuard.Stats.TopMinuteAsleep} :: {consistentlySleepyGuard.Stats.TopMinuteAsleepCount} " +
                $"| ID * Top Sleepy Minute = {Convert.ToInt32(consistentlySleepyGuard.GuardId.TrimStart('#')) * consistentlySleepyGuard.Stats.TopMinuteAsleep}");

            Console.ReadLine();
        }
    }
}