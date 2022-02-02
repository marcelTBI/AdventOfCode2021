internal class AC13
{

    public static List<(int, int)> Fold(List<(int, int)> dots, bool throughX, int foldLine)
    {
        List<(int, int)> newDots = new List<(int, int)> ();
        foreach (var dot in dots)
        {
            var x = dot.Item1;
            var y = dot.Item2;
            if (throughX && x > foldLine)
            {
                x = 2 * foldLine - x;
            }
            if (!throughX && y > foldLine)
            {
                y = 2 * foldLine - y;
            }
            newDots.Add((x, y));
        }
        return newDots.Distinct().ToList();
    }

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input13.txt");

        // parse
        List<(int, int)> dots = new List<(int, int)> ();
        List<(int, bool)> folds = new List<(int, bool)> ();
        foreach (string line in lines)
        {
            string[] split = line.Trim().Split(',');
            if (split.Length == 2)
            {
                dots.Add((Convert.ToInt32(split[0]), Convert.ToInt32(split[1])));
            }
            if (split.Length == 1 && split[0].Contains('='))
            {
                string[] foldSplit = split[0].Split(' ')[^1].Split('=');
                folds.Add((Convert.ToInt32(foldSplit[1]), foldSplit[0] == "x"));
            }
        }

        // fold
        foreach ((int, bool) fold in folds)
        {
            bool throughX = fold.Item2;
            int foldLine = fold.Item1;
            dots = Fold(dots, throughX, foldLine);
            Console.WriteLine(dots.Count);
            //break;
        }

        // print dots:
        int maxX = (from a in dots select a.Item1).Max() + 1;
        int maxY = (from a in dots select a.Item2).Max() + 1;
        int[,] dotsAscii = new int[maxX, maxY];
        foreach (var a in dots)
        {
            dotsAscii[a.Item1, a.Item2] = 1;
        }

        for (int j = 0; j < dotsAscii.GetLength(1); j++)
        {
            for (int i = 0; i < dotsAscii.GetLength(0); i++)
            {
                Console.Write(dotsAscii[i, j] == 1 ? '#' : '.');
            }
            Console.WriteLine();
        }

    }

}
