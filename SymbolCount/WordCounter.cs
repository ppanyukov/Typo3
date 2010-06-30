using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SymbolCount
{
    /// <summary>
    /// Cuts text into words and counts each word's frequency.
    /// These are available in WordMap property.
    /// </summary>
    class WordCounter
    {
        private Dictionary<string, int> _wordMap = new Dictionary<string, int>();

        public IDictionary<string, int> WordMap
        {
            get { return _wordMap; }
        }

        public void AddText(TextReader reader)
        {
            AddText(reader, 0);
        }

        public void AddText(TextReader reader, int wordMinLength)
        {
            foreach (var word in GetWords(reader))
            {
                if(word.Length < wordMinLength)
                {
                    continue;
                }

                // WordMap[word]++;
                if (WordMap.ContainsKey(word))
                {
                    WordMap[word]++;
                }
                else
                {
                    WordMap.Add(new KeyValuePair<string, int>(word, 1));
                }
            }
        }

        internal static IEnumerable<string> GetWords(TextReader reader)
        {
            for (; ; )
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                foreach(var word in GetWords(line))
                {
                    yield return word;
                }
            }
        }

        internal static IEnumerable<string> GetWords(string line)
        {
            const char char_A = '\u0041';
            const char char_Z = '\u005A';
            const char char_a = '\u0061';
            const char char_z = '\u007A';

            var buffer = new StringBuilder();
            foreach (var c in line)
            {
                if (c >= char_a && c <= char_z || c >= char_A && c <= char_Z)
                {
                    buffer.Append(c);
                }
                else
                {
                    if (buffer.Length > 0)
                    {
                        // ready to return stuff in buffer and reset it
                        var res = buffer.ToString();
                        buffer.Clear();
                        yield return res;
                    }
                }
            }

            if (buffer.Length > 0)
            {
                // ready to return stuff in buffer and reset it
                var res = buffer.ToString();
                buffer.Clear();
                yield return res;
            }
        }


    }
}
