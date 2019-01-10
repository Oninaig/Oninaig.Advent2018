using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8_MemoryManeuver.Tree
{
    public static class NodeHelper
    {
        public static string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static Dictionary<char, int> RepeatingAlphabet;
        public static int AlphabetCounter;

        public static void InitRepeatingAlphabet(bool reset = false)
        {
            if (RepeatingAlphabet == null || reset)
            {
                RepeatingAlphabet = new Dictionary<char, int>();
                foreach (var c in ALPHABET)
                    RepeatingAlphabet.Add(c, 1);
                AlphabetCounter = 0;
            }
        }

        public static string GetNextChar()
        {
            var currCharWithCount = RepeatingAlphabet.ElementAt(AlphabetCounter);
            var currChar = currCharWithCount.Key;
            var currCount = currCharWithCount.Value;
            var ret = new string(currChar, currCount);

            //Increment counters
            AlphabetCounter++;
            if (AlphabetCounter == 26)
                AlphabetCounter = 0;
            RepeatingAlphabet[currChar]++;

            return ret;
        }

        public static int ReadInputPartOne(string fileName)
        {
            var dataQueue = ReadInput(fileName);
            var rootNode = populateTree(dataQueue);
            var result = rootNode.GetTotalMetadata();
            return result;
        }

        public static int ReadInputPartTwo(string fileName)
        {
            var dataQueue = ReadInput(fileName);
            var rootNode = populateTree(dataQueue);
            var result = rootNode.NodeValue;
            return result;
        }

        public static Queue<int> ReadInput(string fileName)
        {
            var lines = File.ReadAllText(fileName);
            var splitInput = lines.Split(' ').Select(x => Convert.ToInt32(x)).ToArray();
            var dataQueue = new Queue<int>();
            foreach (var dat in splitInput)
                dataQueue.Enqueue(dat);
            return dataQueue;
        }


        private static MemoryNode populateTree(Queue<int> data)
        {
            var parentNodes = new Stack<MemoryNode>();
            var childNodes = new Stack<MemoryNode>();
            MemoryNode rootNode = null;
            while (data.Count > 0)
            {
                //If there are any parents AND children waiting, pop one of each and pair them together.
                if (parentNodes.Any() && childNodes.Any())
                {
                    var parent = parentNodes.Pop();
                    var child = childNodes.Pop();
                    parent.AddChild(child);

                    //if parent needs more kids, push it back on the parent stack and move on to the next piece of data
                    if (parent.CurrentChildCount < parent.NumChildNodes)
                    {
                        parentNodes.Push(parent);
                        continue;
                    }

                    //if parent doesnt need any more kids, it means it still needs its metadata
                    var metaCount = parent.NumMetaEntries;
                    while (metaCount > 0)
                    {
                        parent.AddMetadata(data.Dequeue());
                        metaCount--;
                    }

                    //if we still have parents left after popping one off of the stack, it means this current parent is a child of another one, push it to the children stack and move on to the next piece of data.
                    if (parentNodes.Any())
                    {
                        childNodes.Push(parent);
                        continue;
                    }

                    //If there is no more data in the queue, we are finished and our current parent is the root of the entire tree.
                    if (!data.Any())
                    {
                        rootNode = parent;
                        break;
                    }
                }


                var nextHeaderChildCount = data.Dequeue();
                var nextHeaderMetaCount = data.Dequeue();
                var nextNode = new MemoryNode(nextHeaderChildCount, nextHeaderMetaCount);
                if (nextHeaderChildCount > 0)
                {
                    parentNodes.Push(nextNode);
                    continue;
                }

                var nextNodeMetaCount = nextHeaderMetaCount;
                while (nextNodeMetaCount > 0)
                {
                    nextNode.AddMetadata(data.Dequeue());
                    nextNodeMetaCount--;
                }

                //If there are any parents left, you must be a child of one of them.
                if (parentNodes.Any())
                    childNodes.Push(nextNode);
            }

            return rootNode;
        }
    }
}