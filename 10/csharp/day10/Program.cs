using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var adapters = LoadData();
            var part1 = Part1(adapters.ToArray());
            var part2 = Part2(adapters.ToList<int>());

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);

            // Part 1: 1656
            // Part 2: 56693912375296
        }

        static int Part1(int[] adapters)
        {
            int[] counts = new int[4];

            int gap = adapters[0] - 0;
            counts[gap]++;

            for (int i = 0; i < adapters.Length - 1; i++)
            {
                gap = adapters[i + 1] - adapters[i];
                counts[gap]++;
            }

            return counts[1] * (counts[3] + 1);
        }

        static long Part2(List<int> adapters)
        {
            adapters.Add(0);
            adapters.Sort();
            adapters.Add(adapters[adapters.Count - 1] + 3);
            Dictionary<int, List<int>> adapterGraph = new Dictionary<int, List<int>>();
            foreach (var adapter in adapters)
            {
                adapterGraph.Add(adapter, adapters.Where(x => x > adapter && (x - adapter <= 3)).ToList());
            }

            long runningTotal = 1;
            int i = 0;
            while (i < adapters.Count)
            {
                if (adapterGraph[adapters[i]].Count == 3)
                {
                    if (adapterGraph[adapters[i + 1]].Count == 3)
                    {
                        runningTotal *= 7;
                        i += 3;
                        continue;
                    }
                    else
                    {
                        runningTotal *= 4;
                        i += 2;
                        continue;
                    }
                }
                else if (adapterGraph[adapters[i]].Count == 2)
                {
                    runningTotal *= 2;
                }

                i++;
            }

            return runningTotal;
        }

        static SortedSet<int> LoadData()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\10\\input.txt";

            string[] lines = File.ReadAllLines(location);

            var temp = new SortedSet<int>();

            for (int i = 0; i < lines.Length; i++)
            {
                temp.Add(Int32.Parse(lines[i]));
            }

            return temp;

        }
    }
}
