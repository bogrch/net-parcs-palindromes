using CommandLine;
using Parcs;
using Parcs.Module.CommandLine;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace FloydWarshallParcs
{
    class MainPalindromes : MainModule
    {
        private static CLIOptions options;

        class CLIOptions : BaseModuleOptions
        {
            [Option("input", Required = true, HelpText = "File path to the input array.")]
            public string InputFile { get; set; }
            [Option("output", Required = true, HelpText = "File path to the sorted array.")]
            public string OutputFile { get; set; }
            [Option("p", Required = true, HelpText = "Number of points.")]
            public int PointsCount { get; set; }
        }

        private IChannel[] channels;
        private IPoint[] points;
        private int number;

        static void Main(string[] args)
        {
            options = new CLIOptions();

            if (args != null)
            {
                if (!Parser.Default.ParseArguments(args, options))
                {
                    throw new ArgumentException($@"Cannot parse the arguments. Possible usages: {options.GetUsage()}");
                }
            }

            (new MainPalindromes()).RunModule(options);
        }

        public override void Run(ModuleInfo info, CancellationToken token = default)
        {
           

            int pointsNum = options.PointsCount;
            number = GetNumber(options.InputFile);

            channels = new IChannel[pointsNum];
            points = new IPoint[pointsNum];

            for (int i = 0; i < pointsNum; ++i)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("PalindromesParcs.ModulePalindromes");
            }


            DistributeAllData();

            int[] result = GatherAllData();

            Console.WriteLine("Done");

            var file = File.CreateText(options.OutputFile);
            for(int i = 0; i<result.Length; ++i)
            {
                file.WriteLine(result[i]);
            }
            file.Dispose();
        }

        static int GetNumber(string filename)
        {
            var file = File.ReadAllLines(filename).First();
            return int.Parse(file);
        }
        private int[] GatherAllData()
        {
            int[]result;
            int count = 0;

            for (int i = 0; i < channels.Length; i++)
            {
                int chunk = channels[i].ReadObject<int>();
                count += chunk;
            }

            result = new int[count];
            int current = 0;

            for (int i = 0; i < channels.Length; i++)
            {
                int[] chunk = channels[i].ReadObject<int[]>();
                for(int j = 0; j < chunk.Length; j++)
                {
                    result[current] = chunk[j];
                    current++;
                }
            }

            return result;
        }

        private void DistributeAllData()
        {
            for (int i = 0; i < channels.Length; i++)
            {
                Console.WriteLine($"Sent to channel: {i}");
                //channels[i].WriteData(i);
                int chunkSize = number / options.PointsCount;

                int[] chunk = new int[2];

                chunk[0] = i * chunkSize;
                chunk[1] = (i + 1) * chunkSize;


                channels[i].WriteObject(chunk);
            }
        }
    }
}
