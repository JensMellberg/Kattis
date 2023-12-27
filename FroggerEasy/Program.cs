using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FroggerEasy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var line = "";
            while ((line = Console.ReadLine()) != null)
            {
                var tokens = line.Split(' ').Select(int.Parse).ToArray();
                var squares = tokens[0];
                var start = tokens[1] - 1;
                var goal = tokens[2];
                var squareNumbers = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var visited = new HashSet<int>();
                var current = start;
                var jumps = 0;
                while (true)
                {
                    if (current < 0)
                    {
                        Console.WriteLine("left");
                        Console.WriteLine(jumps);
                        break;
                    }

                    if (current >= squareNumbers.Length)
                    {
                        Console.WriteLine("right");
                        Console.WriteLine(jumps);
                        break;
                    }

                    var number = squareNumbers[current];
                    if (number == goal)
                    {
                        Console.WriteLine("magic");
                        Console.WriteLine(jumps);
                        break;
                    }

                    if (visited.Contains(current))
                    {
                        Console.WriteLine("cycle");
                        Console.WriteLine(jumps);
                        break;
                    }

                    jumps++;
                    visited.Add(current);
                    current += number;
                }
            }
        }
    }
}