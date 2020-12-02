using System;
using System.Collections.Generic;
using System.IO;

namespace Solution
{
    public static class Solution
    {
        public static void SolutionMethod()
        {
            var sortedNumbers = LoadInput();

            var part1Solution = ProductOfTwoNumbersThatSum2020(sortedNumbers);

            var part2Solution = ProductOfThreeNumbersThatSum2020(sortedNumbers);


        }

        public static int ProductOfThreeNumbersThatSum2020(SortedSet<int> sortedNumbers)
        {
            int firstnumber = 0;
            int secondnumber = 0;
            int thirdnumber = 0;

            foreach (var i in sortedNumbers)
            {
                firstnumber = i;
                foreach (var j in sortedNumbers)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    secondnumber = j;
                    thirdnumber = 2020 - i - j;
                    if (sortedNumbers.Contains(2020 - i - j))
                    {
                        goto End;
                    }
                }
            }

            End:
            return firstnumber * secondnumber * thirdnumber;
        }

        public static int ProductOfTwoNumbersThatSum2020 (SortedSet<int> sortedNumbers)
        {
            int firstnumber = 0;
            int secondnumber = 0;

            foreach (var i in sortedNumbers)
            {
                firstnumber = i;
                secondnumber = 2020 - i;
                if (sortedNumbers.Contains(2020 - i))
                {
                    break;
                }
            }

            return firstnumber * secondnumber;
        }
        private static SortedSet<int> LoadInput()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\1\\input.txt";

            var sortedNumbers = new SortedSet<int>();

            StreamReader file = new StreamReader(location);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                sortedNumbers.Add(Int32.Parse(line));
            }

            file.Close();

            return sortedNumbers;
        }
    }
}
