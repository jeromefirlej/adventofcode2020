using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Advent03
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("./input.txt");
            Console.WriteLine(Part1(lines, 3, 1));   
            Console.WriteLine(Part2(lines));            
        }

        public static long Part1(IEnumerable<string> lines, int right, int down){
            int index = 0;
            int downIndex = 1;
            long nbTrees = 0;
            foreach (var line in lines)
            {
                if(down != 1 && downIndex == down)
                {
                    downIndex = 1;
                    continue;
                }
                if(line[index] == '#'){
                    nbTrees++;
                }
                index += right;
                if(index >= line.Length)
                    index -= line.Length;
                downIndex++;
            }
            return nbTrees;
        }

        public static long Part2(IEnumerable<string> lines){
            var tabLines = lines.ToArray();
            return Part1(tabLines, 1, 1) *
            Part1(tabLines, 3, 1) *
            Part1(tabLines, 5, 1) *
            Part1(tabLines, 7, 1) *
            Part1(tabLines, 1, 2);

        }

    }
}
