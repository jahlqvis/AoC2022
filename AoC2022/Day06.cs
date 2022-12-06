using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    public class Day06
    {
        public string Input;

        public Day06()
        {
            Parse("Input\\Day06.txt");
        }

        void Parse(string s, int line = 0)
        {
            if(line == 0) 
            {
                Input = File.ReadAllText(s);
            }
            else
            {
                Input = File.ReadLines(s).Skip(line-1).Take(1).First();
            }
        }

        public void RunA()
        {
            var start = DateTime.Now.Microsecond;
            int pos=-1;

            char[] stump = new char[4];

            for (int i = 0; i < Input.Count(); i++)
            {
                stump[3] = stump[2];
                stump[2] = stump[1];
                stump[1] = stump[0];
                stump[0] = Input[i];

                if (i > 3 && stump.Distinct().Count() == 4)
                {
                    // +1 because index elves use start with 1
                    pos = i+1;
                    break;
                }
            }
            
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 06A: {pos} : {elapsed} μs");
        }

        public void RunB()
        {
            var start = DateTime.Now.Microsecond;
            int pos = -1;

            char[] stump = new char[14];

            for (int i = 0; i < Input.Count(); i++)
            {
                for(int j=13;j>0;j--)
                {
                    stump[j] = stump[j-1];
                }
                stump[0] = Input[i];

                if (i > 3 && stump.Distinct().Count() == 14)
                {
                    // +1 because index elves use start with 1
                    pos = i + 1;
                    break;
                }
            }

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 06B: {pos} : {elapsed} μs");
        }
    }
}
