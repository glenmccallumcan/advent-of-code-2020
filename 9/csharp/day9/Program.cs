using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Linq;

using MoreLinq.Extensions;

namespace day9
{
    class Program
    {
        static void Main(string[] args)
        {

            var numbers = LoadData();
            var part1 = Part1(numbers.ToArray(), 25);

            Console.WriteLine("Part 1: " + part1);

            (int, int) indexes = Part2(numbers.ToArray(), part1);
            var subArray = numbers.GetRange(indexes.Item1, indexes.Item2 - indexes.Item1 + 1);
            var part2 = subArray.Min() + subArray.Max();

            Console.WriteLine("Part 2: " + part2);

            // Part 1: 144381670
            // Part 2: 20532569
        }

        static long Part1(long[] numbers, int preambleLength)
        {
            return numbers.Skip(preambleLength)
                .Where((targetNumber, i) => numbers[i..(i + preambleLength)]
                .Subsets(2)
                .All(addedNumbers => addedNumbers.Sum() != targetNumber))
                .First();

        }

        static (int, int) Part2(long[] numbers, long sum)
        {

            long windowSum = 0;
            int start = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                while (windowSum > sum && start < i)
                {
                    windowSum -= numbers[start];
                    start++;
                }

                if (windowSum == sum)
                {
                    return (start, i - 1);
                }

                windowSum += numbers[i];
            }

            return (0, 0);
        }

        static List<long> LoadData()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\9\\input.txt";

            string[] lines = File.ReadAllLines(location);

            return lines.Select(x => long.Parse(x)).ToList(); ;
        }
    }

}
