using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day9_MarbleMania
{
    public class MarbleCircle
    {
        public LinkedList<Marble> Circle { get; private set; }
        public int CurrentNum { get; private set; }
        
        private LinkedListNode<Marble> _currentMarble;
        public MarbleCircle()
        {
            this.CurrentNum = 0;
            this.Circle = new LinkedList<Marble>();
        }

        public void AddMarble()
        {
            if (!this.Circle.Any())
            {
                var newMarble = new Marble(CurrentNum++);
                this._currentMarble= this.Circle.AddFirst(newMarble);
                return;
            }

            this._currentMarble = Circle.AddAfter(_currentMarble.NextOrFirst(), new Marble(CurrentNum++));

        }
    }

    public class Marble
    {
        public int Number { get; set; }

        public Marble(int number)
        {
            Number = number;
            
        }
    }
}
