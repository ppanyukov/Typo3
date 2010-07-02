using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SymbolCount.Tests
{
    [TestFixture]
    class WordPickerTests
    {
        //[TestCase]
        //public void internal_word_list_preserves_same_number_of_words_as_input_dictionary()
        //{
        //    var x = new Dictionary<string, int>
        //    {
        //        {"foo", 1},
        //        {"bar", 10}
        //    };

        //    var picker = new WordPicker(x);
        //    Assert.That(picker.WordList.Count, Is.EqualTo(x.Count));
        //}


        //[TestCase]
        //public void internal_word_list_is_stored_in_ascending_order_by_weight()
        //{
        //    var x = new Dictionary<string, int>
        //    {
        //        {"bar", 1000},
        //        {"foo", 1}
        //    };

        //    var picker = new WordPicker(x);
        //    Assert.That(picker.WordList[0].Key, Is.EqualTo("foo"));
        //    Assert.That(picker.WordList[1].Key, Is.EqualTo("bar"));

        //}

        //[TestCase]
        //public void internal_word_list_is_weighted_using_log()
        //{
        //    var x = new Dictionary<string, int>
        //    {
        //        {"0", 1},
        //        {"1", 10},
        //        {"2", 100},
        //        {"3", 1000},
        //        {"4", 10000},
        //        {"5", 100000},
        //    };

        //    var picker = new WordPicker(x);
        //    Assert.That(picker.WordList[0].Value, Is.EqualTo(1)); // log(10)
        //    Assert.That(picker.WordList[1].Value, Is.EqualTo(3)); // log(10)
        //    Assert.That(picker.WordList[2].Value, Is.EqualTo(5)); // log(100)
        //    Assert.That(picker.WordList[3].Value, Is.EqualTo(7)); // log(1000)
        //    Assert.That(picker.WordList[4].Value, Is.EqualTo(10)); // log(10000)
        //    Assert.That(picker.WordList[5].Value, Is.EqualTo(12)); // log(100000)
        //}

        [TestCase]
        public void internal_word_list_is_weighted_using_log()
        {
            var x = new Dictionary<string, int>
            {
                {"0-1", 1},      // 1
                {"0-2", 1},      // 1
                {"1-1", 10},     // 3
                {"1-2", 10},     // 3
                {"2-1", 100},    // 5
                {"2-2", 100},    // 5
                {"3-1", 1000},   // 7
                {"3-2", 1000},   // 7
                {"4-1", 10000},  // 10
                {"4-2", 10000},  // 10
                {"5-1", 100000}, // 12
                {"5-2", 100000}, // 12
            };


            var z = new Dictionary<int, List<string>>()
            {
                {1, new List<string> {"0-1", "0-2"}},
                {3, new List<string> {"1-1", "1-2"}},
                {5, new List<string> {"2-1", "2-2"}},
                {7, new List<string> {"3-1", "3-2"}},
                {10, new List<string> {"4-1", "4-2"}},
                {12, new List<string> {"5-1", "5-2"}},
            };

            var picker = new WordPicker(x);
            var w = picker.WordWeightList;
            Assert.That(w.ContainsKey(1), "contains 1");
            Assert.That(w.ContainsKey(3), "contains 3");
            Assert.That(w.ContainsKey(5), "contains 5");
            Assert.That(w.ContainsKey(7), "contains 7");
            Assert.That(w.ContainsKey(10), "contains 10");
            Assert.That(w.ContainsKey(12), "contains 12");
        }

        [TestCase]
        public void internal_weight_list_is_build_correctly()
        {
            var x = new Dictionary<string, int>
            {
                {"0", 1},    // weight will be 1
                {"1", 10},   // weight will be 2
                {"2", 100},  // weight will be 5
            };

            var picker = new WordPicker(x);
            var tab = picker.WeightTable;

            Assert.That(tab.Count((i) => i == 1), Is.EqualTo(1));
            Assert.That(tab.Count((i) => i == 3), Is.EqualTo(3));
            Assert.That(tab.Count((i) => i == 5), Is.EqualTo(5));
            
        }


        [TestCase]
        public void test_random_output()
        {
            var x = new Dictionary<string, int>
            {
                {"0-1", 1},      // 1
                {"0-2", 1},      // 1
                {"1-1", 10},     // 3
                {"1-2", 10},     // 3
                {"2-1", 100},    // 5
                {"2-2", 100},    // 5
                {"3-1", 1000},   // 7
                {"3-2", 1000},   // 7
                {"4-1", 10000},  // 10
                {"4-2", 10000},  // 10
                {"5-1", 100000}, // 12
                {"5-2", 100000}, // 12
            };

            var w = new WordPicker(x);

            var wordCount = 5000;
            var currentWord = 0;
            Dictionary<string, int> buffer = new Dictionary<string, int>();
            foreach(var word in w)
            {
                var wLight = word.Substring(0, 1);
                if (!buffer.ContainsKey(wLight))
                {
                    buffer.Add(wLight, 0);
                }

                buffer[wLight] = buffer[wLight] + 1;

                // Console.WriteLine("{0}: {1}", word, buffer[word]);
                ++currentWord;

                if(currentWord >= wordCount)
                {
                    break;
                }
            }


            int probTotal = 1 + 3 + 5 + 7 + 10 + 12;
            Console.WriteLine(
                "EXPECTED: {0}, {1}, {2}, {3}, {4}, {5}",
                (double)1/probTotal,
                (double)3/probTotal,
                (double)5/probTotal,
                (double)7/probTotal,
                (double)10/probTotal,
                (double)12/probTotal
                );

            int total = 0;
            foreach(var item in buffer.OrderBy((a)=> a.Key))
            {
                total += item.Value;
                Console.WriteLine("{0} = {1}", item.Key, (double)item.Value / wordCount);
            }

            Console.WriteLine("TOTAL: {0}", total);
        }
    }
}
