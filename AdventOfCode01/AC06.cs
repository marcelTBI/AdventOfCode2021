internal class AC06
{

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input6.txt");
        //lines[0] = "3,4,3,1,2";

        // parse nums
        int[] nums = (from str in lines[0].Split(',') select Convert.ToInt32(str)).ToArray();

        // get nums to array
        long[] fishes = new long[9];
        foreach (int i in nums)
        {
            fishes[i] += 1;
        }

        // procreate!
        for (int i = 0; i < 256; i++)
        {
            var newFish = fishes[0];
            for (int j = 0; j < fishes.Length - 1; j++)
                fishes[j] = fishes[j + 1];
            fishes[6] += newFish;
            fishes[8] = newFish;
        }

        // print number of fish
        Console.WriteLine(fishes.Sum());
    }
}
