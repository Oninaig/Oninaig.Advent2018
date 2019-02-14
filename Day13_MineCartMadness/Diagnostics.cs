using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13_MineCartMadness
{
    public class Diagnostics
    {
        public Dictionary<int, byte[]> TickData { get; private set; }

        public Diagnostics()
        {
            this.TickData = new Dictionary<int, byte[]>();
        }

        public void AddData(string tickDataString, int tickCounter)
        {
            this.TickData[tickCounter] = Encoding.ASCII.GetBytes(tickDataString);
        }

        public void DumpCrashData(int tickCounter, int crashCoordX, int crashCoordY)
        {
            for (int i = 0; i < 5; i++)
            {
                var lines = Encoding.ASCII.GetString(TickData[tickCounter-i]).Split(new []{Environment.NewLine}, StringSplitOptions.None);
                for (int j = crashCoordY - 5; j < crashCoordY + 5; j++)
                {
                    for (int k = crashCoordX - 5; k < crashCoordX + 5; k++)
                    {
                        Console.Write(lines[j][k]);
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}
