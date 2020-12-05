using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day5
{
    public class Program
    {
        static void Main(string[] args)
        {
            var passes = LoadBoardingPasses();
            int maxSeatID = 0;
            var passengerList = new List<int>();

            foreach(var pass in passes)
            {
                var seatID = ComputeSeatId(pass);
                passengerList.Add(seatID);

                if(seatID > maxSeatID)
                {
                    maxSeatID = seatID;
                }
            }

            var unusedSeats = Enumerable.Range(0, maxSeatID).Except(passengerList);
            
            Console.WriteLine("Part 1: " + maxSeatID);
            Console.WriteLine("Part 2: " + unusedSeats.Max());

            //Part 1: 938
            //Part 2: 696
        }

        public static int ComputeSeatId(string input)
        {
            int row = Quicksort(input.Substring(0,7).ToCharArray(), 'F', 'B', 128); //get row
            int seat = Quicksort(input.Substring(7, 3).ToCharArray(), 'L', 'R', 8); //get seat
            return (row * 8) + seat; //multiply them together

            //profit
        }

        static int Quicksort (char[] array, char bottomChar, char topChar, int numElements)
        {
            int lower = 0;
            int upper = numElements - 1;

            for(int i = 0; i < array.Length-1; i++ )
            {
                if(array[i] == bottomChar)
                {
                    upper = (upper + lower) / 2;
                }
                if(array[i] == topChar)
                {
                    lower = (upper + lower) / 2 + 1;
                }
            }

            if(array[array.Length-1] == bottomChar)
            {
                return lower;
            }

            return upper;


        }

        static List<string> LoadBoardingPasses()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\5\\input.txt";

            string[] lines = File.ReadAllLines(location);

            List<string> passports = new List<string>(lines);

            return passports;
        }

    }
}
