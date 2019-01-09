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
        

        public abstract int TotalLength { get; }
        public abstract int NumChildNodes { get; }
        public abstract int NumMetaEntries { get; }
        public abstract IEnumerable<Node<T1,T2>> Children { get; }
        public abstract int AddChild(Node<T1, T2> otherNode);

        public abstract void Dump();
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
        private int _numMetaEntries;
        private int _totalLength;

        public override int TotalLength
        {
            get { return _totalLength; }
        }

        public override int NumChildNodes
        {
            get { return _numChildNodes; }
        }

        public override int NumMetaEntries
        {
            get { return _numMetaEntries; }
        }

        public override IEnumerable<Node<int, int>> Children
        {
            get { return this._children; }
        }

        public override void Dump()
        {
            Console.Write($"{NumChildNodes} {NumMetaEntries} ");
            if (NumChildNodes > 0)
            {
                foreach (var child in _children)
                {
                    child.Dump();
                }
            }

            foreach (var meta in Metadata.MetadataEntries)
            {
                Console.Write($"{meta} ");
            }
        }

        public int GetTotalMetadata()
        {
            var metaCount = 0;
            if (_numChildNodes > 0)
            {
                foreach (MemoryNode child in _children)
                {
                    metaCount += child.GetTotalMetadata();
                }
            }

            foreach (var meta in Metadata.MetadataEntries)
            {
                metaCount += meta;
            }

            return metaCount;
        }

        public MemoryNode(MemoryHeader memHeader)
        {
            this.Header = memHeader;
            this.Metadata = new MemoryMetadata();
            this._children = new List<Node<int, int>>();
            this._numChildNodes = memHeader.ChildNodeCount;
            this._numMetaEntries = memHeader.MetadataCount;
            this._totalLength = 2;
        }
        
        public void AddMetadata(int data)
        {
            this.Metadata.AddData(data);
            this._totalLength++;
        }

        public override int AddChild(Node<int, int> otherNode)
        {
            _children.Add(otherNode);
            _totalLength += otherNode.TotalLength;
            return _totalLength;
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
