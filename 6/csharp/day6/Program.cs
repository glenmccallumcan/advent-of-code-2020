using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var allAnswersPart1 = LoadPart1Answers();
            var part1 = allAnswersPart1.Select(a => a.Count).Sum();

            var allAnswersPart2 = LoadPart2Answers();
            var part2 = allAnswersPart2.Select(a => a.Count).Sum();

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);

            // Part 1: 6703
            // Part 2: 3430
        }

        static List<HashSet<char>> LoadPart2Answers()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\6\\input.txt";

            string[] lines = File.ReadAllLines(location);

            List<HashSet<char>> allAnswers = new List<HashSet<char>>();
            List<char> groupAnswers = new List<char>();
            bool firstFlag = true;

            foreach (var line in lines)
            {
                if (line == "")
                {
                    allAnswers.Add(new HashSet<char>(groupAnswers.ToArray()));
                    groupAnswers = new List<char>();
                    firstFlag = true;
                }
                else if (firstFlag)
                {
                    groupAnswers = new List<char>(line.ToCharArray());
                    firstFlag = false;
                }
                else
                {
                    groupAnswers = groupAnswers.Intersect(new List<char>(line.ToCharArray())).ToList();
                }
            }

            allAnswers.Add(new HashSet<char>(groupAnswers.ToArray()));

            return allAnswers;
        }

        static List<HashSet<char>> LoadPart1Answers()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\6\\input.txt";

            string[] lines = File.ReadAllLines(location);

            List<HashSet<char>> allAnswers = new List<HashSet<char>>();
            HashSet<char> groupAnswers = new HashSet<char>(); ;

            foreach (var line in lines)
            {
                if (line == "")
                {
                    allAnswers.Add(groupAnswers);
                    groupAnswers = new HashSet<char>();
                }
                else
                {
                    foreach (char a in line.ToCharArray())
                    {
                        groupAnswers.Add(a);
                    }
                }
            }

            allAnswers.Add(groupAnswers);

            return allAnswers;
        }
    }
}
