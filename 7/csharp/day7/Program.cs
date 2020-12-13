using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var bagRules = LoadBagRules();

            var numBagThatCanContain = new List<LuggageRule>();
            NumBagsThanCanContain(bagRules, numBagThatCanContain, "shiny gold");
            var part1 = numBagThatCanContain.Select(x => x.OuterColour).Distinct().Count();

            var part2 = NumBagsContainedInside(bagRules, "shiny gold") - 1; // subtract 1 because shiny gold bag is counted

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);
            // Part 1: 151
            // Part 2: 41559
        }

        static int NumBagsContainedInside(List<LuggageRule> allBagRules, string outerColour)
        {
            var sum = 1;
            foreach (var bag in allBagRules.Where(x => x.OuterColour == outerColour))
            {
                sum += bag.Quantity * NumBagsContainedInside(allBagRules, bag.InnerColour);
            }

            return sum;
        }

        static void NumBagsThanCanContain(List<LuggageRule> allBagRules, List<LuggageRule> canContainBagRules, string innerColour)
        {
            canContainBagRules.AddRange(allBagRules.Where(x => x.InnerColour == innerColour));

            foreach (var bag in allBagRules.Where(x => x.InnerColour == innerColour))
            {
                NumBagsThanCanContain(allBagRules, canContainBagRules, bag.OuterColour);
            }
        }


        static List<LuggageRule> LoadBagRules()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\7\\input.txt";

            string[] lines = File.ReadAllLines(location);

            List<LuggageRule> bagRules = new List<LuggageRule>();

            foreach (var line in lines)
            {
                var outerColour = line.Split("bags contain")[0].Trim();

                var rx = new Regex(@"( bags, | bag, | bags\.| bag\.)", RegexOptions.Compiled);
                var childBags = rx.Split(line.Split("bags contain")[1]);

                for (int i = 0; i < childBags.Length - 2; i = i + 2)
                {
                    var rawChildBag = childBags[i].Trim();
                    if (rawChildBag != "no other")
                    {
                        var innerColour = rawChildBag.Substring(2, rawChildBag.Length - 2);
                        int quantity = Int32.Parse(rawChildBag.Substring(0, 1));

                        bagRules.Add(new LuggageRule(outerColour, innerColour, quantity));
                    }
                }
            }

            return bagRules;
        }
    }

    class LuggageRule
    {
        public string OuterColour { get; }

        public string InnerColour { get; }

        public int Quantity { get; }

        public LuggageRule(string outer, string inner, int quant)
        {
            this.OuterColour = outer;
            this.InnerColour = inner;
            this.Quantity = quant;
        }
    }
}
