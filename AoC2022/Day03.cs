using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    public class Day03
    {
        public void RunA()
        {
            var lines = File.ReadAllLines("Input\\Day03.txt");
            
            List<string> compA = new List<string>();
            List<string> compB = new List<string>();

            var start = DateTime.Now.Microsecond;

            int totalPoints = 0;
            foreach(var line in lines)
            {
                var halfize = line.Count() / 2;
                var A = line.Substring(0, halfize);
                var B = line.Substring(halfize, halfize);
                char error = '0';
                int points = 0;
                foreach(char ca in A)
                {
                    error = '0';
                    foreach (char cb in B)
                    {
                        if(ca == cb)
                        {
                            error = ca;
                            break;
                        }
                    }
                    if(error != '0')
                    {
                        points = getPriority(error);
                        break;
                    }
                }
                totalPoints += points;
            }

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 03A: {totalPoints} : {elapsed} μs");
        }

        int getPriority(char ca)
        {
            string priorityTable = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int i = 1;
            foreach(char cb in priorityTable)
            { 
                if(ca == cb)
                {
                    return i;
                }
                i++;
            }
            return 0;
        }

        public void RunB()
        {
            var lines = File.ReadAllLines("Input\\Day03.txt");
            var start = DateTime.Now.Microsecond;
            int totalPoints = 0;

            for (int i = 0; i < lines.Count(); i=i+3)
            {
                var A = lines[i];
                var B = lines[i+1];
                var C = lines[i+2];
                int points = 0;

                foreach(char c in A)
                {
                    if(B.Contains(c) && C.Contains(c))
                    {
                        points = getPriority(c);
                        break;
                    }
                }
                totalPoints += points;
            }

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 03B: {totalPoints} : {elapsed} μs");
        }
    }
}
