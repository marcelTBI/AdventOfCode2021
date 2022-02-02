internal class AC09
{
    public static bool IsLowest(List<short[]> height, int x, int y)
    {
        if (x > 0 && height[y][x - 1] <= height[y][x]) return false;
        if (x < height[y].Length - 1 && height[y][x + 1] <= height[y][x]) return false;
        if (y > 0 && height[y - 1][x] <= height[y][x]) return false;
        if (y < height.Count - 1 && height[y + 1][x] <= height[y][x]) return false;
        return true;
    }

    public static int BasinSize(List<short[]> height, int x, int y, HashSet<(int, int)> basins)
    {
        if (basins.Contains((x, y))) return basins.Count;
        basins.Add((x, y));

        if (x > 0 && height[y][x - 1] > height[y][x] && height[y][x - 1] != 9)
        {
            BasinSize(height, x - 1, y, basins);
        }
        if (x < height[y].Length - 1 && height[y][x + 1] > height[y][x] && height[y][x + 1] != 9)
        {
            BasinSize(height, x + 1, y, basins);
        }
        if (y > 0 && height[y - 1][x] > height[y][x] && height[y - 1][x] != 9)
        {
            BasinSize(height, x, y - 1, basins);
        }
        if (y < height.Count - 1 && height[y + 1][x] > height[y][x] && height[y + 1][x] != 9)
        {
            BasinSize(height, x, y + 1, basins);
        }

        return basins.Count;
    }

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input9.txt");

        // parse lines
        List<short[]> heights = new List<short[]>();
        foreach (string line in lines)
        {
            heights.Add((from l in line.Trim() select Convert.ToInt16(Char.ToString(l))).ToArray());
        }

        // go through all
        int lowest = 0;
        List<int> sizes = new List<int>();
        for (int y = 0; y < heights.Count; y++)
        {
            for (int x = 0; x < heights[y].Length; x++)
            {
                if (IsLowest(heights, x, y))
                {
                    lowest += heights[y][x] + 1;
                    sizes.Add(BasinSize(heights, x, y, new HashSet<(int, int)>()));
                }
            }
        }


        // print appearance
        Console.WriteLine(lowest);

        // print basin sizes
        sizes.Sort();
        Console.WriteLine(sizes[^3] * sizes[^2] * sizes[^1]);
    }
}
