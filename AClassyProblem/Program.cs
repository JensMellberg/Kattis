using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AClassyProblem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var line = Console.ReadLine();
            while ((line = Console.ReadLine()) != null)
            {
                var people = new List<Person>();
                for (var i = 0; i < int.Parse(line); i++)
                {
                    var inLine = Console.ReadLine().Split(' ');
                    var name = inLine[0][..^1];
                    var classes = inLine[1].Split('-');
                    var classList = Enumerable.Range(0, 10 - classes.Length).Select(x => "middle").Concat(classes).Select(ClassFromString).ToList();
                    people.Add(new Person { Name = name, Classes = classList });
                }

                foreach (var p in people.OrderByDescending(x => x))
                {
                    Console.WriteLine(p.Name);
                }

                Console.WriteLine(string.Join("", Enumerable.Range(0, 30).Select(x => "=")));
            }
        }

        static Class ClassFromString(string classs) => classs switch
        {
            "middle" => Class.Middle,
            "upper" => Class.Upper,
            "lower" => Class.Lower
        };

        private class Person : IComparable
        {
            public string Name { get; set; }

            public List<Class> Classes { get; set; } = new List<Class>();

            public int CompareTo(object? obj)
            {
                var other = obj as Person;
                var pointer = 9;
                while (pointer > -1)
                {
                    if (Classes[pointer] != other.Classes[pointer])
                    {
                        return Classes[pointer].CompareTo(other.Classes[pointer]);  
                    }

                    pointer--;
                }

                return Name.CompareTo(other.Name) * -1;
            }
        }

        private enum Class
        {
            Upper = 2,
            Middle = 1,
            Lower = 0
        }
    }
}