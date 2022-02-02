internal class AC12
{
    public static int SearchPath(Dictionary<string, List<string>> caves, string current, HashSet<string> foundAlready, bool time)
    {
        if (current == "end")
        {
            return 1;
        }

        int ret = 0;

        foreach (string cave in caves[current])
        {
            if (!foundAlready.Contains(cave) || cave.ToUpper() == cave)
            {
                foundAlready.Add(cave);
                ret += SearchPath(caves, cave, foundAlready, time);
                foundAlready.Remove(cave);
            }
            else if (time && cave != "start")
            {
                foundAlready.Add(cave + "time");
                ret += SearchPath(caves, cave, foundAlready, false);
                foundAlready.Remove(cave + "time");
            }

        }
        return ret;
    }

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input12.txt");

        // parse
        Dictionary<string, List<string>> caves = new Dictionary<string, List<string>>();
        foreach (string line in lines)
        {
            var split = line.Trim().Split('-');
            if (!caves.ContainsKey(split[0]))
            {
                caves[split[0]] = new List<string>();
            }
            if (!caves.ContainsKey(split[1]))
            {
                caves[split[1]] = new List<string>();
            }
            caves[split[0]].Add(split[1]);
            caves[split[1]].Add(split[0]);
        }

        // search for paths
        int paths = SearchPath(caves, "start", new HashSet<string> { "start" }, true);

        Console.WriteLine(paths);
    }
}
