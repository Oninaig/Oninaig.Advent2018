using System;

namespace Day5_AlchemicalReduction
{
    public abstract class Node<T>
    {
        public readonly Guid Id;

        protected Node()
        {
            Id = Guid.NewGuid();
        }

        public T Data { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }
        public bool HasPrevious => Previous != null;
        public bool HasNext => Next != null;

        public abstract void Destroy();
    }
}