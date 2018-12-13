using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5_AlchemicalReduction
{
    public class PolymerChainManager
    {
        public readonly string BaseInput;
        public Dictionary<char, int> UniqueCharRemovedLength { get; private set; }

        public PolymerChainManager(string inputData)
        {
            this.BaseInput = inputData;
            this.UniqueCharRemovedLength = new Dictionary<char, int>();
            initUnqiues();
        }

        private void initUnqiues()
        {
            foreach (var chr in BaseInput)
            {
                if (!UniqueCharRemovedLength.ContainsKey(chr) && !UniqueCharRemovedLength.ContainsKey(((char)(chr ^ ' '))))
                    UniqueCharRemovedLength.Add(chr, int.MaxValue);
            }
            Console.WriteLine($"Total unique characters: {UniqueCharRemovedLength.Count}");
        }

        public void FindBestCharToRemove()
        {
            var uniqArr = UniqueCharRemovedLength.ToArray();

            for (int i = 0; i < uniqArr.Length; i++)
            {
                Console.WriteLine($"Begin test {i}");
                UniqueCharRemovedLength[uniqArr[i].Key] = TestCharacter(uniqArr[i].Key);
                Console.WriteLine($"End test for {uniqArr[i].Key}. Result was {UniqueCharRemovedLength[uniqArr[i].Key]}");
            }

            Console.WriteLine("Finished testing all keys.");
            Console.WriteLine($"Best character to remove: {UniqueCharRemovedLength.OrderBy(x=>x.Value).First().Key}");
        }

        private int TestCharacter(char tChar)
        {
            var asStr = tChar.ToString();
            var invAsStr = ((char) (tChar ^ ' ')).ToString();
            var testData = BaseInput.Replace(asStr, string.Empty).Replace(invAsStr, string.Empty);
            var puzzleChain = new PolymerChain();
            foreach (var poly in testData)
                puzzleChain.InsertPolymerAtBack(poly);
            puzzleChain.StartReactionSansRecursion();
            return puzzleChain.PolymerCount;
        }
    }
}
