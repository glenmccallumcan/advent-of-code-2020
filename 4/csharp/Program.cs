using System;
using System.Linq;

using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day4
{
    static class Program
    {
        static int CountValidPassportsPart1(IEnumerable<string> passports)
        {
            return  passports.Count(x =>
            x.Contains("byr:")
            && x.Contains("iyr:")
            && x.Contains("eyr:")
            && x.Contains("hgt:")
            && x.Contains("hcl:")
            && x.Contains("ecl:")
            && x.Contains("pid"));
        }

        static int CountValidPassportsPart2(IEnumerable<string> passports)
        {
            return passports.Count(x =>
                x.YearsAreValid("byr:", 1920, 2002)
                && x.YearsAreValid("iyr:", 2010, 2020)
                && x.YearsAreValid("eyr:", 2020, 2030)
                && x.HeightIsValid()
                && x.MatchesPattern("ecl:", "^(amb|blu|brn|gry|grn|hzl|oth)$")
                && x.MatchesPattern("hcl:", "^#([a-f]|\\d){6}$")
                && x.MatchesPattern("pid:", "^\\d{9}$")
                );
        }

        static bool MatchesPattern(this string passport, string prefix, string pattern)
        {
            Regex rgx = new Regex(pattern);

            if(passport.Contains(prefix)
                && rgx.IsMatch(passport.Split(" ").Where(x => x.StartsWith(prefix)).First<string>().Remove(0, 4)))
            {
                return true;
            }

            return false;
        }
        static bool YearsAreValid(this string passport, string prefix, int start, int end)
        {

            if (passport.Contains(prefix)
                && int.TryParse(passport.Split(" ").Where(x => x.StartsWith(prefix)).First<string>().Remove(0, 4), out int year)
                && year >= start
                && year <= end)
            {
                return true;
            }

            return false;
        }

        static bool HeightIsValid(this string passport)
        {
            var prefix = "hgt:";
            if(!passport.Contains(prefix))
            { 
            return false;
            }

            string hgt = passport.Split(" ").Where(x => x.StartsWith(prefix)).First<string>().Remove(0, 4);

            if (hgt.EndsWith("in") &&
                int.TryParse(hgt.Substring(0,hgt.Length-2), out int value_in)
                && value_in >= 59
                && value_in <= 76)
            {
                return true;
            }

            if (hgt.EndsWith("cm") &&
                int.TryParse(hgt.Substring(0, hgt.Length - 2), out int value_cm)
                && value_cm >= 150
                && value_cm <= 193)
            {
                return true;
            }

            return false;
        }

        static void Main(string[] args)
        {
            var passports = LoadPassports();
            Console.WriteLine("Part 1: " + CountValidPassportsPart1(passports));
            Console.WriteLine("Part 2: " + CountValidPassportsPart2(passports));

            // Part 1: 254
            // Part 2: 184

        }

        static List<string> LoadPassports()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\4\\input.txt";

            string[] lines = File.ReadAllLines(location);

            List<string> passports = new List<string>();
            string passport = "";

            foreach(var line in lines)
            {
                if(line == "")
                {
                    passports.Add(passport.Trim());
                    passport = "";
                }
                else
                {
                    passport = passport + line + " ";
                }
            }

            passports.Add(passport.Trim());

            return passports;
        }
    }
}
