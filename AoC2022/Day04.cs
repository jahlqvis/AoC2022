using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    public class SectionAssignment
    {
        public SectionAssignment(int b1, int e1, int b2, int e2) 
        {
            Begin1 = b1;
            End1 = e1;
            Begin2 = b2;
            End2 = e2;
        }

        public int Begin1 { get; set; }
        public int End1 { get; set; }
        public int Begin2 { get; set; }
        public int End2 { get; set; }
    }

    
    public class Day04
    {
        public List<SectionAssignment> Assignments { get; set; }

        public Day04()
        {
            Assignments = new List<SectionAssignment>();
            Parse("Input\\Day04.txt");
        }

        public void Parse(string path)
        {
            var lines = File.ReadAllLines(path);
            foreach(var line in lines)
            {
                var assignments = line.Split(",");
                var assObject = new SectionAssignment(int.Parse(assignments[0].Split("-")[0]), int.Parse(assignments[0].Split("-")[1]), int.Parse(assignments[1].Split("-")[0]), int.Parse(assignments[1].Split("-")[1]));
                Assignments.Add(assObject);
            }
        }

        public void RunA()
        {
            var start = DateTime.Now.Microsecond;
            int totalOverlaps = 0;
            foreach(var ass in Assignments)
            {
                if(ass.Begin1 <= ass.Begin2 && ass.End1 >= ass.End2)
                {
                    totalOverlaps++;
                } else if(ass.Begin2 <= ass.Begin1 && ass.End2 >= ass.End1)
                {
                    totalOverlaps++;
                } else
                {
                    // do nothing
                }
            }
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 04A: {totalOverlaps} : {elapsed} μs");
        }

        public void RunB()
        {
            var start = DateTime.Now.Microsecond;
            int totalOverlaps = 0;
            foreach (var ass in Assignments)
            {
                if (!(ass.End1 < ass.Begin2 || ass.End2 < ass.Begin1 || ass.Begin1 > ass.End2 || ass.Begin2 > ass.End1))
                {
                    totalOverlaps++;
                }
            }
            var elapsed = DateTime.Now.Microsecond - start;
            Console.WriteLine($"Day 04B: {totalOverlaps} : {elapsed} μs");
        }
    }
}
