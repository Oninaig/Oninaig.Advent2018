using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var splitInput = lines.Split(' ');

            var inputQueue = new Queue<MemoryNode>();
            var inputStack = new Stack<MemoryNode>();
            var currChildIndex = 0;
            for (int i = 0; i < splitInput.Length; i++)
            {
                var currentHeaderChildNodeCount = Convert.ToInt32(splitInput[i]);
                var currentHeaderMetadataCount = Convert.ToInt32(splitInput[i + 1]);

                var header = new MemoryHeader();
                header.SetChildNodeCount(currentHeaderChildNodeCount);
                header.SetMetadataCount(currentHeaderMetadataCount);



                var newNode = new MemoryNode(header);

                
                //we are already incrementing by 1 per loop, so the amount we "skip" to get to the next node *WHEN WE HAVE CHILDREN* is just an additional 1
                var skipValue = 2;

                //if our child count is > 0, it means the 3rd value in this current set is starting the first child of the current node
                if (header.ChildNodeCount > 0)
                {
                    inputQueue.Enqueue(newNode);
                    inputStack.Push(newNode);
                    i += skipValue;
                    //also increment our child index counter so we can keep our place during processing
                    currChildIndex += newNode.NumChildNodes;
                    continue;
                }

                for (int j = 0; j < header.MetadataCount; j++)
                {
                    var metaEntry = Convert.ToInt32(splitInput[i + j + 2]);
                    newNode.AddMetadata(metaEntry);
                }
                //if we dont have children, the skipvalue is (numMetaDataEntries - childNodes) + 1
                skipValue = header.MetadataCount - header.ChildNodeCount + 1;
                inputQueue.Enqueue(newNode);
                inputStack.Push(newNode);
                i += skipValue;

            }
            
        }


    }
}
