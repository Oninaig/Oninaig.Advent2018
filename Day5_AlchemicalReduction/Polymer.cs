using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5_AlchemicalReduction
{
    public class Polymer : Node<char>
    {
        public bool HasPrevious => Previous != null;
        public bool HasNext => Next != null;

        public Polymer(char element) : base()
        {
            this.Data = element;
        }

        public Polymer(char element, Node<char> next) : base()
        {
            this.Data = element;
            this.Next = next;
            next.Previous = this;
        }

        public override void Destroy()
        {
            if (HasPrevious && HasNext)
            {
                Previous.Next = this.Next;
                Next.Previous = this.Previous;
            }
            else if (HasNext && !HasPrevious)
                Next.Previous = null;
            else if (HasPrevious && !HasNext)
                Previous.Next = null;


            this.Next = null;
            this.Previous = null;
            this.Data = '#';
        }

    }
}
