using System;

internal class AC02
{

    public static void Run1(string[] args)
    {
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input2.txt");
        var direction = (from line in lines select line.Split()[0]).ToArray();
        var length = (from line in lines select Convert.ToInt32(line.Split()[1])).ToArray();

        int x = 0;
        int depth = 0;

        for (int i = 0; i < length.Length; i++)
        {
            if (direction[i] == "forward")
            {
                x += length[i];
            }
            if (direction[i] == "up")
            {
                depth -= length[i];
            }
            if (direction[i] == "down")
            {
                depth += length[i];
            }
        }

        Console.WriteLine(x * depth);
    }

    public static void Run2(string[] args)
    {
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input2.txt");
        var direction = (from line in lines select line.Split()[0]).ToArray();
        var length = (from line in lines select Convert.ToInt32(line.Split()[1])).ToArray();

        int x = 0;
        int depth = 0;
        int aim = 0;

        for (int i = 0; i < length.Length; i++)
        {
            if (direction[i] == "forward")
            {
                x += length[i];
                depth += aim * length[i];
            }
            if (direction[i] == "up")
            {
                aim -= length[i];
            }
            if (direction[i] == "down")
            {
                aim += length[i];
            }
        }

        Console.WriteLine(x * depth);
    }
}
