using System.Drawing;
using Colorful;

namespace Day5_AlchemicalReduction
{
    public class Polymer : Node<char>
    {
        public Polymer(char element)
        {
            Data = element;
        }

        public Polymer(char element, Node<char> next)
        {
            Data = element;
            Next = next;
            next.Previous = this;
        }

        public override void Destroy()
        {
            if (HasPrevious && HasNext)
            {
                Previous.Next = Next;
                Next.Previous = Previous;
            }
            else if (HasNext && !HasPrevious)
            {
                Next.Previous = null;
            }
            else if (HasPrevious && !HasNext)
            {
                Previous.Next = null;
            }


            Next = null;
            Previous = null;
            Data = '#';
        }

        public char Inverted()
        {
            return (char) (Data ^ ' ');
        }

        public bool React(bool verbose = false)
        {
            //Cant react with anything if you aren't followed by anything
            if (!HasNext)
                return false;

            if (Data == ((Polymer) Next).Inverted())
            {
                //Next.Destroy();
                //Destroy();
                if (verbose)
                    Console.WriteLine($"{Data} REACTS WITH {Next.Data}!", Color.Orange);
                return true;
            }

            return false;
        }
    }
}