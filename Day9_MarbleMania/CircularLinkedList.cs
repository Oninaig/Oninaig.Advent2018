using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day9_MarbleMania
{
    public static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> node)
        {
            return node.Next ?? node.List.First;
        }

        public static LinkedListNode<T> PrevOrLast<T>(this LinkedListNode<T> node)
        {
            return node.Previous ?? node.List.Last;
        }
    }
}
