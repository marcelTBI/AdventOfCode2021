internal class AC07
{
    public static int CalcFuel(int[] nums, int target)
    {
        return (from num in nums select Math.Abs(num - target)).ToArray().Sum();
    }

    public static int CalcFuel2(int[] nums, int target)
    {
        return (from num in nums select Math.Abs(num - target) * (Math.Abs(num - target) + 1) / 2).ToArray().Sum();
    }


    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input7.txt");
        //lines[0] = "16,1,2,0,4,2,7,1,2,14";

        // parse nums
        int[] nums = (from str in lines[0].Split(',') select Convert.ToInt32(str)).ToArray();

        // get nums to array
        int minFuel = Int32.MaxValue;
        for (int i = nums.Min(); i < nums.Max(); i++)
        {
            minFuel = Math.Min(CalcFuel2(nums, i), minFuel);
        }

        // print min. fuel
        Console.WriteLine(minFuel);
    }
}
