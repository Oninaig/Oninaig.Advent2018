using System;
using System.Collections.Generic;
using System.Linq;

namespace Day8_MemoryManeuver.Tree
{
    public abstract class Node<T1, T2>
    {
        public Header<T1> Header;
        public Metadata<T2> Metadata;
        public Guid UniqueId;

        public abstract int TotalLength { get; }
        public abstract int NumChildNodes { get; }
        public abstract int NumMetaEntries { get; }
        public abstract IEnumerable<Node<T1, T2>> Children { get; }
        public abstract int NodeValue { get; }
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
        private readonly List<Node<int, int>> _children;
        private readonly int _numChildNodes;
        private int _totalLength;

        public MemoryNode(MemoryHeader memHeader)
        {
            Header = memHeader;
            Metadata = new MemoryMetadata();
            _children = new List<Node<int, int>>();
            _numChildNodes = memHeader.ChildNodeCount;
            NumMetaEntries = memHeader.MetadataCount;
            _totalLength = 2;
            UniqueId = Guid.NewGuid();
        }

        public MemoryNode(int headerChildCount, int headerMetaCount) : this(new MemoryHeader(headerChildCount,
            headerMetaCount))
        {
        }

        public bool HasChildren => NumChildNodes > 0;

        public int CurrentChildCount => Children.Count();

        public override int NodeValue
        {
            get
            {
                if (!HasChildren)
                {
                    var nodeVal = 0;
                    foreach (var meta in Metadata.MetadataEntries)
                        nodeVal += meta;
                    return nodeVal;
                }
                else
                {
                    var nodeVal = 0;
                    foreach (var meta in Metadata.MetadataEntries)
                    {
                        var child = Children.ElementAtOrDefault(meta - 1);
                        if (child != null)
                            nodeVal += child.NodeValue;
                    }

                    return nodeVal;
                }
            }
        }

        public override int TotalLength => _totalLength;

        public override int NumChildNodes => _numChildNodes;

        public override int NumMetaEntries { get; }

        public override IEnumerable<Node<int, int>> Children => _children;

        public override void Dump()
        {
            Console.Write($"{NumChildNodes} {NumMetaEntries} ");
            if (NumChildNodes > 0)
                foreach (var child in _children)
                    child.Dump();

            foreach (var meta in Metadata.MetadataEntries) Console.Write($"{meta} ");
        }

        public int GetTotalMetadata()
        {
            var metaCount = 0;
            if (_numChildNodes > 0)
                foreach (MemoryNode child in _children)
                    metaCount += child.GetTotalMetadata();

            foreach (var meta in Metadata.MetadataEntries) metaCount += meta;

            return metaCount;
        }

        public void AddMetadata(int[] data)
        {
            foreach (var dat in data)
                AddMetadata(dat);
        }

        public void AddMetadata(int data)
        {
            Metadata.AddData(data);
            _totalLength++;
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
        private readonly List<int> _metadataEntries;

        public MemoryMetadata()
        {
            _metadataEntries = new List<int>();
        }

        public override IEnumerable<int> MetadataEntries => _metadataEntries;

        public override void AddData(int data)
        {
            _metadataEntries.Add(data);
        }
    }


    public class MemoryHeader : Header<int>
    {
        public static readonly int ChildNodeCountIndex = 0;
        public static readonly int MetaDataCountIndex = 1;
        private readonly int[] _headerData;

        public int StartingChildCount = -1;

        public MemoryHeader()
        {
            _headerData = new int[2];
        }

        public MemoryHeader(int x, int y)
        {
            _headerData = new int[2];
            _headerData[0] = x;
            _headerData[1] = y;
        }

        public override int[] HeaderData => _headerData;

        public int ChildNodeCount => _headerData[ChildNodeCountIndex];

        public int MetadataCount => _headerData[MetaDataCountIndex];

        public void SetChildNodeCount(int count)
        {
            if (StartingChildCount == -1)
                StartingChildCount = count;
            _headerData[0] = count;
        }

        public void SetMetadataCount(int count)
        {
            _headerData[1] = count;
        }
    }
}