using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace CreativeAccounting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var line = "";
            while ((line = Console.ReadLine()) != null)
            {
                var tokens = line.Split(' ').Select(int.Parse).ToArray();
                var minChoice = tokens[1];
                var maxChoice = tokens[2];
                var days = tokens[0];
                var dailyProfit = new int[days];
                for (var i = 0; i < days; i++)
                {
                    dailyProfit[i] = int.Parse(Console.ReadLine());
                }


            }
        }
    }
}