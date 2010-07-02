using System;
using System.Collections.Generic;
using System.IO;

namespace SymbolCount
{
    public class WordList
    {
        private WordCounter _wordCounter;
        private WordPicker _wordPicker;

        public WordCounter WordCounter
        {
            get { return _wordCounter; }
        }

        public WordPicker WordPicker
        {
            get { return _wordPicker; }
        }

        public void Load(string sourceDir)
        {
            _wordCounter = new WordCounter();
            foreach (var fileName in GetFiles(sourceDir))
            {
                using (var reader = File.OpenText(fileName))
                {
                    WordCounter.AddText(reader, 2);  // words 2 chars or more
                }
            }

            _wordPicker = new WordPicker(WordCounter.WordMap);
        }

        private static IEnumerable<string> GetFiles(string dirName)
        {
            foreach (var fileName in Directory.GetFiles(dirName, "*.cs", SearchOption.AllDirectories))
            {
                if (!fileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (fileName.EndsWith("designer.cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (fileName.EndsWith("AssemblyInfo.cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                yield return fileName;

            }
        }

    }
}
