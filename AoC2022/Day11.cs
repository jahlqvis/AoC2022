using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2022
{
    public class Day11
    {
        public List<Monkey> MonkeyList = new List<Monkey>();

        public class Monkey
        {
            private List<int> _items;
            private int _id;
            private eOperation _operation;
            private int _testDivisor;
            private int _operationData;
            private int _testTrueIndex;
            private int _testFalseIndex;
            private int _worryDivisor = 1;
            private long _inspectionsCount = 0;

            public List<int> Items {  get { return _items; }  set { _items = value; } }
            public int Id { get => _id; set => _id = value; }
            public eOperation Operation { get => _operation; set => _operation = value; }
            public int TestDivisor { get => _testDivisor; set => _testDivisor = value; }
            public int OperationData { get => _operationData; set => _operationData = value; }
            public int TestTrueIndex { get => _testTrueIndex; set => _testTrueIndex = value; }
            public int TestFalseIndex { get => _testFalseIndex; set => _testFalseIndex = value; }
            public int WorryDivisor { get => _worryDivisor; set => _worryDivisor = value; }
            public long InspectionsCount { get => _inspectionsCount; set => _inspectionsCount = value; }

            public enum eOperation
            {
                Unset = 0,
                Add = 1,
                Multiply = 2,
                Square = 3
            };
            public Monkey()
            {
                _items = new List<int>();
            }
            public Monkey(int id, eOperation o, int opdata, int testdiv, int testtrueindex, int testfalseindex, List<int> items)
            {
                this.Id = id;
                for (int i = 0; i < items.Count; i++)
                {
                    _items.Add(items[i]);
                }
                this.Operation = o;
                this.TestDivisor = testdiv;
                this.TestTrueIndex= testtrueindex;
                this.TestFalseIndex= testfalseindex;
            }

            public override string ToString()
            {
                string str="";
                foreach(var i in _items)
                {
                    str += i.ToString();
                    str += " ";
                }
                return str;
            }
            public bool RunOperation(int i)
            {
                switch (Operation)
                {
                    case eOperation.Add:
                        _items[i] = _items[i] + OperationData;
                        break;
                    case eOperation.Multiply:
                        _items[i] = _items[i] * OperationData;
                        break;
                    case eOperation.Square:
                        _items[i] = _items[i] * _items[i];
                        break;
                    default:
                        throw new ArgumentException("Unknown operation");
                }
                _inspectionsCount++;

                //_items[i] = (int)_items[i] / WorryDivisor;
                return true;
            }

            public int Test(int index)
            {
                if (_items[index] % TestDivisor == 0)
                    return TestTrueIndex;
                else
                    return TestFalseIndex;
            }
        }

        public Day11()
        {
        }

        private void Parse(string s)
        {
            var lines = File.ReadAllLines(s);

            Monkey monkey=null;
            int row = 0;
            List<int> items = new List<int>();
            Monkey.eOperation op=Monkey.eOperation.Unset;
            foreach (var line in lines)
            {
                if(line == string.Empty) continue;
                
                if(row == 0)
                {
                    //Monkey 0:
                    Regex pattern = new Regex(@"Monkey (?<index>\d+):");
                    Match match = pattern.Match(line);
                    monkey = new Monkey();
                    monkey.Id = int.Parse(match.Groups["index"].Value);
                    row = 1;
                }
                else if(row == 1) 
                {
                    //  Starting items: 79, 98
                    Regex pattern = new Regex(@"  Starting items: (?<itemstr>.+)");
                    Match match = pattern.Match(line);
                    var str = match.Groups["itemstr"].ToString();
                    foreach(var e in str.Split(", "))
                    {
                        monkey.Items.Add(int.Parse(e));
                    }
                    row = 2;
                }
                else if (row == 2)
                {
                    //  Operation: new = old * 19
                    Regex pattern = new Regex(@"  Operation: new = old (?<opstr>.+)");
                    Match match = pattern.Match(line);
                    var str = match.Groups["opstr"].ToString().Split(" ");
                    if (str.Length != 2)
                    {
                        throw new ArgumentException("Parsing monkey operation; instruction length not valid");
                    }
                    if (str[0] == "+")
                    {
                        monkey.Operation = Monkey.eOperation.Add;
                        monkey.OperationData = int.Parse(str[1]);
                    }
                    else if (str[0] == "*")
                    {

                        if (str[1] == "old")
                        {
                            monkey.Operation = Monkey.eOperation.Square;
                            monkey.OperationData = 0;
                        }
                        else
                        {
                            monkey.Operation = Monkey.eOperation.Multiply;
                            monkey.OperationData = int.Parse(str[1]);
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Parser expected + or * only, not {str[3]}");
                    }
                    row = 3;
                }
                else if (row == 3)
                {
                    //  Test: divisible by 23
                    Regex pattern = new Regex(@"  Test: divisible by (?<teststr>\d+)");
                    Match match = pattern.Match(line);
                    monkey.TestDivisor = int.Parse(match.Groups["teststr"].ToString());
                    
                    row = 4;
                }
                else if (row == 4)
                {
                    //  If true: throw to monkey 2
                    Regex pattern = new Regex(@"  If true: throw to monkey (?<teststr>\d+)");
                    Match match = pattern.Match(line);
                    monkey.TestTrueIndex = int.Parse(match.Groups["teststr"].ToString());

                    row = 5;
                }
                else if (row == 5)
                {
                    //  If false: throw to monkey 3
                    Regex pattern = new Regex(@"  If false: throw to monkey (?<teststr>\d+)");
                    Match match = pattern.Match(line);
                    monkey.TestFalseIndex = int.Parse(match.Groups["teststr"].ToString());

                    MonkeyList.Add(monkey);
                    
                    row = 0;
                }
                
            }
        }

        public void RunA()
        {
            var start = DateTime.Now.Microsecond;

            Parse("Input\\Day11.txt");
            const int maxRounds = 20;

            for(int i=0; i < maxRounds; i++)
            {
                for(int m=0;m<MonkeyList.Count;m++)
                {
                    for(int k = 0; k < MonkeyList[m].Items.Count;k++)
                    {
                        MonkeyList[m].RunOperation(k);
                        var moveTo = MonkeyList[m].Test(k);
                        MonkeyList[moveTo].Items.Add(MonkeyList[m].Items[k]);
                    }
                    MonkeyList[m].Items.Clear();
                }
                //Monkey 0: 20, 23, 27, 26
                //Monkey 1: 2080, 25, 167, 207, 401, 1046
                //Monkey 2: 
                //Monkey 3: 
                //foreach(var m in MonkeyList)
                //{
                //    Console.WriteLine($"Monkey {m.Id}: {m.ToString()}");
                //}
                //Console.WriteLine("");
            }
            var monkeyOrderedList = MonkeyList.OrderByDescending(x => x.InspectionsCount).ToList();
            
            //foreach(var m in MonkeyList)
            //{
            //    Console.WriteLine($"Monkey: {m.Id} Inspections: {m.InspectionsCount}");
            //}

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 11A: {monkeyOrderedList[0].InspectionsCount* monkeyOrderedList[1].InspectionsCount} {elapsed} μs");
        }

        public void RunB()
        {
            var start = DateTime.Now.Microsecond;

            Parse("Input\\Day11test.txt");
            const int maxRounds = 10000;

            for (int i = 1; i <= maxRounds; i++)
            {
                for (int m = 0; m < MonkeyList.Count; m++)
                {
                    for (int k = 0; k < MonkeyList[m].Items.Count; k++)
                    {
                        MonkeyList[m].RunOperation(k);
                        var moveTo = MonkeyList[m].Test(k);
                        MonkeyList[moveTo].Items.Add(MonkeyList[m].Items[k]);
                    }
                    MonkeyList[m].Items.Clear();
                }
                //foreach(var m in MonkeyList)
                //{
                //    Console.WriteLine($"Monkey {m.Id}: {m.ToString()}");
                //}
                //Console.WriteLine("");

                if(i == 1 || i == 20 || i % 1000 == 0)
                {
                    Console.WriteLine($"== After round {i} ==");
                    foreach (var m in MonkeyList)
                    {
                        Console.WriteLine($"Monkey: {m.Id} Inspections: {m.InspectionsCount}");
                    }
                    Console.WriteLine($"");
                }
                
            }
            var monkeyOrderedList = MonkeyList.OrderByDescending(x => x.InspectionsCount).ToList();

           

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 11A: {(long)monkeyOrderedList[0].InspectionsCount * monkeyOrderedList[1].InspectionsCount} {elapsed} μs");
        }
    }
}
