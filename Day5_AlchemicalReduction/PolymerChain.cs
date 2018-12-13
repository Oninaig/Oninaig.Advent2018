using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;

namespace Day5_AlchemicalReduction
{
    public class PolymerChain
    {
        public Node<char> FirstPolymer;
        public Node<char> LastPolymer;
        public int PolymerCount { get; set; }

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
            PolymerCount++;
            if (IsEmpty())
            {
                FirstPolymer = LastPolymer = new Polymer(polymerData);
            }
            else
            {
                var newPoly = new Polymer(polymerData);
                LastPolymer.Next = newPoly;
                newPoly.Previous = LastPolymer;
                LastPolymer = newPoly;
            }
        }

        public void InsertPolymerAtBack(Polymer polymerData)
        {
            PolymerCount++;
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
            PolymerCount++;
            if (IsEmpty())
                FirstPolymer = LastPolymer = new Polymer(polymerData);
            else
                FirstPolymer = new Polymer(polymerData, FirstPolymer);
        }

        public void InsertPolymerAtFront(Polymer polymerData)
        {
            PolymerCount++;
            if (IsEmpty())
            {
                FirstPolymer = LastPolymer = polymerData;
            }
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

            if (polymer == FirstPolymer)
                FirstPolymer = polymer.Next;
            if (polymer == LastPolymer)
                LastPolymer = polymer.Previous;
            polymer.Destroy();
            PolymerCount--;
            return true;
        }

        public void StartReactionSansRecursion(bool verbose = false)
        {
            if (IsEmpty())
            {
                Console.WriteLine("No chain to start reaction", Color.Red);
            }
            else
            {
                var stack = new Stack<Node<char>>();
                stack.Push(FirstPolymer);
                var finished = false;
                while (stack.Count > 0 && !finished)
                {
                    var current = stack.Pop();
                    if (current == LastPolymer)
                        finished = true;
                    if (((Polymer) current).React(verbose))
                    {
                        if (verbose)
                        {
                            Console.WriteLine($"Deleting {current.Data} and {current.Next.Data}...", Color.DarkOrange);
                            Dump(new[] {current.Id, current.Next.Id});
                        }

                        Delete((Polymer) current.Next);
                        Delete((Polymer) current);

                        //maintain stack if necessary
                        if (FirstPolymer != null)
                            stack.Push(FirstPolymer);
                    }
                    else
                    {
                        stack.Push(current.Next);
                    }
                }
            }
        }

        public void Dump(IEnumerable<Guid> standouts = null)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Empty chain", Color.Red);
            }
            else
            {
                var current = FirstPolymer;
                while (current != null)
                {
                    //Aqua will be our color for the start and end of a chain
                    if (current.Id == FirstPolymer.Id)
                        Console.Write('#', Color.Aqua);

                    //standouts != null && standouts.Contains(current.Id) ?

                    Console.Write(current.Data,
                        standouts != null && standouts.Contains(current.Id) ? Color.Red : Color.Gray);

                    //We will use magenta to show links
                    if (current.Next?.Id != current.Id && current.Next != null)
                        Console.Write(new string('-', 2), Color.Magenta);

                    if (current.Id == LastPolymer.Id)
                        Console.Write('#', Color.Aqua);

                    current = current.Next;
                }

                Console.WriteLine($"{Environment.NewLine}End Dump", Color.LightGreen);
            }
        }
    }
}