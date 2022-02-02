public class Plane
{
    List<List<int>> plane = new List<List<int>>();

    public Plane(string[] lines)
    {
        foreach (string line in lines)
        {
            plane.Add(line.Trim().Select(ch => ch == '>' ? 1 : (ch == 'v' ? 2 : 0)).ToList());
        }
    }

    public int MoveEast()
    {
        List<(int, int)> canMove = new List<(int, int)>();
        for (int i = 0; i < plane.Count; i++)
        {
            for (int j = 0; j < plane[i].Count; j++)
            {
                if (plane[i][j] == 1 && plane[i][(j + 1) % plane[i].Count] == 0)
                {
                    canMove.Add((i, j));
                }
            }
        }

        foreach (var (i, j) in canMove)
        {
            plane[i][j] = 0;
            plane[i][(j + 1) % plane[i].Count] = 1;
        }

        return canMove.Count;
    }

    public int MoveSouth()
    {
        List<(int, int)> canMove = new List<(int, int)>();
        for (int i = 0; i < plane.Count; i++)
        {
            for (int j = 0; j < plane[i].Count; j++)
            {
                if (plane[i][j] == 2 && plane[(i+1) % plane.Count][j] == 0)
                {
                    canMove.Add((i, j));
                }
            }
        }

        foreach (var (i, j) in canMove)
        {
            plane[i][j] = 0;
            plane[(i + 1) % plane.Count][j] = 2;
        }

        return canMove.Count;
    }

   
}

internal class AC25
{

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input25.txt");

        Plane plane = new Plane(lines);

        int movesE;
        int movesS;
        int step = 0;
        do
        {
            movesE = plane.MoveEast();
            movesS = plane.MoveSouth();
            step++;
            Console.WriteLine($"{step} {movesE} {movesS}");
        } while (movesE + movesS > 0);
    }
}
