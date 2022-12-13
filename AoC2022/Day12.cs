using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AoC2022.Day07;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static AoC2022.Day08;
using System.Collections;

namespace AoC2022
{
    public class Day12
    {
        private int[,] Map;
        private (int, int) Start;
        private List<(int, int)> Starts = new List<(int, int)>();
        private (int, int) End;
        
        public Day12()
        {
        }

        private void Parse(string s)
        {
            var lines = File.ReadAllLines(s);
            Map = new int[lines.Length, lines[0].Length];

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var heightchar = lines[y][x];
                    if (heightchar == 'S')
                    {
                        Start = (x, y);
                    }
                    if(heightchar == 'E')
                    {
                        End = (x, y);
                    }
                    Map[y, x] = ConvertToNumHeight(heightchar);
                }
            }
        }

        private int ConvertToNumHeight(char c)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < alphabet.Count(); i++)
                if (c == alphabet[i])
                    return i;
            
            if (c == 'S')
                return 0;
            if (c == 'E')
                return alphabet.Length-1;

            throw new ArgumentException($"Could not find height for {c}");
        }
        public void RunA()
        {
            var start = DateTime.Now.Microsecond;
            Parse("Input\\Day12.txt");
            
            int R = Map.GetLength(0); // y
            int C = Map.GetLength(1); // x
            int m = R * C;
            Queue<int> rq = new Queue<int>();
            Queue<int> cq = new Queue<int>();
            int moveCount = 0;
            int nodesLeftInLayer = 1;
            int nodesInNextLayer = 0;
            bool reachedEnd = false;
            bool[,] visited = new bool[R, C];

            int[] dr = new int[4] { -1, 1, 0, 0 };
            int[] dc = new int[4] { 0, 0, 1, -1 };

            int sc = Start.Item1;
            int sr = Start.Item2;
            int ec = End.Item1;
            int er = End.Item2;

            void ExploreNeighbours(int r, int c)
            {
                for(int i=0; i<4;i++)
                {
                    var rr = r + dr[i];
                    var cc = c + dc[i];

                    //skip out of bounds
                    if (rr < 0 || cc < 0) continue;
                    if(rr >= R || cc >= C) continue;

                    //skip visited locations or blocked cells
                    if (visited[rr, cc]) continue;
                    if (Map[rr, cc] > Map[r, c] + 1) continue; // height can only be 1 unit higher
                    
                    rq.Enqueue(rr);
                    cq.Enqueue(cc);
                    visited[rr, cc] = true;
                    nodesInNextLayer++;
                }
            }

            int Solve(int sr, int sc, int er, int ec)
            {
                rq.Enqueue(sr); 
                cq.Enqueue(sc);

                while (rq.Count() > 0)
                {
                    var r = rq.Dequeue();
                    var c = cq.Dequeue();
                    if (c == ec && r == er) // where 'E' is 
                    {
                        reachedEnd = true;
                        break;
                    }
                    ExploreNeighbours(r, c);
                    nodesLeftInLayer--;
                    if (nodesLeftInLayer == 0)
                    {
                        nodesLeftInLayer = nodesInNextLayer;
                        nodesInNextLayer = 0;
                        moveCount++;
                    }
                }
                if (reachedEnd)
                {
                    return moveCount;
                }
                else
                {
                    return -1;
                }
            }
            
            void PrintMap()
            {
                Console.WriteLine("");
                for (int y=0; y<Map.GetLength(0); y++)
                {
                    string line = "";
                    for (int x = 0; x < Map.GetLength(1); x++)
                    {
                        line += Map[y, x].ToString("00") + " ";
                    }
                    Console.WriteLine(line); 
                }
                Console.WriteLine("");
            }

            void PrintVisited()
            {
                Console.WriteLine("");
                for (int y = 0; y < visited.GetLength(0); y++)
                {
                    string line = "";
                    for (int x = 0; x < visited.GetLength(1); x++)
                    {
                        line += visited[y, x].ToString() + " ";
                    }
                    Console.WriteLine(line);
                }
                Console.WriteLine("");
            }

            var steps = Solve(sr, sc, er, ec);

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 12A: {steps} {elapsed} μs");
            //PrintMap();
            //PrintVisited();
        }

        private void ParseB(string s)
        {
            var lines = File.ReadAllLines(s);
            Map = new int[lines.Length, lines[0].Length];

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var heightchar = lines[y][x];
                    if (heightchar == 'S')
                    {
                        Starts.Add((x, y));
                    }
                    if (heightchar == 'E')
                    {
                        End = (x, y);
                    }
                    if (heightchar == 'a')
                    {
                        Starts.Add((x, y));
                    }
                    Map[y, x] = ConvertToNumHeight(heightchar);
                }
            }
        }

        public void RunB()
        {
            var start = DateTime.Now.Microsecond;
            ParseB("Input\\Day12.txt");

            int R = Map.GetLength(0); // y
            int C = Map.GetLength(1); // x
            int m = R * C;
            Queue<int> rq = new Queue<int>();
            Queue<int> cq = new Queue<int>();
            int moveCount = 0;
            int nodesLeftInLayer = 1;
            int nodesInNextLayer = 0;
            bool reachedEnd = false;
            bool[,] visited = new bool[R, C];

            int[] dr = new int[4] { -1, 1, 0, 0 };
            int[] dc = new int[4] { 0, 0, 1, -1 };

            int sc = Start.Item1;
            int sr = Start.Item2;
            int ec = End.Item1;
            int er = End.Item2;

            void ExploreNeighbours(int r, int c)
            {
                for (int i = 0; i < 4; i++)
                {
                    var rr = r + dr[i];
                    var cc = c + dc[i];

                    //skip out of bounds
                    if (rr < 0 || cc < 0) continue;
                    if (rr >= R || cc >= C) continue;

                    //skip visited locations or blocked cells
                    if (visited[rr, cc]) continue;
                    if (Map[rr, cc] > Map[r, c] + 1) continue; // height can only be 1 unit higher

                    rq.Enqueue(rr);
                    cq.Enqueue(cc);
                    visited[rr, cc] = true;
                    nodesInNextLayer++;
                }
            }

            int Solve(int sr, int sc, int er, int ec)
            {
                rq.Enqueue(sr);
                cq.Enqueue(sc);

                while (rq.Count() > 0)
                {
                    var r = rq.Dequeue();
                    var c = cq.Dequeue();
                    if (c == ec && r == er) // where 'E' is 
                    {
                        reachedEnd = true;
                        break;
                    }
                    ExploreNeighbours(r, c);
                    nodesLeftInLayer--;
                    if (nodesLeftInLayer == 0)
                    {
                        nodesLeftInLayer = nodesInNextLayer;
                        nodesInNextLayer = 0;
                        moveCount++;
                    }
                }
                if (reachedEnd)
                {
                    return moveCount;
                }
                else
                {
                    return -1;
                }
            }

            void PrintMap()
            {
                Console.WriteLine("");
                for (int y = 0; y < Map.GetLength(0); y++)
                {
                    string line = "";
                    for (int x = 0; x < Map.GetLength(1); x++)
                    {
                        line += Map[y, x].ToString("00") + " ";
                    }
                    Console.WriteLine(line);
                }
                Console.WriteLine("");
            }

            void PrintVisited()
            {
                Console.WriteLine("");
                for (int y = 0; y < visited.GetLength(0); y++)
                {
                    string line = "";
                    for (int x = 0; x < visited.GetLength(1); x++)
                    {
                        line += visited[y, x].ToString() + " ";
                    }
                    Console.WriteLine(line);
                }
                Console.WriteLine("");
            }

            List<int> Steps = new List<int>();
            for (int i = 0; i < Starts.Count; i++)
            {
                reachedEnd= false;
                rq.Clear();
                cq.Clear();
                moveCount = 0;
                nodesLeftInLayer = 1;
                nodesInNextLayer = 0;
                for (int x = 0; x < C; x++)
                    for (int y = 0; y < R; y++)
                        visited[y, x] = false;
                
                int steps = Solve(Starts[i].Item2, Starts[i].Item1, er, ec);
                if(steps != -1)
                {
                    Steps.Add(steps);
                }    
            }
            var OrderedSteps = Steps.OrderBy(x => x).ToList();

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 12B: {OrderedSteps[0]} {elapsed} μs");
            //PrintMap();
            //PrintVisited();
        }
    }
}
