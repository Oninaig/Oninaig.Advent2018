using System;
using System.Collections.Generic;
using System.Text;

namespace Day13_MineCartMadness
{
    public class Diagnostics
    {
        public Diagnostics()
        {
            TickData = new Dictionary<int, byte[]>();
        }

        public Dictionary<int, byte[]> TickData { get; }

        public void AddData(string tickDataString, int tickCounter)
        {
            TickData[tickCounter] = Encoding.ASCII.GetBytes(tickDataString);
        }

        public void DumpCrashData(int tickCounter, int crashCoordX, int crashCoordY)
        {
            for (var i = 0; i < 5; i++)
            {
                var lines = Encoding.ASCII.GetString(TickData[tickCounter - i])
                    .Split(new[] {Environment.NewLine}, StringSplitOptions.None);
                for (var j = crashCoordY - 5; j < crashCoordY + 5; j++)
                {
                    for (var k = crashCoordX - 5; k < crashCoordX + 5; k++) Console.Write(lines[j][k]);

                    Console.WriteLine();
                }
            }
        }
    }
}