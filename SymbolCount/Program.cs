using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolCount
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length <= 0)
            {
                PrintUsage();
                Exit();
            }

            var dirName = args[0];
            if(!Directory.Exists(dirName))
            {
                Console.WriteLine("Directory '{0}' does not exist or is unaccessible.", dirName);
                Exit();
            }


            var wordCounter = new WordCounter();

            foreach(var fileName in GetFiles(dirName))
            {
                using(var reader = File.OpenText(fileName))
                {
                    wordCounter.AddText(reader, 2);  // words 2 chars or more
                }
            }

            var rnd = new Random();

            var orderedList = wordCounter.WordMap.OrderByDescending((kvp) => kvp.Value).ToList();
            var minIndex = 0;
            var maxIndex = orderedList.Count - 1;

            foreach(var keyValuePairs in orderedList)
            {
                Console.WriteLine(
                    "{0} [{1}, {2}] : {3}", 
                    keyValuePairs.Value, 
                    Math.Log(keyValuePairs.Value), 
                    rnd.Next(minIndex, maxIndex), 
                    keyValuePairs.Key);
            }
        }



        private static IEnumerable<string> GetFiles(string dirName)
        {
            foreach (var fileName in Directory.GetFiles(dirName, "*.cs", SearchOption.AllDirectories))
            {
                if (!fileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if(fileName.EndsWith("designer.cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if(fileName.EndsWith("AssemblyInfo.cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                yield return fileName;

            }
        }


        private static void Exit()
        {
            Environment.Exit(0);
        }

        private static void PrintUsage()
        {
            var progName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            Console.WriteLine("{0} <directory>", progName);
        }
    }
}
