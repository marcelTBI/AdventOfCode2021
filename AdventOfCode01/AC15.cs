internal class AC15
{
    public static int BestPath(List<short[]> risks)
    {
        PriorityQueue<(int, int), int> pq = new PriorityQueue<(int, int), int>();
        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        Dictionary<(int, int), int> minimal = new Dictionary<(int, int), int>();
        minimal[(0, 0)] = 0;
        pq.Enqueue((0, 0), 0);

        while (true)
        {
            // get new position
            bool valid = pq.TryDequeue(out (int, int) element, out int currentRisk);
            if (!valid) break;
            //Console.WriteLine(Convert.ToString(element.Item1) + " " + Convert.ToString(element.Item2) + " " + Convert.ToString(currentRisk));
            visited.Add(element);

            // add x,y
            int x = element.Item1;
            int y = element.Item2;

            // end 
            if (x == risks[^1].Length - 1 && y == risks.Count() - 1)
            {
                return currentRisk;
            }

            // process it
            List<(int, int, int)> tryAdd = new List<(int, int, int)>();

            if (x > 0 && !visited.Contains((x - 1, y))) tryAdd.Add((x - 1, y, risks[x - 1][y] + currentRisk));
            if (y > 0 && !visited.Contains((x, y - 1))) tryAdd.Add((x, y - 1, risks[x][y - 1] + currentRisk));
            if (x < risks[0].Length - 1 && !visited.Contains((x + 1, y))) tryAdd.Add((x + 1, y, risks[x + 1][y] + currentRisk));
            if (y < risks.Count() - 1 && !visited.Contains((x, y + 1))) tryAdd.Add((x, y + 1, risks[x][y + 1] + currentRisk));

            for (int i = 0; i < tryAdd.Count; i++)
            {
                if (!minimal.ContainsKey((tryAdd[i].Item1, tryAdd[i].Item2)) || minimal[(tryAdd[i].Item1, tryAdd[i].Item2)] > tryAdd[i].Item3)
                {
                    pq.Enqueue((tryAdd[i].Item1, tryAdd[i].Item2), tryAdd[i].Item3);
                    minimal[(tryAdd[i].Item1, tryAdd[i].Item2)] = tryAdd[i].Item3;
                }
            }


        }

        // should not happen
        return -1;
    }

    public static short[] Extend(short[] x)
    {
        short[] result = new short[x.Length * 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < x.Length; j++)
            {
                result[i * x.Length + j] = (short)(x[j] + i > 9 ? x[j] + i - 9 : x[j] + i);
            }
        }

        return result;
    }


    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input15.txt");

        // parse lines
        List<short[]> risks = new List<short[]>();
        foreach (string line in lines)
        {
            risks.Add((from l in line.Trim() select Convert.ToInt16(Char.ToString(l))).ToArray());
        }

        // edit the input
        // first elongate rows
        for (int i = 0; i < risks.Count; i++)
        {
            risks[i] = Extend(risks[i]);
        }
        // now elongate columns
        int len = risks.Count();
        for (int i = 1; i < 5; i++)
        {
            for (int j = 0; j < len; j++)
            {
                risks.Add((short[])risks[j].Clone());
                for (int k = 0; k < risks[^1].Length; k++) risks[^1][k] = (short)(risks[^1][k] + i > 9 ? risks[^1][k] + i - 9 : risks[^1][k] + i);
            }
        }

        // find best path
        int length = BestPath(risks);
        Console.WriteLine(length);
    }


}
