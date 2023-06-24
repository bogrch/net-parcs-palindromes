using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FloydWarshallConsequtive
{
    class Program
    {
        class CommandLineOptions
        {
            [Option("input", Required = true, HelpText = "File path to the input array.")]
            public string InputFile { get; set; }
            [Option("output", Required = true, HelpText = "File path to the sorted array.")]
            public string OutputFile { get; set; }
        }

        private static CommandLineOptions options;

        static void Main(string[] args)
        {
            options = new CommandLineOptions();

            if (args != null)
            {
                if (!Parser.Default.ParseArguments(args, options))
                {
                    throw new ArgumentException($@"Cannot parse the arguments.");
                }
            }

            if (!File.Exists(options.InputFile))
            {
                throw new ArgumentException("Input file doesn't exist");
            }

            var number = GetNumber(options.InputFile);
            var sw = new Stopwatch();
            sw.Start();
            var palindromes = GetPalindromes(0, number).ToArray();
            sw.Stop();
            Console.WriteLine("Done");
            Console.WriteLine($"Total time {sw.ElapsedMilliseconds} ms ({sw.ElapsedTicks} ticks)");

            var file = File.CreateText(options.OutputFile);
            for (int i = 0; i < palindromes.Count(); ++i)
            {
                file.WriteLine(palindromes[i]);
            }
            file.Dispose();
        }

        static int GetNumber(string filename)
        {
            var file = File.ReadAllLines(filename).First();
            return int.Parse(file);
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
