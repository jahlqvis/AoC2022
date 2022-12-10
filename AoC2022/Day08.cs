using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2022
{
    public class Day08
    {
        public record Tree
        {
            public int Height { get; set; }
            public bool VisibleN { get; set; }
            public bool VisibleE { get; set; }
            public bool VisibleS { get; set; }
            public bool VisibleW { get; set; }
            public bool Visible { get; set; }
            public int VisibleTreesN { get; set; }
            public int VisibleTreesE { get; set; }
            public int VisibleTreesS { get; set; }
            public int VisibleTreesW { get; set; }
            public int VisibleTreesTotal { get; set; }
        }


        public Tree[,] Grid { get; set; }


        public Day08()
        {
            Parse("Input\\Day08.txt");
        }

        void Parse(string s)
        {
            var lines = File.ReadAllLines(s);

            Grid = new Tree[lines[0].Length, lines.Length];

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    Grid[x, y] = new Tree();
                    ReadOnlySpan<char> asSpan = lines[y].AsSpan();
                    Grid[x, y].Height = int.Parse(asSpan.Slice(x, 1));
                    Grid[x, y].VisibleN = false;
                    Grid[x, y].VisibleS = false;
                    Grid[x, y].VisibleE = false;
                    Grid[x, y].VisibleW = false;
                    Grid[x, y].Visible = false;

                }
            }

            //for(int y=0;y< Grid.GetLength(0); y++)
            //{
            //    for(int x=0;x < Grid.GetLength(1); x++)
            //    {
            //        Console.Write(Grid[x, y]);
            //    }
            //    Console.WriteLine("");
            //}
        }

        public void RunA()
        {
            var start = DateTime.Now.Microsecond;

            // Edge

            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    Grid[x, y].VisibleN = y == 0;
                    Grid[x, y].VisibleW = x == 0;
                    Grid[x, y].VisibleE = x == Grid.GetLength(1) - 1;
                    Grid[x, y].VisibleS = y == Grid.GetLength(0) - 1;
                }
            }

            int heighest;

            // North

            for (int x = 1; x < Grid.GetLength(1) - 1; x++)
            {
                heighest = -1;
                for (int y = 0; y < Grid.GetLength(0); y++)
                {
                    Grid[x, y].VisibleN = Grid[x, y].Height > heighest;
                    heighest = Math.Max(heighest, Grid[x, y].Height);
                }
            }


            // West

            for (int y = 1; y < Grid.GetLength(0) - 1; y++)
            {
                heighest = -1;
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    Grid[x, y].VisibleW = Grid[x, y].Height > heighest;
                    heighest = Math.Max(heighest, Grid[x, y].Height);
                }
            }

            // South

            for (int x = 1; x < Grid.GetLength(1) - 1; x++)
            {
                heighest = -1;
                for (int y = Grid.GetLength(0) - 1; y >= 0; y--)
                {
                    Grid[x, y].VisibleS = Grid[x, y].Height > heighest;
                    heighest = Math.Max(heighest, Grid[x, y].Height);
                }
            }

            // East

            for (int y = Grid.GetLength(0) - 2; y > 0; y--)
            {
                heighest = -1;
                for (int x = Grid.GetLength(1) - 1; x >= 0; x--)
                {
                    Grid[x, y].VisibleE = Grid[x, y].Height > heighest;
                    heighest = Math.Max(heighest, Grid[x, y].Height);
                }
            }

            int TreeCounter = 0;
            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    Grid[x, y].Visible = Grid[x, y].VisibleN || Grid[x, y].VisibleS || Grid[x, y].VisibleE || Grid[x, y].VisibleW;
                    if (Grid[x, y].Visible)
                    {
                        TreeCounter++;
                    }
                }
            }
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 08A: {TreeCounter} : {elapsed} μs");
        }

        private int CalcLineOfSight(ref Dictionary<int, int> section, int currPos, int currHeight, int maxLineSight, bool asending = true)
        {
            // heigth = key, position = value
            Dictionary<int, int> sortedDict;
            if (asending)
            {
                sortedDict = section.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                sortedDict = section.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            
            foreach(var kvp in sortedDict)
            {
                if(kvp.Key >= currHeight)
                {
                    if (asending)
                    {
                        return currPos - kvp.Value;
                    }
                    else
                    {
                        return kvp.Value - currPos;
                    }
                }
            }

            if (asending)
            {
                return currPos;
            }
            else
            {
                return maxLineSight - currPos ;
            }
        }

        public void RunB()
        {
            var start = DateTime.Now.Microsecond;
            Dictionary<int, int> section = new Dictionary<int, int>();

            // North
            
            for (int x = 0; x <= Grid.GetLength(1) - 1; x++)
            {
                section.Clear();
                for (int y = 0; y < Grid.GetLength(0); y++)
                {
                    Grid[x, y].VisibleTreesN = CalcLineOfSight(ref section, y, Grid[x, y].Height, Grid.GetLength(0));
                    section[Grid[x, y].Height] = y;
                }
            }

            // West
            for (int y = 0; y <= Grid.GetLength(0) - 1; y++)
            {
                section.Clear();
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    Grid[x, y].VisibleTreesW = CalcLineOfSight(ref section, x, Grid[x, y].Height, Grid.GetLength(1));
                    section[Grid[x, y].Height] = x;
                }
            }

            // South
            for (int x = 0; x <= Grid.GetLength(1) - 1; x++)
            {
                section.Clear();
                for (int y = Grid.GetLength(0) - 1; y >= 0; y--)
                {
                    Grid[x, y].VisibleTreesS = CalcLineOfSight(ref section, y, Grid[x, y].Height, Grid.GetLength(0)-1, false);
                    section[Grid[x, y].Height] = y;
                }
            }

            // East
            for (int y = Grid.GetLength(0) - 1; y >= 0; y--)
            {
                section.Clear();
                for (int x = Grid.GetLength(1) - 1; x >= 0; x--)
                {
                    Grid[x, y].VisibleTreesE = CalcLineOfSight(ref section, x, Grid[x, y].Height, Grid.GetLength(1) - 1, false);
                    section[Grid[x, y].Height] = x;
                }
            }

            int HighestScoreTracker = 0;
            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    Grid[x, y].VisibleTreesTotal =  Grid[x, y].VisibleTreesN * 
                                                    Grid[x, y].VisibleTreesE * 
                                                    Grid[x, y].VisibleTreesS * 
                                                    Grid[x, y].VisibleTreesW;

                    if (Grid[x, y].VisibleTreesTotal > HighestScoreTracker)
                        HighestScoreTracker = Grid[x, y].VisibleTreesTotal;
                }
            }
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 08B: {HighestScoreTracker} : {elapsed} μs");
        }
    }
}
