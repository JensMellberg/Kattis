using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AddEmUp
{
    internal class Program
    {
        static Dictionary<char, char> UpsideDowns = new Dictionary<char, char>()
        {
            {'1', '1' },
            {'2', '2' },
            {'5', '5' },
            {'6', '9' },
            {'9', '6' },
            {'8', '8' },
            {'0', '0' }
        };

        static void Main(string[] args)
        {
            var line = "";
            while ((line = Console.ReadLine()) != null)
            {
                var tokens = line.Split(' ').Select(int.Parse).ToArray();
                var cardsCount = tokens[0];
                var sum = tokens[1];
                var cards = new int[cardsCount];
                var possibleNumbersCount = new (int count, int fromIndex)[100000001];
                var values = Console.ReadLine().Split(' ');
                for (var i = 0; i < cardsCount; i++)
                {
                    var value = int.Parse(values[i]);
                    possibleNumbersCount[value] = (possibleNumbersCount[value].count + 1, i);
                    var turned = Turn(value);
                    if (turned != -1)
                    {
                        possibleNumbersCount[turned] = (possibleNumbersCount[turned].count + 1, i);
                    }
                }

                var flag = false;
                for (var i = 0; i < possibleNumbersCount.Length; i++)
                {
                    var count = possibleNumbersCount[i];
                    if (count.count == 0)
                    {
                        continue;
                    }

                    var numberNeeded = sum - i;
                    var numberStatus = possibleNumbersCount[numberNeeded];
                    if (numberStatus.count > 1 || 
                        numberStatus.fromIndex != count.fromIndex && numberStatus.count == 1)
                    {
                        Console.WriteLine("YES");
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    Console.WriteLine("NO");
                }
            }
        }

        static int Turn(int number)
        {
            var stringified = number.ToString();
            if (stringified.Any(x => !UpsideDowns.ContainsKey(x)))
            {
                return -1;
            }

            return int.Parse(string.Join("", stringified.Reverse().Select(x => UpsideDowns[x])));
        }
    }
}