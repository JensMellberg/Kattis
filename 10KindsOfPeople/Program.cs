using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace _10KindsOfPeople
{
    internal class Program
    {
        static int[,] Groups;

        static int CurrentGroup;

        static void Main(string[] args)
        {
            var line = "";
            while ((line = Console.ReadLine())!= null)
            {
                var tokens = line.Split(' ');
                var height = int.Parse(tokens[0]);
                var width = int.Parse(tokens[1]);
                var field = new bool[width, height];
                Groups = new int[width, height];
                for (var y = 0; y < height; y++)
                {
                    var inLine = Console.ReadLine();
                    for (var x = 0; x < width; x++)
                    {
                        field[x, y] = inLine[x] == '1';
                    }
                }

                SetGroups(field);
                var querys = int.Parse(Console.ReadLine());
                for (var i = 0; i < querys; i++)
                {
                    var inLine = Console.ReadLine();
                    var coords = inLine.Split(' ').Select(int.Parse).ToArray();
                    var start = (coords[1] - 1, coords[0] - 1);
                    var end = (coords[3] - 1, coords[2] - 1);
                    var startGroup = Groups[start.Item1, start.Item2];
                    var endGroup = Groups[end.Item1, end.Item2];
                    if (startGroup == endGroup)
                    {
                        Console.WriteLine(field[start.Item1, start.Item2] ? "decimal" : "binary");
                    }
                    else
                    {
                        Console.WriteLine("neither");
                    }
                }
            }
        }

        static void SetGroups(bool[,] field)
        {
            for (var x = 0; x < field.GetLength(0); x++)
            {
                for (var y = 0; y < field.GetLength(1); y++)
                {
                    if (Groups[x, y] == 0)
                    {
                        SetGroups(field, (x, y));
                    }
                }
            }
        }

        static void SetGroups(bool[,] field, (int x, int y) start)
        {
            var group = ++CurrentGroup;
            var isDecimal = field[start.x, start.y];
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue((start.x, start.y));
            var visited = new HashSet<(int x, int y)>();
            while (queue.Any())
            {
                var (x, y) = queue.Dequeue();
                Groups[x, y] = group;

                TryAdd(x - 1, y);
                TryAdd(x + 1, y);
                TryAdd(x, y - 1);
                TryAdd(x, y + 1);

                void TryAdd(int newX, int newY)
                {
                    if (newX >= 0 && newX < field.GetLength(0) 
                        && newY >= 0 && newY < field.GetLength(1)
                        && !visited.Contains((newX, newY))
                        && field[newX, newY] == isDecimal)
                    {
                        visited.Add((newX, newY));
                        queue.Enqueue((newX, newY));
                    }
                }
            }
        }
    }
}