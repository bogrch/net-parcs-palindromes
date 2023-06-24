using Parcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FloydWarshallParcs
{
    class ModulePalindromes : IModule
    {
        private int[] chunk;

        public void Run(ModuleInfo info, CancellationToken token = default)
        {
            chunk = info.Parent.ReadObject<int[]>();

            int start = chunk[0];
            int finish = chunk[1];
            Console.WriteLine($"Start {start}");
            Console.WriteLine($"Finish {finish}");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var palindromes = GetPalindromes(start, finish);
            sw.Stop();
            Console.WriteLine($"Total time {sw.ElapsedMilliseconds} ms ({sw.ElapsedTicks} ticks)");

            var formattedPalindromes = string.Join("\n", palindromes);
            Console.WriteLine(formattedPalindromes);

            info.Parent.WriteObject(palindromes.Count());
            info.Parent.WriteObject(palindromes.ToArray());
            Console.WriteLine("Done!");
            Console.WriteLine($"Total time {sw.ElapsedMilliseconds} ms ({sw.ElapsedTicks} ticks)");
        }

        static IEnumerable<int> GetPalindromes(int fromNumber, int toNumber)
        {
            for (int currentNumber = fromNumber; currentNumber < toNumber; ++currentNumber)
            {
                var currentString = currentNumber.ToString();
                var reversedString = new string(currentString.Reverse().ToArray());

                if (currentString == reversedString)
                {
                    yield return currentNumber;
                }
            }
        }
    }
}
