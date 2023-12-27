using System;

string line;
while ((line = Console.ReadLine()) != null)
{
    var goalNumber = long.Parse(line);

    var current = goalNumber;
    var checkForWin = true;
    while (current >= 1)
    {
        if (checkForWin)
        {
            var winsWhen2 = (long)Math.Ceiling((double)current / 9);
            var range = (winsWhen2, current);
            current = winsWhen2;
            if (range.winsWhen2 <= 1)
            {
                Console.WriteLine("Stan wins.");
                break;
            }
        }
        else
        {
            var losesWhen = (long)Math.Ceiling((double)current / 2);
            var range = (losesWhen, current);
            current = losesWhen;
            if (range.losesWhen <= 1)
            {
                Console.WriteLine("Ollie wins.");
                break;
            }
        }

        checkForWin = !checkForWin;
    }
}
