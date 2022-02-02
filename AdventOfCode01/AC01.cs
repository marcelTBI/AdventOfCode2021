internal class AC01
{
    public static void Run1()
    {
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input1.txt");
        long lastDepth = long.MaxValue;
        int times = 0;

        foreach (string line in lines)
        {
            var depth = long.Parse(line);
            if (depth > lastDepth)
            {
                times++;
            }
            lastDepth = depth;

        }

        Console.WriteLine(times);
    }

    public static void Run2()
    {
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input1.txt");
        var numbers = (from line in lines select long.Parse(line)).ToArray();

        var triplets = (from idx in Enumerable.Range(0, numbers.Length - 2) select numbers[idx] + numbers[idx + 1] + numbers[idx + 2]).ToArray();
        var increase = (from idx in Enumerable.Range(0, triplets.Length - 1) select Convert.ToDecimal(triplets[idx + 1] > triplets[idx])).ToArray();


        Console.WriteLine(increase.Sum());
    }
}
