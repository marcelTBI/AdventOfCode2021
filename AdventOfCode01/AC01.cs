using System;

internal class AC01 
{
    public static void Run1(string[] args)
    {

        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input1.txt");
        long lastDepth = Int64.MaxValue;
        int times = 0;

        foreach (string line in lines)
        {
            var depth = Int64.Parse(line);
            if (depth > lastDepth)
            {
                times++;
            }
            lastDepth = depth;

        }

        Console.WriteLine(times);
    }

    public static void Run2(string[] args)
    {
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input1.txt");
        var numbers = (from line in lines select Int64.Parse(line)).ToArray();

        var triplets = (from idx in Enumerable.Range(0, numbers.Length - 2) select numbers[idx] + numbers[idx + 1] + numbers[idx + 2]).ToArray();
        var increase = (from idx in Enumerable.Range(0, triplets.Length - 1) select Convert.ToDecimal(triplets[idx + 1] > triplets[idx])).ToArray();


        Console.WriteLine(increase.Sum());
    }
}
