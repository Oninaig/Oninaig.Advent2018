using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4_ReposeRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("input.txt");
            var processor = new GuardRecordProcessor(data);
            var sleepiest = processor.SleepiestGuard();
            var sleepiestMinute = sleepiest.Stats.SleepCountDict.OrderBy(x => x.Value).Last();


            Console.WriteLine($"Sleepiest Guard: {sleepiest.GuardId} | Sleepiest Minute: {sleepiestMinute.Key} | ID * SleepiestMinute = {Convert.ToInt32(sleepiest.GuardId.TrimStart('#')) * sleepiestMinute.Key}");
            Console.ReadLine();
        }
    }
}
