using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8_MemoryManeuver.Tree
{
    public abstract class Node<T1, T2>
    {
        public Header<T1> Header;
        public Metadata<T2> Metadata;
        
        public abstract int NumChildNodes { get; }
        public abstract IEnumerable<Node<T1,T2>> Children { get; }
        public abstract bool AddChild(Node<T1, T2> otherNode);
    }

    public abstract class Metadata<T>
    {
        public abstract IEnumerable<T> MetadataEntries { get; }

        public abstract void AddData(T data);
        
    }

    public abstract class Header<T>
    {
        public abstract T[] HeaderData { get; }
    }



    public class MemoryNode : Node<int, int>
    {
        private List<Node<int, int>> _children;
        private int _numChildNodes;

        public override int NumChildNodes
        {
            get { return _numChildNodes; }
        }

        public override IEnumerable<Node<int, int>> Children
        {
            get { return this._children; }
        }


        public MemoryNode(MemoryHeader memHeader)
        {
            this.Header = memHeader;
            this.Metadata = new MemoryMetadata();
            this._children = new List<Node<int, int>>();
            this._numChildNodes = memHeader.ChildNodeCount;
        }
        
        public void AddMetadata(int data)
        {
            this.Metadata.AddData(data);
        }

        public override bool AddChild(Node<int, int> otherNode)
        {
            _children.Add(otherNode);
            return true;
        }
    }



    public class MemoryMetadata : Metadata<int>
    {
        private List<int> _metadataEntries;

        public override IEnumerable<int> MetadataEntries
        {
            get { return _metadataEntries; }
        }

        public override void AddData(int data)
        {
            this._metadataEntries.Add(data);
        }

        public MemoryMetadata()
        {
            this._metadataEntries = new List<int>();
        }

    }



    public class MemoryHeader : Header<int>
    {
        private int[] _headerData;
        private int _childNodeCount;
        public static readonly int ChildNodeCountIndex = 0;
        public static readonly int MetaDataCountIndex = 1;

        public override int[] HeaderData
        {
            get { return this._headerData; }
        }

        public int ChildNodeCount
        {
            get { return _headerData[ChildNodeCountIndex]; }
        }

        public int MetadataCount
        {
            get { return _headerData[MetaDataCountIndex]; }
        }

        public void SetChildNodeCount(int count)
        {
            _headerData[0] = count;
        }

        public void SetMetadataCount(int count)
        {
            _headerData[1] = count;
        }

        public MemoryHeader()
        {
            this._headerData = new int[2];
        }
    }
}
