using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    public class Day02
    {
        public List<string> OpponentShapes = new List<string>();
        public List<string> Result = new List<string>();
        public List<string> MyShapes = new List<string>();
        private const string Rock = "A";
        private const string Paper = "B";
        private const string Scissor = "C";
        private const string Lose = "X";
        private const string Draw = "Y";
        private const string Win = "Z";

        public Day02()
        {
            
        }

        public void Parse(string input)
        {
            var lines = File.ReadAllLines(input);

            foreach (var line in lines)
            {
                var shapes = line.Split(" ");
                switch (shapes[1])
                {
                    case "X":
                        MyShapes.Add(Rock);
                        break;
                    case "Y":
                        MyShapes.Add(Paper);
                        break;
                    case "Z":
                        MyShapes.Add(Scissor);
                        break;
                }
                Result.Add(shapes[1]);
                OpponentShapes.Add(shapes[0]);
            }
        }

        public void RunB()
        {
            var start = DateTime.Now.Microsecond;
            int totalScore = 0;
            for (int i = 0; i < MyShapes.Count; i++)
            {
                int score = 0;
                switch (Result[i])
                {
                    case Win:
                        score += OpponentShapes[i] switch
                        {
                            Rock => 2,
                            Paper => 3,
                            Scissor => 1
                        };
                        score += 6;
                        break;
                    case Draw:
                        score += OpponentShapes[i] switch
                        {
                            Rock => 1,
                            Paper => 2,
                            Scissor => 3
                        };
                        score += 3;
                        break;
                    case Lose:
                        score += OpponentShapes[i] switch
                        {
                            Rock => 3,
                            Paper => 1,
                            Scissor => 2
                        };
                        score += 0;
                        break;
                }

                totalScore += score;
            }

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 02B: {totalScore} : {elapsed} μs");
        }

        public void RunA()
        {
            var start = DateTime.Now.Microsecond;
            int totalScore = 0;
            for (int i = 0; i < MyShapes.Count; i++)
            {
                int score = 0;
                switch (MyShapes[i])
                {
                    case Rock:
                        score += OpponentShapes[i] switch
                        {
                            Rock => 3,
                            Paper => 0,
                            Scissor => 6
                        };
                        score += 1;
                        break;
                    case Paper:
                        score += OpponentShapes[i] switch
                        {
                            Rock => 6,
                            Paper => 3,
                            Scissor => 0
                        };
                        score += 2;
                        break;
                    case Scissor:
                        score += OpponentShapes[i] switch
                        {
                            Rock => 0,
                            Paper => 6,
                            Scissor => 3
                        };
                        score += 3;
                        break;
                }

                totalScore += score;
            }

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 02A: {totalScore} : {elapsed} μs");
        }
    }
}
