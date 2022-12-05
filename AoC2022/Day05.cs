using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2022
{
    public class Day05
    {
        List<Stack<char>> Stacks = new List<Stack<char>>();
        
        public Day05() 
        {
            
        }

        public void Parse(string s, Action<int, int, int> move) 
        {
            var lines = File.ReadAllLines(s);
            Regex r = new Regex("^[A-Z]");
            List<string> part1 = new List<string>();
            List<string> part2 = new List<string>();

            int state = 0;

            foreach (var line in lines)
            {
                
                if (state == 0 && line != "")
                {
                    part1.Add(line);
                }
                else if (state == 0 && line == "")
                {
                    //[N] [C]    
                    //[Z] [M] [P]
                    // 1   2   3
                    var numbers = Regex.Split(part1.Last(), @"\D+");
                    var numStacks = numbers.Length-2;
                    while(numStacks > 0)
                    {
                        Stacks.Add(new Stack<char>());
                        numStacks--;
                    }
                    
                    for(int i=part1.Count-2; i>=0; i--)
                    {
                        var str = part1[i];
                        for(int j=1, n=0; j<str.Length; j=j+4, n++)
                        {
                            if (char.IsLetter(str[j]))
                            {
                                Stacks[n].Push(str[j]);
                            }
                        }
                    }

                    state = 1;
                } 
                else if (state == 1)
                {
                    // Split on one or more non-digit characters.
                    string[] numbers = Regex.Split(line, @"\D+");
                    
                    var num = int.Parse(numbers[1]);
                    var from = int.Parse(numbers[2]);
                    var to = int.Parse(numbers[3]);
                    move(num, from, to);
                }
            }

        }

        public void RunA() 
        {
            var start = DateTime.Now.Microsecond;
            void MoveA(int num, int from, int to)
            {
                // move 1 from 2 to 1
                for (int i = 1; i <= num; i++)
                {
                    Stacks[to - 1].Push(Stacks[from - 1].Pop());
                }
            }

            Parse("Input\\Day05.txt", MoveA);
            var elapsed = DateTime.Now.Microsecond - start;

            string getTopChars()
            {
                string result = String.Empty;
                foreach(var s in Stacks)
                {
                    result += s.Peek();
                }
                return result;
            }
            
            Console.WriteLine($"Day 05A: {getTopChars()} : {elapsed} μs");
        }
        public void RunB()
        {
            Stacks.Clear();
            var start = DateTime.Now.Microsecond;
            void MoveB(int num, int from, int to)
            {
                // move 1 from 2 to 1

                List<char> temp = new List<char>();
                for (int i = 1; i <= num; i++)
                {
                    temp.Add(Stacks[from - 1].Pop());
                }

                for (int i = num-1; i >= 0; i--)
                {
                    Stacks[to - 1].Push(temp[i]);
                }
            }

            Parse("Input\\Day05.txt", MoveB);
            var elapsed = DateTime.Now.Microsecond - start;

            string getTopChars()
            {
                string result = String.Empty;
                foreach (var s in Stacks)
                {
                    result += s.Peek();
                }
                return result;
            }

            Console.WriteLine($"Day 05B: {getTopChars()} : {elapsed} μs");
        }
    }
}
