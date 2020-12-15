using System;
using System.Collections.Generic;

namespace day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new int[] { 18, 8, 0, 5, 4, 1, 20 };
            var result = PlayGame(input, 2020);
            Console.WriteLine("Part 1: " + result);
            result = PlayGame(input, 30000000);
            Console.WriteLine("Part 2: " + result);

            // Part 1: 253
            // Part 2: 13710
        }

        static int PlayGame(int[] startingNumbers, int iterations)
        {
            var trackers = new Dictionary<int, Tracker>();
            int turnCounter = 1;
            int lastNumSpoken = -1;
            
            foreach(var num in startingNumbers)
            {
                trackers.Add(num, new Tracker(turnCounter));
                lastNumSpoken = num;
                turnCounter++;
            }

            while(turnCounter <= iterations)
            {
                if(trackers[lastNumSpoken].BeforePreviousTurnNumber == 0)
                {
                    lastNumSpoken = 0;
                    
                }else
                {
                    lastNumSpoken = trackers[lastNumSpoken].PreviousTurnNumber - trackers[lastNumSpoken].BeforePreviousTurnNumber;
                }

                if(!trackers.ContainsKey(lastNumSpoken))
                {
                    trackers.Add(lastNumSpoken, new Tracker(turnCounter));
                }else
                {
                    trackers[lastNumSpoken].Update(turnCounter);
                }
                
                //Console.WriteLine("Last Num Spoken: " + lastNumSpoken + " Turn Number: " + turnCounter);
                turnCounter++;
            }

            return lastNumSpoken;
        }
    }

    class Tracker
    {
        public int PreviousTurnNumber { get; private set; }

        public int BeforePreviousTurnNumber { get; private set; }

        public Tracker(int ptn)
        {
            this.PreviousTurnNumber = ptn;
            this.BeforePreviousTurnNumber = 0;
        }

        public void Update(int turnCount)
        {
            this.BeforePreviousTurnNumber = this.PreviousTurnNumber;
            this.PreviousTurnNumber = turnCount;
        }
    }
}
