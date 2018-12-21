using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8_MemoryManeuver.Tree
{
    public abstract class Node
    {
        public int NumChildNodes;
        public IEnumerable<Node> Children;
        public Header<object> Header;
        public Metadata Metadata;

        public abstract bool AddChild(Node otherNode);
    }

    public abstract class Metadata<T>
    {
        public abstract IEnumerable<T> MetadataEntries { get; set; }
    }

    public abstract class Header<T>
    {
        public abstract T[] HeaderData { get; set; }
    }

    public class MemoryNode : Node
    {
        public override Metadata<int> MetaData { get; set; }

        public override bool AddChild(Node otherNode)
        {
            this.MetaData = new MemoryMetadata();
            throw new NotImplementedException();
        }
    }

    public class MemoryMetadata : Metadata<int>
    {
        public override IEnumerable<int> MetadataEntries { get; set; }
    }

    public class MemoryHeader : Header<int>
    {
        public override int[] HeaderData { get; set; }
    }
}
