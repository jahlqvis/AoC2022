using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AoC2022.Day07;

namespace AoC2022
{
    public class Day07
    {
        public List<string> Commands= new List<string>();
        public Node Root = new Node();

        public class Node : IComparable
        {
            public string Filename = "";
            public int Size;
            public bool IsDir;
            public Node? Parent;
            public List<Node> Childs = new List<Node>();

            public int CompareTo(Object o)
            {
                Node e = o as Node;
                if (e == null)
                    throw new ArgumentException("o is not an Node object.");

                return Filename.CompareTo(e.Filename);
            }
        };

        public class NodeSearch
        {
            String _s;

            public NodeSearch(String s)
            {
                _s = s;
            }

            public bool Equals(Node e)
            {
                return e.Filename.Equals(_s);
            }
        }

        static Node newNode(string filename, int size, bool isDir, Node parent)
        {
            Node temp = new Node();
            temp.Filename = filename;
            temp.Size = size;
            temp.IsDir= isDir;
            temp.Parent = parent;
            return temp;
        }

        int CalculateDirectorySizes(Node node)
        {
            int size = 0;

            if(node.Childs.Count == 0)
            {
                return node.Size;
            }
            else
            {
                foreach (var n in node.Childs)
                {
                    size += CalculateDirectorySizes(n);
                }
                if (node.IsDir)
                {
                    node.Size = size;
                }
                return node.Size;
            }
        }

        public int TotalUsedDiskSpace=0;
        public int NeedToFree;
        public List<int> FoundNodeSizes = new List<int>();

        int FindDirectoriesSizeLessThan(Node node)
        {
            //int totalSize = 0;
            int size = 0;
            foreach (var n in node.Childs)
            {
                if (n.IsDir)
                {
                    size = FindDirectoriesSizeLessThan(n);
                    if (n.Size <= 100000)
                    {
                        TotalUsedDiskSpace += n.Size + size;
                        //Console.WriteLine($"{n.Filename} {n.Size} {TotalSize}");
                    }
                }
            }
            
            return size;
        }


        void TraversePostorderFindDirB(Node node)
        {
            foreach (var n in node.Childs)
            {
                if (n.IsDir)
                {
                    TraversePostorderFindDirB(n);
                    if (n.Size >= NeedToFree)
                    {
                        FoundNodeSizes.Add(n.Size);
                    }
                }
            }
        }
        public Day07()
        {
            Root.Filename = "/";
            Root.IsDir = true;
            Root.Childs = new List<Node>();
            Root.Parent = null;

            Parse("Input\\Day07.txt");
        }

        public void Parse(string s)
        {
            var lines = File.ReadAllLines(s);

            foreach (var line in lines)
            {
                Commands.Add(line);
            }
        }

        public void RunB()
        {
            const int totalDiskCapacity = 70000000;
            const int freeDiskSpaceNeededForUpdate = 30000000;

            var start = DateTime.Now.Microsecond;

            int freeDiskSpace = totalDiskCapacity - Root.Size;
            NeedToFree = freeDiskSpaceNeededForUpdate - freeDiskSpace;

            TraversePostorderFindDirB(Root);
            FoundNodeSizes.Sort();

            Debug.Assert(FoundNodeSizes[0] == 5025657);
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 07B: {FoundNodeSizes[0]} : {elapsed} μs");
        }

        public void RunA()
        {
            var start = DateTime.Now.Microsecond;
            var currentDir = "";
            Node currentNode = Root;
            int size;
            bool ls = false;
            foreach (var command in Commands)
            {
                if(command.StartsWith("$"))
                {
                    if(command.Contains("cd"))
                    {
                        ls = false;
                        var temp = command.Split(' ');
                        currentDir = temp[temp.Length - 1];

                        if(currentDir != "/")
                        {
                            if (currentDir == "..")
                            {
                                currentDir = currentNode.Parent.Filename;
                                currentNode = currentNode.Parent;
                            }
                            else
                            {
                                NodeSearch es = new NodeSearch(currentDir);
                                var inx = currentNode.Childs.FindIndex(0, currentNode.Childs.Count, es.Equals);

                                currentNode = currentNode.Childs[inx];
                            }
                        }
                    }
                    if (command.Contains("ls"))
                    {
                        ls = true;
                    }
                }
                if(ls)
                {
                    var temp = command.Split(' ');
                    size=0;
                    string name = "";
                    if (temp[0] == "dir")
                    {
                        name = temp[1];
                        
                        var node = newNode(name, size, true, currentNode);
                        currentNode.Childs.Add(node);
                    }
                    else if (temp[0].All(char.IsDigit))
                    {
                        size = int.Parse(temp[0]);
                        name = temp[1];
                        
                        var node = newNode(name, size, false, currentNode);
                        currentNode.Childs.Add(node);
                    }
                    else
                    {
                        // nada
                    }
                }
            }

            CalculateDirectorySizes(Root);
            size = FindDirectoriesSizeLessThan(Root);
            Debug.Assert(TotalUsedDiskSpace == 1915606);

            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 07A: {TotalUsedDiskSpace} : {elapsed} μs");
        }
    }
}
