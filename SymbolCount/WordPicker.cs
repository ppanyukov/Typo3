using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SymbolCount
{
    /// <summary>
    /// Takes the word frequency map and selects a
    /// sequence of words from the map based on how
    /// frequently they appear. The more often the word 
    /// appears in the input map, the more often it will appear
    /// in the ouput sequence.
    /// </summary>
    public class WordPicker : IEnumerable<string>
    {
        private Random _rand = new Random();

        private List<int> _weightTable;
        private Dictionary<int, List<string>> _wordWeightList;

        public WordPicker(IDictionary<string, int> wordMap)
        {
            // List of words grouped by same weight
            var wordWeightList = new Dictionary<int, List<string>>();
            foreach (var p in wordMap)
            {
                var weight = (int)Math.Log(p.Value) + 1;
                var value = p.Key;

                if(!wordWeightList.ContainsKey(weight))
                {
                    wordWeightList.Add(weight, new List<string>());
                }
                wordWeightList[weight].Add(value);
            }
            _wordWeightList = wordWeightList;


            // Get the weights
            // Say we have input work list as Foo-1, Bar-3
            // The resulted weight list will be
            // {1, 3, 3, 3}
            // 
            // The number 1 will appear once, the number 3 will appear 3 times etc
            _weightTable = new List<int>();
            var zzz = (from x in _wordWeightList select x.Key).Distinct();
            foreach(var i in zzz)
            {
                for(var j = 0; j < i; ++j)
                {
                    _weightTable.Add(i);
                }
            }
        }


        public ReadOnlyCollection<int> WeightTable
        {
            get { return _weightTable.AsReadOnly(); }
        }

        public Dictionary<int, List<string>> WordWeightList
        {
            get { return _wordWeightList; }
        }

        public IEnumerator<string> GetEnumerator()
        {
            for(;;)
            {
                // Get the random weight to get the word from.
                // The get random word from the list of words having that weight
                yield return NextWord;
            }
        }

        public string NextWord
        {
            get
            {
                var weightTableIndex = _rand.Next(_weightTable.Count);
                var currentWeight = _weightTable[weightTableIndex];
                var wordListAtWeight = _wordWeightList[currentWeight];
                var wordListIndex = _rand.Next(wordListAtWeight.Count);
                return wordListAtWeight[wordListIndex];   
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
