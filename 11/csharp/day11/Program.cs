using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;


namespace day11
{
    static class Program
    {
        static void Main(string[] args)
        {
            var seatArray = LoadData();
            var ferry = BuildFerry(seatArray);
            var part1 = SeatPassengers(ferry, false, 4);
            var part2 = SeatPassengers(ferry, true, 5);

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);

            // Part 1: 2275
            // Part 2: 2121
        }

        static int SeatPassengers(List<Seat> ferryBeforeRules, bool extendedVisability, int adjacentOccupideAcceptability)
        {
            int seatsOccupiedPrevious = ferryBeforeRules.Count(s => s.Occupied);
            int seatsOccupiedNow = 0;
            List<Seat> ferryAfterRules;

            while (seatsOccupiedPrevious !=
                seatsOccupiedNow)
            {
                seatsOccupiedPrevious = seatsOccupiedNow;
                ferryAfterRules = ferryBeforeRules.DeepClone();

                foreach (var seat in ferryBeforeRules.Where(s => !s.Aisle))
                {

                    if (!seat.Occupied && (extendedVisability ? seat.AllExtendedAdjacentSeatsUnoccupied() : seat.AllAdjacentSeatsUnnoccupied()))
                    {
                        ferryAfterRules.GetSeatAt(seat.Row, seat.Column).Occupied = true;
                    }

                    if (seat.Occupied && (extendedVisability ? seat.ExtendedOccupiedCount() : seat.AdacentOccupiedCount()) >= adjacentOccupideAcceptability)
                    {
                        ferryAfterRules.GetSeatAt(seat.Row, seat.Column).Occupied = false;
                    }
                }

                seatsOccupiedNow = ferryAfterRules.Count(s => s.Occupied);
                ferryBeforeRules = ferryAfterRules;
            }

            return seatsOccupiedNow;

        }

        static List<Seat> BuildFerry(char[,] seats)
        {
            var ferry = new List<Seat>();
            int numRows = seats.GetLength(0);
            int numColums = seats.GetLength(1);

            for (int row = 0; row < numRows; row++)
            {
                for (int column = 0; column < numColums; column++)
                {
                    var seat = ferry.GetSeatAt(row, column);
                    if (seats[row, column] == '.')
                    {
                        seat.Aisle = true;
                        seat.Occupied = false;
                    }

                    if (row > 0)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.Front, ferry.GetSeatAt(row - 1, column)));
                    }

                    if (row > 0 && column > 0)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.FrontLeft, ferry.GetSeatAt(row - 1, column - 1)));
                    }

                    if (column > 0)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.Left, ferry.GetSeatAt(row, column - 1)));
                    }

                    if (row < numRows - 1 && column > 0)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.RearLeft, ferry.GetSeatAt(row + 1, column - 1)));
                    }

                    if (row < numRows - 1)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.Rear, ferry.GetSeatAt(row + 1, column)));
                    }

                    if (row < numRows - 1 && column < numColums - 1)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.RearRight, ferry.GetSeatAt(row + 1, column + 1)));
                    }

                    if (column < numColums - 1)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.Right, ferry.GetSeatAt(row, column + 1)));
                    }

                    if (row > 0 && column < numColums - 1)
                    {
                        seat.AdjacentSeats.Add((SeatDirection.FrontRight, ferry.GetSeatAt(row - 1, column + 1)));
                    }
                }
            }

            return ferry;
        }

        static char[,] LoadData()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\11\\input.txt";

            string[] lines = File.ReadAllLines(location);

            int numRows = lines.Length;
            int numColums = lines[0].Length;

            var seats = new char[numRows, numColums];

            for (int i = 0; i < numRows; i++)
            {
                var line = lines[i].ToCharArray();
                for (int j = 0; j < numColums; j++)
                {
                    seats[i, j] = line[j];
                }
            }

            return seats;
        }

        static Seat GetSeatAt(this List<Seat> ferry, int row, int column)
        {
            if (!ferry.Any(s => s.Row == row && s.Column == column))
            {
                var seat = new Seat(row, column);
                ferry.Add(seat);
                return seat;
            }

            return ferry.Single(s => s.Row == row && s.Column == column);
        }

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }

    [Serializable]
    class Seat
    {
        public int Row { get; }

        public int Column { get; }
        public bool Occupied { get; set; }

        public bool Aisle { get; set; }

        public List<(SeatDirection, Seat)> AdjacentSeats { get; set; }

        public Seat(int r, int c)
        {
            this.Row = r;
            this.Column = c;
            this.Occupied = true;
            this.Aisle = false;
            this.AdjacentSeats = new List<(SeatDirection, Seat)>();
        }

        public bool AllAdjacentSeatsUnnoccupied()
        {
            return this.GetAdjacentSeats().All(x => !x.Occupied);
        }

        public bool AllExtendedAdjacentSeatsUnoccupied()
        {
            return this.GetExtendedAdjacentSeats().All(x => !x.Occupied);
        }

        private List<Seat> GetAdjacentSeats()
        {
            return this.AdjacentSeats.Where(s => !s.Item2.Aisle).Select(x => x.Item2).ToList();
        }

        private List<Seat> GetExtendedAdjacentSeats()
        {
            var extendedAdjacentSeats = new List<Seat>();

            foreach (SeatDirection suit in (SeatDirection[])Enum.GetValues(typeof(SeatDirection)))
            {
                var seat = this.GetNextVisibleSeat(suit);
                if (seat != null)
                {
                    extendedAdjacentSeats.Add(seat);
                }
            }

            return extendedAdjacentSeats.Where(s => !s.Aisle).ToList();
        }

        public int AdacentOccupiedCount()
        {
            return this.GetAdjacentSeats().Count(seat => seat.Occupied);
        }

        public int ExtendedOccupiedCount()
        {
            return this.GetExtendedAdjacentSeats().Count(seat => seat.Occupied);
        }

        private Seat GetNextVisibleSeat(SeatDirection dir)
        {
            if (!this.AdjacentSeats.Any(s => s.Item1 == dir))
            {
                return null;
            }

            var seat = this.AdjacentSeats.Single(s => s.Item1 == dir).Item2;

            if (!seat.Aisle)
            {
                return seat;
            }

            return seat.GetNextVisibleSeat(dir);
        }
    }

    enum SeatDirection
    {
        Front,
        FrontRight,
        Right,
        RearRight,
        Rear,
        RearLeft,
        Left,
        FrontLeft
    }
}
