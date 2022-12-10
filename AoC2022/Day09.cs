using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static AoC2022.Day09;

namespace AoC2022
{
    public class Day09
    {
        public Day09() { }
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Coordinate(int x, int y) 
            {
                X = x;
                Y = y;
            }
        }
        public class CoordinateEqualityComparer : IEqualityComparer<Coordinate>
        {
            public bool Equals(Coordinate a, Coordinate b)
            {
                return a.X == b.X && a.Y == b.Y;
            }

            public int GetHashCode(Coordinate obj)
            {
                https://stackoverflow.com/questions/5221396/what-is-an-appropriate-gethashcode-algorithm-for-a-2d-point-struct-avoiding
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    // Suitable nullity checks etc, of course :)
                    hash = hash * 23 + obj.X.GetHashCode();
                    hash = hash * 23 + obj.Y.GetHashCode();
                    return hash;
                }
            }
        }

        private Rope _rope;
        private List<string> _commands = new List<string>();
        public class Rope
        {
            List<Coordinate> _knots= new List<Coordinate>();
            
            public Rope(int numKnots)
            {
                for(int i = 0;i< numKnots; i++)
                {
                    _knots.Add(new Coordinate(0,0));
                }
            }

            public Rope(Coordinate head, Coordinate tail)
            {
                _knots.Add(tail);
                _knots.Add(head);
            }

            private List<Coordinate> _tailPositions = new List<Coordinate>() { new Coordinate(0, 0) };

            public void MoveHead(string direction)
            {
                switch (direction)
                {
                    case "U":
                        _knots[0].Y++;
                        break;
                    case "D":
                        _knots[0].Y--;
                        break;
                    case "R":
                        _knots[0].X++;
                        break;
                    case "L":
                        _knots[0].X--;
                        break;
                    default:
                        throw new ArgumentException("Couldn't parse command");
                }
                for (int i = 0; i <= _knots.Count - 2; i++)
                {
                    var prev = _knots[i];
                    var next = _knots[i + 1];
                    MoveKnots(ref prev, ref next);
                    //_knots[i] = prev;
                    //_knots[i + 1] = next;

                    if (i == _knots.Count - 2)
                    {
                        //Console.WriteLine($"T: {_knots[i+1].X}, {_knots[i+1].Y}");
                        _tailPositions.Add(new Coordinate(_knots[i + 1].X, _knots[i + 1].Y));
                    }
                }
            }
            public List<Coordinate> GetTailPositions()
            {
                return _tailPositions;
            }

            private void MoveKnots(ref Coordinate prev, ref Coordinate next)
            {
                if (prev == next)
                    // if Tail is same place as Head
                    return;

                // 0,2 1,2 2,2
                // 0,1 1,1 2,1
                // 0,0 1,0 2,0

                bool VicinityCheck(ref Coordinate prev, ref Coordinate next)
                {
                    for (int x = prev.X - 1; x <= prev.X + 1; x++)
                    {
                        for (int y = prev.Y - 1; y <= prev.Y + 1; y++)
                        {
                            if (next.X == x && next.Y == y)
                                return true;
                        }
                    }
                    return false;
                }
                
                while(!VicinityCheck(ref prev, ref next))
                {
                    if(prev.X > next.X)
                    {
                        next.X++;
                    }
                    else if(prev.X < next.X)
                    {
                        next.X--;
                    }
                    else
                    {
                        // do nothing
                    }

                    if(prev.Y > next.Y)
                    {
                        next.Y++;
                    }
                    else if(prev.Y < next.Y)
                    {
                        next.Y--;
                    }
                    else
                    {
                        // do nothing
                    }
                }

                return;
            }
        }

        private void Parse(string s)
        {
            var lines = File.ReadAllLines(s);
            foreach (var line in lines)
            {
                _commands.Add(line);
            }
        }

        public void RunA()
        {
            var start = DateTime.Now.Microsecond;
            _rope = new Rope(new Coordinate(0, 0), new Coordinate(0, 0));
            Parse("Input\\Day09.txt");
            foreach(var cmd in _commands)
            {
                var temp = cmd.Split(" ");
                for(int n = 1; n <= int.Parse(temp[1]);n++)
                {
                    _rope.MoveHead(temp[0]);
                }
            }

            var tailCoords = _rope.GetTailPositions();
            var comparer = new CoordinateEqualityComparer();
            var uniqueCoords = tailCoords.Distinct(comparer);
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 09A: {uniqueCoords.Count()} : {elapsed} μs");
        }

        public void RunB()
        {
            var start = DateTime.Now.Microsecond;
            _rope = new Rope(10);
            Parse("Input\\Day09.txt");
            foreach (var cmd in _commands)
            {
                var temp = cmd.Split(" ");
                for (int n = 1; n <= int.Parse(temp[1]); n++)
                {
                    _rope.MoveHead(temp[0]);
                }
            }

            var tailCoords = _rope.GetTailPositions();
            var comparer = new CoordinateEqualityComparer();
            var uniqueCoords = tailCoords.Distinct(comparer);
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 09B: {uniqueCoords.Count()} : {elapsed} μs");
        }
    }
}
