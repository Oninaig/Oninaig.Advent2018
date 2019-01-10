using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day9_MarbleMania
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new MarbleGameBoard();
            for (int i = 0; i < 5; i++)
            {
                board.AddMarble(new Marble(i));
            }
            board.PrintBoard();
            Console.ReadLine();
        }
    }
}
