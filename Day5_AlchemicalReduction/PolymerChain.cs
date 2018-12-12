using System;
using System.Drawing;
using Console = Colorful.Console;

namespace Day5_AlchemicalReduction
{
    public class PolymerChain
    {
        public Node<char> FirstPolymer;
        public Node<char> LastPolymer;

        public PolymerChain()
        {
            FirstPolymer = LastPolymer = null;
        }

        public bool IsEmpty()
        {
            return FirstPolymer == null;
        }

        public void InsertPolymerAtBack(char polymerData)
        {
            if (IsEmpty())
            {
                FirstPolymer = LastPolymer = new Polymer(polymerData);
            }
            else
            {
                LastPolymer.Next = new Polymer(polymerData);
                LastPolymer = LastPolymer.Next;
            }
        }
        public void InsertPolymerAtBack(Polymer polymerData)
        {
            if (IsEmpty())
            {
                FirstPolymer = LastPolymer = polymerData;
            }
            else
            {
                LastPolymer.Next = polymerData;
                polymerData.Previous = LastPolymer;
                LastPolymer = polymerData;
            }
        }
        public void InsertPolymerAtFront(char polymerData)
        {
            if (IsEmpty())
                FirstPolymer = LastPolymer = new Polymer(polymerData);
            else
                FirstPolymer = new Polymer(polymerData, FirstPolymer);
        }

        public void InsertPolymerAtFront(Polymer polymerData)
        {
            if (IsEmpty())
                FirstPolymer = LastPolymer = polymerData;
            else
            {
                FirstPolymer.Previous = polymerData;
                polymerData.Next = FirstPolymer;
                FirstPolymer = polymerData;
            }
        }

        public bool Delete(Polymer polymer)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Empty chain", Color.Red);
                return false;
            }

            var current = FirstPolymer;
            while (current != null)
            {
                var next = current.Next;
                if (current.Id == polymer.Id)
                    current.Destroy();
                current = next;
            }

            return true;
        }

        public bool StartReaction()
        {

            if (IsEmpty())
                Console.WriteLine("No chain to start reaction", Color.Red);
            else
            {
                var current = FirstPolymer;
                while (current != null)
                {
                    if (((Polymer)current).React())
                    {
                        Console.Write($"Deleting {current.Data} and {current.Next.Data}...", Color.DarkOrange);
                        
                        //We are reacting, run through our checks and deletions
                        //First check: If current.next.next is null AND current.prev is NOT null, it means current.next was
                        //the last element in the chain so we need to set current.prev.next to null.
                        if (current.Previous != null && current.Next.Next == null)
                        {

                        }

                    }
                }
            }
        }


        public void Dump()
        {
            if (IsEmpty())
                Console.WriteLine("Empty chain", Color.Red);
            else
            {
                var current = FirstPolymer;
                while (current != null)
                {
                    //Aqua will be our color for the start and end of a chain
                    if (current.Id == FirstPolymer.Id)
                        Console.Write('#', Color.Aqua);

                    Console.Write(current.Data);

                    //We will use magenta to show links
                    if (current.Next?.Id != current.Id && current.Next != null)
                        Console.Write(new string('-', 2), Color.Magenta);

                    if (current.Id == LastPolymer.Id)
                        Console.Write('#', Color.Aqua);

                    current = current.Next;
                }

                Console.WriteLine($"{Environment.NewLine}Done", Color.Green);
            }
        }
    }
}