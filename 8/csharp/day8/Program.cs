using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day8
{
    static class Program
    {
        static void Main(string[] args)
        {
            var bootCode = LoadBootCode();
            var part1 = FindAccValueBeforeInfiniteLoop(bootCode.ToArray());
            var part2 = FixAndGetAnswer(bootCode.ToArray());
            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);
            // Part 1: 1087
            // Part 2: 780
        }

        static int FixAndGetAnswer(Instruction[] instructions)
        {
            var map = ComesFromMap(instructions);
            var locationsThatFinish = new HashSet<int>();
            GetAllLinesThatLeadToFinish(map, instructions.Length, locationsThatFinish);

            int location = 0;
            int acc = 0;
            bool repaired = false;
            while (location < instructions.Length)
            {
                if (!repaired && (instructions[location].Command == "nop" || instructions[location].Command == "jmp"))
                {
                    var alteredCommand = instructions[location].Command == "nop" ? "jmp" : "nop";

                    int possibleNextLocation = 0;
                    switch (alteredCommand)
                    {
                        case "nop":
                            possibleNextLocation = location + 1;
                            break;
                        case "jmp":
                            possibleNextLocation = location + instructions[location].Value;
                            break;
                    }

                    if (locationsThatFinish.Contains(possibleNextLocation))
                    {
                        location = possibleNextLocation;
                        repaired = true;
                        continue;
                    }
                }

                acc += instructions[location].GetNextAccumulatorValue();
                location = instructions[location].GetNextInstructionLocation();
            }

            return acc;
        }

        static void GetAllLinesThatLeadToFinish(Dictionary<int, List<int>> comesFromMap, int location, HashSet<int> success)
        {
            success.Add(location);

            if (!comesFromMap.TryGetValue(location, out List<int> incomingNodes) || !incomingNodes.Any())
            {
                return;
            }

            foreach (var node in incomingNodes)
            {
                GetAllLinesThatLeadToFinish(comesFromMap, node, success);
            }
        }

        static Dictionary<int, List<int>> ComesFromMap(Instruction[] instructions)
        {
            var map = new Dictionary<int, List<int>>();

            foreach (var inst in instructions)
            {
                map.EnsureExists(inst.GetNextInstructionLocation()).TryGetValue(inst.GetNextInstructionLocation(), out List<int> comesFromLocations);
                comesFromLocations.Add(inst.Location);
            }

            return map;
        }

        static Dictionary<int, List<int>> EnsureExists(
        this Dictionary<int, List<int>> dict,
        int value)
        {
            if (!dict.ContainsKey(value))
            {
                dict.Add(value, new List<int>());
            }

            return dict;
        }

        static int FindAccValueBeforeInfiniteLoop(Instruction[] instructions)
        {
            int[,] visited = new int[instructions.Length, 2];
            int acc = 0;
            int location = 0;

            while (visited[location, 1] == 0)
            {
                visited[location, 1] = 1;
                switch (instructions[location].Command)
                {
                    case "acc":
                        acc += instructions[location].Value;
                        location++;
                        break;
                    case "nop":
                        location++;
                        break;
                    case "jmp":
                        location += instructions[location].Value;
                        break;
                }

            }

            return acc;
        }

        static List<Instruction> LoadBootCode()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\8\\input.txt";

            string[] lines = File.ReadAllLines(location);
            List<Instruction> instructions = new List<Instruction>();
            int lineNum = 0;

            foreach (var line in lines)
            {
                instructions.Add(new Instruction(lineNum, line.Substring(0, 3), Int32.Parse(line.Substring(3, line.Length - 3))));
                lineNum++;
            }

            return instructions;
        }
    }

    class Instruction
    {
        public string Command { get; }

        public int Location { get; }
        public int Value { get; }

        public Instruction(int loc, string com, int val)
        {
            this.Location = loc;
            this.Command = com;
            this.Value = val;
        }

        public int GetNextInstructionOffset()
        {
            int offset = 0;
            switch (this.Command)
            {
                case "acc":
                case "nop":
                    offset = 1;
                    break;
                case "jmp":
                    offset = this.Value;
                    break;
            }
            return offset;
        }

        public int GetNextInstructionLocation()
        {
            return this.Location + this.GetNextInstructionOffset();
        }

        public int GetNextAccumulatorValue()
        {
            if (this.Command != "acc")
            {
                return 0;
            }
            return this.Value;
        }
    }
}
