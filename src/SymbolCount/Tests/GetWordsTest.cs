using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SymbolCount.Tests
{
    [TestFixture]
    class GetWordsTest
    {

        [TestCase("\n\n", Result = 0)]
        [TestCase("", Result = 0)]
        [TestCase("a b c\na b c", Result = 6)]
        [TestCase(" a b c ", Result = 3)]
        [TestCase("a b c", Result = 3)]
        [TestCase("a 1 b  2 c", Result = 3, TestName = "Numbers are ignored")]
        [TestCase("a1 b2 c", Result = 3, TestName = "numbers are ignored even if they are part of word")]
        public int breaks_line_into_correct_number_of_words(string inputText)
        {
            var reader = new StringReader(inputText);
            return WordCounter.GetWords(reader).Count();
        }


        [TestCase("foo", Result = 1)]
        [TestCase("foo bar", Result = 2)]
        [TestCase("foo bar foo foo", Result = 2)]
        [TestCase("foo foo foo foo", Result = 1)]
        public int correct_number_of_words_appear_in_word_map(string inputString)
        {
            var c = new WordCounter();
            c.AddText(new StringReader(inputString));
            return c.WordMap.Count;
        }


        [TestCase("foo", Result = new[] { "foo" })]
        [TestCase("foo bar", Result = new[] { "bar", "foo"})]
        [TestCase("bar foo", Result = new[] { "bar", "foo" })]
        [TestCase("bar foo bar bar bar foo", Result = new[] { "bar", "foo" })]
        public string[] correct_words_appear_in_word_map(string inputString)
        {
            var c = new WordCounter();
            c.AddText(new StringReader(inputString));
            return c.WordMap.Keys.OrderBy(s => s).ToArray();
        }

        [TestCase("a ab abc abcd", 3, Result = new[] {"abc", "abcd" })]
        public string[] correct_words_appear_in_word_map_when_threshold_is_applied(string inputString, int workMinLength)
        {
            var c = new WordCounter();
            c.AddText(new StringReader(inputString), workMinLength);
            return c.WordMap.Keys.OrderBy(s => s).ToArray();
        }
    }
}
