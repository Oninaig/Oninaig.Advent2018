using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5_AlchemicalReduction
{
    public abstract class Node<T>
    {
        public readonly Guid Id;
        public T Data { get; set; }
        public Node<T> Next { get;set; }
        public Node<T> Previous { get;set; }

        protected Node()
        {
            this.Id = Guid.NewGuid();
        }

        public abstract void Destroy();

    }
}
