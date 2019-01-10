using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Day9_MarbleMania
{
    public class Marble
    {
        public bool IsCurrent { get;set; }
        public int Value { get; private set; }

        public Guid UniqueId { get; }

        public Marble(int value)
        {
            Value = value;
            this.UniqueId = Guid.NewGuid();
        }
    }

    public class MarbleGameBoard
    {
        public LinkedList<Marble> Board { get; }
        public Marble CurrentMarble { get; private set; }
        public MarbleGameBoard(){ this.Board = new LinkedList<Marble>();}


        private void SetCurrentMarble(Marble marble)
        {
            if(CurrentMarble!= null)
                CurrentMarble.IsCurrent = false;
            marble.IsCurrent = true;
            CurrentMarble = marble;
        }
        public void AddMarble(Marble marble)
        {
            if (CurrentMarble == null)
            {
                SetCurrentMarble(marble);
                Board.AddFirst(marble);
            }
            else
            {
                if (CurrentMarble == Board.Last.Value)
                {
                    Board.AddAfter(Board.First, marble);
                    SetCurrentMarble(marble);
                }
                else
                {
                    var currentMarbleNode = Board.Find(CurrentMarble);
                    Board.AddAfter(currentMarbleNode.Next, marble);
                    SetCurrentMarble(marble);
                }
            }
        }

        public void PrintBoard()
        {
            foreach (var marble in Board)
            {
                Console.Write($"{(!marble.IsCurrent ? $"{marble.Value} " : $"( {marble.Value} ) ")}");
            }
        }

    }
}
