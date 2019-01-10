using System.Collections.Generic;

namespace Day9_MarbleMania
{
    public static class LinkedListExtensions
    {
        public static LinkedListNode<T> CircularMoveBack<T>(this LinkedList<T> lList, LinkedListNode<T> element,
            int moveCount)
        {
            var currElement = element;
            while (moveCount > 0)
            {
                if (currElement == lList.First)
                    currElement = lList.Last;
                else
                    currElement = currElement.Previous;
                moveCount--;
            }

            return currElement;
        }
    }
}