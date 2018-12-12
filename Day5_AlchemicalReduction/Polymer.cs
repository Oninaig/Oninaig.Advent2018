using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;
namespace Day5_AlchemicalReduction
{
    public class Polymer : Node<char>
    {
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

        public char Inverted()
        {
            return (char) (Data ^ ' ');
        }

        public bool React(bool verbose = false)
        {
            //Cant react with anything if you aren't followed by anything
            if (!HasNext)
                return false;

            if (this.Data == ((Polymer) Next).Inverted())
            {
                //Next.Destroy();
                //Destroy();
                if(verbose)
                    Console.WriteLine($"{Data} REACTS WITH {Next.Data}!", Color.Orange);
                return true;
            }
            
            return false;
        }
    }
}
