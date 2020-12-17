using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var fields = LoadFields();
            var tickets = LoadTickets();
            var part1 = Part1(fields, tickets);
            var part2 = Part2(fields, tickets);

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);

            // Part 1: 29878
            // Part 2: 855438643439
        }

        static int Part1(List<Field> fields, List<int[]> tickets)
        {
            var sum = 0;
            var problemtickets = new List<int[]>();
            int goodTicketCount = 0;
            bool goodTicket = true;
            foreach(var tic in tickets)
            {
                foreach (var num in tic)
                {
                    if (!fields.Any(f => f.IsNumberValid(num)))
                    {
                        sum += num;
                        goodTicket = false;
                    }
                }
                if(goodTicket)
                {
                    goodTicketCount++;
                }
                goodTicket = true;
            }
            return sum;
        }

        static long Part2(List<Field> fields, List<int[]> tickets)
        {
            List<int[]> goodTickets = tickets.Where(tic => 
                tic.All(t => fields.Any(f => f.IsNumberValid(t)))).ToList();

            while (fields.Where(f => f.Name.StartsWith("departure")).Any(f => f.Index == -1))
            {
                for (int i = 0; i < goodTickets[0].Length; i++)
                {
                    var f = fields.Where(f => f.Index == -1 && goodTickets.All(t => f.IsNumberValid(t[i]))).ToList();
                    if (f.Count == 1)
                    {
                        f.Single().Index = i;
                    }
                }
            }

            var myTicket = new int[] { 53, 67, 73, 109, 113, 107, 137, 131, 71, 59, 101, 179, 181, 61, 97, 173, 103, 89, 127, 139 };

            long myProduct = 1;
            foreach(var field in fields.Where(f => f.Name.StartsWith("departure")))
            {
                myProduct *= myTicket[field.Index];
            }

            return myProduct;
        }

        static List<int[]> LoadTickets()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\16\\input_tickets.txt";

            string[] lines = File.ReadAllLines(location);

            return lines.Select(l => l.Split(",").Select(i => Int32.Parse(i)).ToArray()).ToList();
        }

        static List<Field> LoadFields()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\16\\input_fields.txt";

            string[] lines = File.ReadAllLines(location);
            var fields = new List<Field>();

            foreach(var line in lines)
            {
                var name = line.Split(": ")[0];
                var ranges = line.Split(": ")[1];
                var frl = Int32.Parse(ranges.Split(" or ")[0].Split("-")[0]);
                var frh = Int32.Parse(ranges.Split(" or ")[0].Split("-")[1]);
                var srl = Int32.Parse(ranges.Split(" or ")[1].Split("-")[0]);
                var srh = Int32.Parse(ranges.Split(" or ")[1].Split("-")[1]);
                fields.Add(new Field(name, frl, frh, srl, srh));
            }
            

            return fields;
        }
    }  

    class Field
    {
        public string Name { get; }

        public int FirstRangeLow { get; }

        public int FirstRangeHigh { get; }

        public int SecondRangeLow { get; }

        public int SeconRangeHigh { get; }

        public int Index { get; set; }

        public Field(string nam, int frl, int frh, int srl, int srh)
        {
            this.Name = nam;
            this.FirstRangeLow = frl;
            this.FirstRangeHigh = frh;
            this.SecondRangeLow = srl;
            this.SeconRangeHigh = srh;
            this.Index = -1;
        }

        public bool IsNumberValid(int num)
        {
            return ((num >= FirstRangeLow && num <= FirstRangeHigh)
                    || (num >= SecondRangeLow && num <= SeconRangeHigh));
        }

        public int GetTicketErrorRate(int ticket)
        {
            if (!((ticket >= FirstRangeLow && ticket <= FirstRangeHigh)
                    || (ticket >= SecondRangeLow && ticket <= SeconRangeHigh)))
            {
                return ticket;
            }
            return 0;
        }
    }
}
