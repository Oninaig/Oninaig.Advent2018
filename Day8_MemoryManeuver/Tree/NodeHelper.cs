using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Day8_MemoryManeuver.Tree
{
    public static class NodeHelper
    {
        public static string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static Dictionary<char, int> RepeatingAlphabet;
        public static int AlphabetCounter = 0;
        public static void InitRepeatingAlphabet(bool reset= false)
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


        public static void ReadInput(string fileName)
        {
            var lines = File.ReadAllText(fileName);
            var splitInput = lines.Split(' ').Select(x=> Convert.ToInt32(x)).ToArray();

            var inputQueue = new Queue<MemoryNode>();
            var inputStack = new Stack<MemoryNode>();
            var currChildIndex = 0;

            var root = AddNode(splitInput);
            root.Dump();
            var result = root.GetTotalMetadata();
            Console.ReadLine();
        }


        private static MemoryNode AddNode(int[] data, int needToSkip = 2)
        {
            //var rootHeaderCNodes = Convert.ToInt32(data[0]);
            //var rootHeaderMDataCount = Convert.ToInt32(data[1]);
            
            //var rootHeader = new MemoryHeader();
            //rootHeader.SetChildNodeCount(rootHeaderCNodes);
            //rootHeader.SetMetadataCount(rootHeaderMDataCount);

            var rawDataStack = new Stack<int[]>();
            

            //for our first run we just enqueue everything
            rawDataStack.Push(data);

            //now we create a variable that will store the remaining number of items left in our data array to process.
            //we calculate this by taking the number of header values (2) plus the number of metadata entries for the current series/data.
            var remainingData = data.Length;
            var numChildren = data[0];
            var currSkip = 2;
            while (numChildren > 0)
            {
                numChildren--;
                var currDataStartingWithChild = data.Skip(currSkip).ToArray();
                bool hasChild = false;
                if (currDataStartingWithChild[0] > 0)
                {
                    numChildren++;
                    hasChild = true;
                }

                

                if (hasChild)
                {
                    var currNodeData = currDataStartingWithChild.ToArray();
                    rawDataStack.Push(currNodeData);
                    currSkip += 2;
                }
                else
                {
                    var currNodeData = currDataStartingWithChild.Take(2 + currDataStartingWithChild[1]).ToArray();
                    rawDataStack.Push(currNodeData);
                    currSkip += currNodeData.Length;
                }

            }

            //Process the stack
            var childStack = new Stack<MemoryNode>();
            MemoryNode masterRootNode = null;
            while (rawDataStack.Count > 0)
            {
                var currNodeData = rawDataStack.Pop();
                var rootHeaderCNodes = currNodeData[0];
                var rootHeaderMDataCount = currNodeData[1];

                var rootHeader = new MemoryHeader();
                rootHeader.SetChildNodeCount(rootHeaderCNodes);
                rootHeader.SetMetadataCount(rootHeaderMDataCount);

                masterRootNode = new MemoryNode(rootHeader);


                if (masterRootNode.NumChildNodes == 0)
                {
                    foreach (var meta in currNodeData.Skip(2).Take(currNodeData[1]))
                    {
                        masterRootNode.AddMetadata(meta);
                    }

                }
                else if (childStack.Count > 0 && masterRootNode.NumChildNodes > 0)
                {
                    var childSkip = 2;
                    for (int i = 0; i < masterRootNode.NumChildNodes; i++)
                    {
                        var currChild = childStack.Pop();
                        childSkip += currChild.TotalLength;
                        masterRootNode.AddChild(currChild);
                    }
                    foreach (var meta in currNodeData.Skip(childSkip).Take(currNodeData[1]))
                    {
                        masterRootNode.AddMetadata(meta);
                    }
                }

                
                childStack.Push(masterRootNode);

            }
            return masterRootNode;



        }


    }


}
