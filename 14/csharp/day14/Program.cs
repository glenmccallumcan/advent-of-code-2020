using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day14
{
    static class Program
    {
        static void Main(string[] args)
        {
            var program = LoadData();
            var part1 = Part1(program);
            var part2 = Part2(program);

            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);
        }

        static ulong Part1(List<MemoryLocation> ml)
        {
            Dictionary<ulong,ulong> memoryValues = new Dictionary<ulong, ulong>();
            foreach(var m in ml)
            {
                if (memoryValues.ContainsKey(m.Address))
                {
                    memoryValues[m.Address] =  m.MaskedValue;
                }
                else
                {
                    memoryValues.Add(m.Address, m.MaskedValue);
                }
            }

            ulong sum = 0;
            foreach (var mem in memoryValues)
            {
                
                sum += mem.Value;
            }

            return sum;
        }

        static ulong Part2(List<MemoryLocation> ml)
        {
            Dictionary<ulong, ulong> memoryValues = new Dictionary<ulong, ulong>();
            foreach (var m in ml)
            {
                foreach(ulong address in m.GetExtendedAddresses())
                {
                    if (memoryValues.ContainsKey(address))
                    {
                        memoryValues[address] = m.Value;
                    }
                    else
                    {
                        memoryValues.Add(address, m.Value);
                    }
                }
            }

            ulong sum = 0;
            foreach (var mem in memoryValues)
            {

                sum += mem.Value;
            }

            return sum;
        }

        public static List<MemoryLocation> LoadData()
        {
            var location = "C:\\Users\\gmccallum\\source\\repos\\advent-of-code-2020\\14\\input.txt";

            string[] lines = File.ReadAllLines(location);

            string bitmask = string.Empty;
            var memoryLocations = new List<MemoryLocation>();

            foreach (var line in lines)
            {
                var input = line.Split(" = ");
                var directive = input[0];
                var value = input[1];


                if (line.StartsWith("mas"))
                {
                    bitmask = value;

                }
                else if (line.StartsWith("mem"))
                {
                    var index = UInt64.Parse(Regex.Matches(line, @"\[(.+?)\]").Select(m => m.Groups[1].Value).First());
                    memoryLocations.Add(new MemoryLocation(index, UInt64.Parse(value), bitmask));
                    
                }
            }

            return memoryLocations;
        }
    }

    

    class MemoryLocation
    {
        public ulong Address { get; }

        public ulong Value { get; }

        public ulong MaskedValue { get; private set; }

        public string BitMask { get; }

        public MemoryLocation(ulong address, ulong value, string bitmask)
        {
            this.Address = address;
            this.Value = value;
            this.BitMask = bitmask;
            this.ApplyBitMask();
        }

        private void ApplyBitMask()
        {
            // convert decimal to 64 bit binary string
            string val = ConvertULongToStringBinary(this.Value, 36);

            // iterate over and apply mask
            var valArray = val.ToArray();
            for (int i = 0; i< valArray.Length;i++)
            {
                if(this.BitMask[i] != 'X')
                {
                    valArray[i] = this.BitMask[i];
                }
            }

            val = new string(valArray);

            // convert 64 bit binary string back to base 10
            this.MaskedValue = Convert.ToUInt64(val, 2);
            return;
        }

        public List<ulong> GetExtendedAddresses()
        {
            // convert address to string binary of length x, first in collection
            List<char[]> addresses = new List<char[]> { ConvertULongToStringBinary(this.Address, 36).ToCharArray() };
            
            //List of 36 character arrays?
            // iterate over apply mask to each string
            for (int i = 0; i < BitMask.Length; i++)
            {
                if (this.BitMask[i] == '1')
                {
                    foreach(var address in addresses)
                    {
                        // copy over for each address
                        address[i] = this.BitMask[i];
                    }
                    

                }else if(this.BitMask[i] == 'X')
                {
                    var newAddresses = new List<char[]>();
                    foreach(var address in addresses)
                    {
                        //duplicate each address and add to list with first containg 0 and duplicate containg 1 at i
                        char[] address1 = new char[address.Length];
                        Array.Copy(address, address1, address.Length);
                        address[i] = '0';
                        address1[i] = '1';
                        newAddresses.Add(address1);
                    }
                    addresses.AddRange(newAddresses);
                }
            }
 
            //return list of new addresses
            return addresses.Select(a=> new String(a)).Select(a => Convert.ToUInt64(a, 2)).ToList();
        }

        private static string ConvertULongToStringBinary(ulong val, int length)
        {
            string output = Convert.ToString((long)val, 2);

            //pad leading zeros
            while (output.Length < length)
            {
                output = "0" + output;
            }

            if (output.Length != length)
            {
                throw new Exception("bitmask doesn't match value length");
            }

            return output;
        }

    }
}
