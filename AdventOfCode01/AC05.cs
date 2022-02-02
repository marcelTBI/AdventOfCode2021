public class Vent
{
    public int x1;
    public int y1;
    public int x2;
    public int y2;

    public Vent(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
    }

    public bool Diagonal()
    {
        return (x1 != x2) && (y1 != y2);
    }

    public IEnumerable<(int, int)> GetPoints()
    {
        int dx = Math.Sign(x2 - x1);
        int dy = Math.Sign(y2 - y1);

        for (int i = 0; i <= Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2)); i++)
        {
            yield return (x1 + dx * i, y1 + dy * i);
        }
    }

}

internal class AC05
{

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input5.txt");

        // parse lines
        List<Vent> vents = new List<Vent>();
        foreach (string line in lines)
        {
            var split = line.Split(' ');
            var first = split[0].Split(',');
            var second = split[2].Split(',');
            vents.Add(new Vent(Convert.ToInt32(first[0]), Convert.ToInt32(first[1]), Convert.ToInt32(second[0]), Convert.ToInt32(second[1])));
        }

        // go through all vents and build dict of overlaps
        Dictionary<(int, int), int> numVents = new Dictionary<(int, int), int>();
        foreach (Vent vent in vents)
        {
            if (!vent.Diagonal() || true)  // change to 'if (!vent.Diagonal())' for 1st star
            {
                foreach (var (x, y) in vent.GetPoints())
                {
                    if (numVents.ContainsKey((x, y)))
                    {
                        numVents[(x, y)] += 1;
                    }
                    else
                    {
                        numVents[(x, y)] = 1;
                    }
                }
            }
        }

        // count all >=2 vents
        var dangerous = numVents.Values.Count(v => v >= 2);

        // print
        Console.WriteLine(dangerous);
    }
}
