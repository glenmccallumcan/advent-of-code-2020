using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = LoadData();
            var part1 = Part1(input.Item1, input.Item2);
            var part2 = Part2(input.Item1, input.Item2);

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);

            // Part 1: 370
            // Part 2: 894954360381385
        }

        static int Part1(int time, List<String> busNumbers)
        {
            List<Bus> buses = busNumbers.Where(x => x != "x").Select(x => Int32.Parse(x)).Select(x => new Bus(x, time)).ToList();

            Bus nextBus = buses.OrderBy(x => x.NextTimePastPort).First();

            return nextBus.Number * (nextBus.NextTimePastPort - time);
        }

        static BigInteger Part2(int time, List<String> busNumbers)
        {
            List<Bus> buses = new List<Bus>();

            for(int i = 0; i < busNumbers.Count; i++)
            {
                if(busNumbers[i] == "x")
                {
                    continue;
                }

                buses.Add(new Bus(Int32.Parse(busNumbers[i]), time, i));
            }


            // what follows is the chinese remainder theorem
            // https://www.geeksforgeeks.org/chinese-remainder-theorem-set-2-implementation/
            // https://www.geeksforgeeks.org/multiplicative-inverse-under-modulo-m/
            // https://planetcalc.com/3311/
            BigInteger product = new BigInteger(1);

            foreach(var b in buses)
            {
                product *= new BigInteger(b.Number);
            }
                

            buses.ForEach(b => b.PP = product / new BigInteger(b.Number));

            buses.ForEach(b => b.INV = BigInteger.ModPow(b.PP, new BigInteger(b.Number) - 2, new BigInteger(b.Number)));

            BigInteger sumOfProducts = new BigInteger();

            foreach(var b in buses)
            {
                sumOfProducts += new BigInteger(b.Number - b.Offset) * b.PP * b.INV;
            }

            return sumOfProducts % product;
 
        }

        public static (int,List<string>) LoadData()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\13\\input.txt";

            string[] lines = File.ReadAllLines(location);

            var time = Int32.Parse(lines[0]);
            var buses = lines[1].Split(',').ToList(); ;

            return (time, buses);
        }


    }

    class Bus
    {
        public int Number { get; }

        public int Offset { get; }
        
        public int TripsCompleted { get; }
        
        public int LastTimePastPort { get; private set; }

        public int NextTimePastPort { get; private set; }

        public BigInteger PP { get; set; }

        public BigInteger INV { get; set; }

        public Bus(int num, int time)
        {
            this.Number = num;
            this.TripsCompleted = (int)Math.Floor((double)time /(double) this.Number);
            this.LastTimePastPort = this.TripsCompleted * this.Number;
            this.NextTimePastPort = (this.TripsCompleted + 1) * this.Number;
        }

        public Bus(int num, int time, int offset)
        {
            this.Number = num;
            this.TripsCompleted = (int)Math.Floor((double)time / (double)this.Number);
            this.LastTimePastPort = this.TripsCompleted * this.Number;
            this.NextTimePastPort = (this.TripsCompleted + 1) * this.Number;
            this.Offset = offset;
        }
    }
}
