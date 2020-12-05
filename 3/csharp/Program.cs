using System;
using System.IO;
using System.Numerics;

namespace day3
{
    class Program
    {
        static int countTrees (int right, int down, char[,] forest)
        {
            int treeCounter = 0;
            int rows = forest.GetLength(0);
            int columns = forest.GetLength(1);

            int j = 0;
            for (int i = down; i < rows; i += down)
            {
                j += right;
                if (forest[i, j % columns] == '#')
                {
                    treeCounter++;
                }
            }

            return treeCounter;
        }

        static void Main(string[] args)
        {
            var forest = CreateForest();

            BigInteger part1_2b = countTrees(3, 1, forest);

            BigInteger part2a = countTrees(1, 1, forest);
            BigInteger part2c = countTrees(5, 1, forest);
            BigInteger part2d = countTrees(7, 1, forest);
            BigInteger part2e = countTrees(1, 2, forest);

            var part2 = part1_2b * part2a * part2c * part2d * part2e;
            
            Console.WriteLine("Part 1: " + part1_2b);
            Console.WriteLine("Part 2: " + part2);

            // Part 1: 284
            // Part 2: 3510149120
        }

        static char[,] CreateForest()
        {

            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\3\\input.txt";

            string[] lines = File.ReadAllLines(location);

            int numRows = lines.Length;
            int numColums = lines[0].Length;

            var forest = new char[numRows,numColums];

            for(int i = 0; i < numRows; i++)
            {
                var line = lines[i].ToCharArray();
                for (int j = 0; j < numColums; j++)
                {
                    forest[i, j] = line[j];
                }
            }
            return forest;
        }
    }
}
