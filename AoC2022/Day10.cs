using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    public class Day10
    {
        public Day10()
        {

        }

        private Stack<string> _code = new Stack<string>();
        private int _registerX = 1;
        private int _cycle = 0;
        private void Parse(string s)
        {
            var lines = File.ReadAllLines(s);
            for(int i=lines.Length-1; i>=0; i--)
            {
                _code.Push(lines[i]);
            }
        }

        public void RunA()
        {
            int worker=0;
            int signalstrengthSum =0;
            var start = DateTime.Now.Microsecond;
            Parse("Input\\Day10.txt");
            string currentCommand = "";
            // CPU
            for (int cycle=1; cycle<=220 ; cycle++) 
            {
                if (worker == 0)
                {
                    // Finnish up previous command
                    if (currentCommand.Split(' ')[0] == "addx")
                    {
                        _registerX += int.Parse(currentCommand.Split(" ")[1]);
                    }

                    // Read new command
                    currentCommand = _code.Pop();
                    if (currentCommand.Split(' ')[0] == "noop")
                    {
                        // Do nothing
                        worker = 0;
                        
                    }
                    else if (currentCommand.Split(' ')[0] == "addx")
                    {
                        // Wait one cycle
                        worker = 1;
                    }
                }
                else
                {
                    // Pretend to work
                    worker--;
                }

                if(cycle == 20 || (cycle+20) % 40 == 0)
                {
                    signalstrengthSum += cycle * _registerX;
                    Console.WriteLine($"Cycle: {cycle} Register X: {_registerX} Signalstrength: {cycle*_registerX}");
                }
            }

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 10A: {signalstrengthSum} : {elapsed} μs");
        }
    }
}
