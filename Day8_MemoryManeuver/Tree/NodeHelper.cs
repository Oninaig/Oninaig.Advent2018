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

            var root = AddNode(splitInput);

            



        }


        private static MemoryNode AddNode(string[] data)
        {
            var rootHeaderCNodes = Convert.ToInt32(data[0]);
            var rootHeaderMDataCount = Convert.ToInt32(data[1]);
            
            var rootHeader = new MemoryHeader();
            rootHeader.SetChildNodeCount(rootHeaderCNodes);
            rootHeader.SetMetadataCount(rootHeaderMDataCount);

            var rootNode = new MemoryNode(rootHeader);
            var childNodeCount = rootHeaderCNodes;
            var needToSkip = 0;
            while (childNodeCount > 0)
            {
                needToSkip += rootNode.AddChild(AddNode(data.Skip(2).ToArray()));
                childNodeCount--;
            }
            
            var metaArr = data.Skip(2 + needToSkip).Take(rootHeaderMDataCount);
            foreach (var meta in metaArr)
            {
                rootNode.AddMetadata(Convert.ToInt32(meta));
            }

            return rootNode;

        }


    }
}
