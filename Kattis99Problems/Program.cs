using System;

string line;
while (( line = Console.ReadLine()) != null)
{
    var digits = line;
    if (digits.Length < 3)
    {
        Console.WriteLine(99);
    }
    else
    {
        var digitsInt = int.Parse(digits);
        var twoLast = int.Parse(digits[^2..]);
        if (twoLast >= 49)
        {
            PrintWithEnding("99");
        }
        else
        {
            var newDigits = digitsInt - (twoLast + 1);
            Console.WriteLine(newDigits);
        }
    }

    void PrintWithEnding(string ending)
    {
        Console.WriteLine(digits[..^ending.Length] + ending);
    }
}
