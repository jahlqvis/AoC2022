using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private Rope _rope = new Rope(new Coordinate(0, 0), new Coordinate(0, 0));
        private List<string> _commands = new List<string>();
        public class Rope
        {
            private Coordinate _head { get; set; }
            private Coordinate _tail { get; set; }
            public Rope(Coordinate head, Coordinate tail)
            {
                _head = head;
                _tail = tail;
            }
            private List<Coordinate> _tailPositions = new List<Coordinate>() { new Coordinate(0, 0) };

            public void MoveHead(string direction)
            {
                switch(direction)
                {
                    case "U":
                        _head.Y++;
                        break;
                    case "D":
                        _head.Y--;
                        break;
                    case "R":
                        _head.X++;
                        break;
                    case "L":
                        _head.X--;
                        break;
                    default:
                        throw new ArgumentException("Couldn't parse command");
                }
                MoveTail();
            }

            public List<Coordinate> GetTailPositions()
            {
                return _tailPositions;
            }

            private void MoveTail()
            {
                if (_head == _tail)
                    // if Tail is same place as Head
                    return;

                // 0,2 1,2 2,2
                // 0,1 1,1 2,1
                // 0,0 1,0 2,0

                for(int x=_head.X-1;x<=_head.X+1; x++)
                {
                    for (int y = _head.Y - 1; y <= _head.Y + 1; y++)
                    {
                        if (_tail.X == x && _tail.Y == y)
                            // if Tail is just one tile away 
                            return;
                    }
                }

                // same row
                if (_head.X == _tail.X)
                {
                    if (_head.Y > _tail.Y)
                        _tail.Y++;
                    else
                        _tail.Y--;
                    
                }
                // same column
                else if (_head.Y == _tail.Y)
                {
                    if (_head.X > _tail.X)
                        _tail.X++;
                    else
                        _tail.X--;
                }
                // both row and column different
                else
                {
                    if(_head.X == _tail.X + 2 && _head.Y == _tail.Y + 1)
                    {
                        _tail.X++;
                        _tail.Y++;
                    }
                    else if (_head.X == _tail.X + 2 && _head.Y == _tail.Y - 1)
                    {
                        _tail.X++;
                        _tail.Y--;
                    }
                    else if (_head.X == _tail.X - 2 && _head.Y == _tail.Y + 1)
                    {
                        _tail.X--;
                        _tail.Y++;
                    }
                    else if (_head.X == _tail.X - 2 && _head.Y == _tail.Y - 1)
                    {
                        _tail.X--;
                        _tail.Y--;
                    }
                    else if (_head.X == _tail.X + 1 && _head.Y == _tail.Y + 2)
                    {
                        _tail.X++;
                        _tail.Y++;
                    }
                    else if (_head.X == _tail.X + 1 && _head.Y == _tail.Y - 2)
                    {
                        _tail.X++;
                        _tail.Y--;
                    }
                    else if (_head.X == _tail.X - 1 && _head.Y == _tail.Y + 2)
                    {
                        _tail.X--;
                        _tail.Y++;
                    }
                    else if (_head.X == _tail.X - 1 && _head.Y == _tail.Y - 2)
                    {
                        _tail.X--;
                        _tail.Y--;
                    }
                    else
                    {
                        throw new ArgumentException("Should not reach this state");
                    }
                }

                _tailPositions.Add(new Coordinate(_tail.X, _tail.Y));
                Console.WriteLine($"T: {_tail.X}, {_tail.Y}");
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
    }
}
