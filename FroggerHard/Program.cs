using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace FroggerEasy
{
    internal class Program
    {
        private class GroupChain
        {
            public long Count { get; set; }

            public Dictionary<int, (int index, int duplicates)> Indexes { get; set; }

            public HashSet<int> CycleIndexes { get; set; }

            public long CycleCount { get; set; }
        }
        static void Main(string[] args)
        {
            var line = "";
            while ((line = Console.ReadLine()) != null)
            {
                var squares = int.Parse(line);
                var squareNumbers = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var currentGroup = 1;
                var squareGroups = new int[squareNumbers.Length];
                var groupChainIndexes = new Dictionary<int, GroupChain>();
                var visitedNumbers = new HashSet<int>();
                var duplicates = 0;
                long total = 0;
                for (var i = 0; i < squareNumbers.Length; i++)
                {
                    if (squareGroups[i] == 0)
                    {
                        var group = currentGroup++;
                        var current = i;
                        var index = 0;
                        var visited = new Dictionary<int, (int index, int duplicates)>();
                        while (true)
                        {
                            if (current < 0 || current >= squareNumbers.Length)
                            {
                                var count = index - duplicates;
                                var a = (long)count * (long)(count + 1);
                                total += a / 2;
                                var chain = new GroupChain
                                {
                                    Indexes = visited,
                                    Count = index
                                };

                                groupChainIndexes.Add(group, chain);
                                break;
                            }

                            var existingGroup = squareGroups[current];
                            if (existingGroup != 0)
                            {
                                if (existingGroup == group)
                                {
                                    var chainStartIndex = visited[current];
                                    var chainIndexes = visited.Where(x => x.Value.index >= chainStartIndex.index).Select(x => x.Key).ToHashSet();
                                    var chainCount = index - chainStartIndex.index;
                                    var normalCount = chainStartIndex.index;
                                    var a = (long)normalCount * (long)(normalCount + chainCount + chainCount + 1);
                                    total += a / 2;
                                    total += (long)Math.Pow((long)chainCount, 2);
                                    var groupChain = new GroupChain
                                    {
                                        Indexes = visited,
                                        CycleIndexes = chainIndexes,
                                        Count = index,
                                        CycleCount = chainCount
                                    };
                                    
                                    groupChainIndexes.Add(group, groupChain);
                                    break;
                                }

                                var existingChain = groupChainIndexes[existingGroup];
                                long count = 0;
                                if (existingChain.CycleIndexes != null && existingChain.CycleIndexes.Contains(current))
                                {
                                    var a = (long)index * (long)(index + existingChain.CycleCount + existingChain.CycleCount + 1);
                                    total += a / 2;
                                    count = index + existingChain.CycleCount;
                                }
                                else
                                {
                                    var otherCount = existingChain.Count - existingChain.Indexes[current].index;
                                    var a = (long)index * (long)(index + otherCount + otherCount + 1);
                                    total += a / 2;
                                    count = index + otherCount;
                                }

                                var chainy = new GroupChain
                                {
                                    Indexes = visited,
                                    Count = count
                                };

                                groupChainIndexes.Add(group, chainy);

                                break;
                            }

                            var number = squareNumbers[current];
                            if (visitedNumbers.Contains(number))
                            {
                                duplicates++;
                            }
                            else
                            {
                                visitedNumbers.Add(number);
                            }

                            visited.Add(current, (index++, duplicates));
                            squareGroups[current] = group;
                            current += number;
                        }
                    }
                }

                Console.WriteLine(total);
            }
        }
    }
}