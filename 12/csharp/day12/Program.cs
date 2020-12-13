using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day12
{
    class Program
    {
        private static readonly List<Char> compassDirections = new List<char> { 'N', 'E', 'S', 'W' };
        private static readonly List<Char> rotationDirections = new List<char> { 'R', 'L' };

        public static void Main(string[] args)
        {
            var directions = LoadData();
            var part1 = Part1(directions);
            var part2 = Part2(directions);

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);

            // Part 1: 2297
            // Part 2: 89984
        }

        static int Part1(List<Command> directions)
        {
            Ship ship = new Ship(0, 0, 'E');

            foreach (var command in directions)
            {
                if (compassDirections.Contains(command.Letter))
                {
                    ship.MoveShip(command.Letter, command.Value);

                }
                else if (rotationDirections.Contains(command.Letter))
                {
                    ship.ChangeDirection(command.Letter, command.Value);

                }
                else if (command.Letter == 'F')
                {
                    ship.MoveForward(command.Value);
                }
                else
                {
                    throw new NotImplementedException("Command Letter: " + command.Letter);
                }


            }
            return Math.Abs(ship.ShipX) + Math.Abs(ship.ShipY);
        }

        static int Part2(List<Command> directions)
        {
            Ship ship = new Ship(0, 0, 10, 1);

            foreach (var command in directions)
            {
                if (compassDirections.Contains(command.Letter))
                {
                    ship.MoveWaypoint(command.Letter, command.Value);

                }
                else if (rotationDirections.Contains(command.Letter))
                {
                    ship.RotateWaypoint(command.Letter, command.Value);

                }
                else if (command.Letter == 'F')
                {
                    ship.MoveShipToWaypoint(command.Value);
                }
                else
                {
                    throw new NotImplementedException("Command Letter: " + command.Letter);
                }


            }
            return Math.Abs(ship.ShipX) + Math.Abs(ship.ShipY);
        }

        static List<Command> LoadData()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\12\\input.txt";

            string[] lines = File.ReadAllLines(location);

            return lines.Select(command => new Command(command[0], Int32.Parse(command[1..]))).ToList();
        }
    }

    class Ship
    {
        public int ShipX { get; private set; }

        public int ShipY { get; private set; }

        public int WaypointOffsetX { get; private set; }

        public int WaypointOffsetY { get; private set; }

        public char CurrentDirection { get; private set; }

        public Ship(int x, int y, int wx, int wy)
        {
            this.ShipX = x;
            this.ShipY = y;
            this.WaypointOffsetX = wx;
            this.WaypointOffsetY = wy;
        }

        public Ship(int x, int y, char dir)
        {
            this.ShipX = x;
            this.ShipY = y;
            this.WaypointOffsetX = 0;
            this.WaypointOffsetY = 0;
            this.CurrentDirection = dir;
        }

        public void MoveShip(char letter, int amount)
        {
            switch (letter)
            {
                case 'N':
                    this.ShipY += amount;
                    break;
                case 'E':
                    this.ShipX += amount;
                    break;
                case 'S':
                    this.ShipY -= amount;
                    break;
                case 'W':
                    this.ShipX -= amount;
                    break;

            }
            return;
        }

        public void MoveForward(int amount)
        {
            this.MoveShip(this.CurrentDirection, amount);
        }

        public void MoveShipToWaypoint(int amount)
        {
            this.ShipX += amount * this.WaypointOffsetX;
            this.ShipY += amount * this.WaypointOffsetY;
        }

        public void MoveWaypoint(char direction, int amount)
        {
            switch (direction)
            {
                case 'N':
                    this.WaypointOffsetY += amount;
                    break;
                case 'E':
                    this.WaypointOffsetX += amount;
                    break;
                case 'S':
                    this.WaypointOffsetY -= amount;
                    break;
                case 'W':
                    this.WaypointOffsetX -= amount;
                    break;
            }
        }

        public void RotateWaypoint(char direction, int amount)
        {
            int temp;
            switch (direction)
            {
                case 'R':
                    switch (amount)
                    {
                        case 90:
                            temp = this.WaypointOffsetX;
                            this.WaypointOffsetX = this.WaypointOffsetY;
                            this.WaypointOffsetY = (-1) * temp;
                            break;
                        case 180:
                            this.WaypointOffsetY = (-1) * this.WaypointOffsetY;
                            this.WaypointOffsetX = (-1) * this.WaypointOffsetX;
                            break;
                        case 270:
                            temp = this.WaypointOffsetX;
                            this.WaypointOffsetX = (-1) * this.WaypointOffsetY;
                            this.WaypointOffsetY = temp;
                            break;
                    }
                    break;
                case 'L':
                    switch (amount)
                    {
                        case 90:
                            temp = this.WaypointOffsetX;
                            this.WaypointOffsetX = (-1) * this.WaypointOffsetY;
                            this.WaypointOffsetY = temp;
                            break;
                        case 180:
                            this.WaypointOffsetY = (-1) * this.WaypointOffsetY;
                            this.WaypointOffsetX = (-1) * this.WaypointOffsetX;
                            break;
                        case 270:
                            temp = this.WaypointOffsetX;
                            this.WaypointOffsetX = this.WaypointOffsetY;
                            this.WaypointOffsetY = (-1) * temp;
                            break;
                    }
                    break;
            }
        }

        public void ChangeDirection(char letter, int degrees)
        {
            switch (letter)
            {
                case 'R':
                    switch (this.CurrentDirection)
                    {
                        case 'N':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'E';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'S';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'W';
                                    break;
                            }
                            break;
                        case 'E':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'S';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'W';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'N';
                                    break;
                            }
                            break;
                        case 'S':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'W';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'N';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'E';
                                    break;
                            }
                            break;
                        case 'W':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'N';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'E';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'S';
                                    break;
                            }
                            break;
                    }
                    break;
                case 'L':
                    switch (this.CurrentDirection)
                    {
                        case 'N':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'W';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'S';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'E';
                                    break;
                            }
                            break;
                        case 'E':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'N';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'W';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'S';
                                    break;
                            }
                            break;
                        case 'S':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'E';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'N';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'W';
                                    break;
                            }
                            break;
                        case 'W':
                            switch (degrees)
                            {
                                case 90:
                                    this.CurrentDirection = 'S';
                                    break;
                                case 180:
                                    this.CurrentDirection = 'E';
                                    break;
                                case 270:
                                    this.CurrentDirection = 'N';
                                    break;
                            }
                            break;
                    }
                    break;
            }
            return;

        }
    }



    class Command
    {
        public char Letter { get; }

        public int Value { get; }

        public Command(char let, int dist)
        {
            this.Letter = let;
            this.Value = dist;
        }
    }
}
