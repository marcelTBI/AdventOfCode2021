
public class Leds
{
    HashSet<(int, int, int)> ledOn = new HashSet<(int, int, int)>();

    public long TurnOn(int bx, int ex, int by, int ey, int bz, int ez)
    {
        for (int x = bx; x <= ex; x++)
            for (int y = by; y <= ey; y++)
                for (int z = bz; z <= ez; z++)
                    ledOn.Add((x, y, z));

        return ledOn.Count;
    }

    public long TurnOff(int bx, int ex, int by, int ey, int bz, int ez)
    {
        for (int x = bx; x <= ex; x++)
            for (int y = by; y <= ey; y++)
                for (int z = bz; z <= ez; z++)
                    if (ledOn.Contains((x, y, z))) ledOn.Remove((x, y, z));

        return ledOn.Count;
    }
}

public class Cube
{
    public int Xbeg { get; set; }
    public int Ybeg { get; set; }
    public int Zbeg { get; set; }
    public int Xend { get; set; }
    public int Yend { get; set; }
    public int Zend { get; set; }

    public Cube(int bx, int by, int bz, int ex, int ey, int ez)
    {
        // if (!Cube.Valid(bx, by, bz, ex, ey, ez)) throw new Exception("End is lower than beginning.");
        Xbeg = bx;
        Ybeg = by;
        Zbeg = bz;
        Xend = ex;
        Yend = ey;
        Zend = ez;
    }

    public bool Valid()
    {
        return Cube.Valid(Xbeg, Ybeg, Zbeg, Xend, Yend, Zend);
    }

    public static bool Valid(int bx, int by, int bz, int ex, int ey, int ez)
    {
        return ex >= bx && ey >= by && ez >= bz;
    }

    public bool Intersects(Cube second)
    {
        return Math.Min(Xend, second.Xend) >= Math.Max(Xbeg, second.Xbeg) && Math.Min(Yend, second.Yend) >= Math.Max(Ybeg, second.Ybeg) && Math.Min(Zend, second.Zend) >= Math.Max(Zbeg, second.Zbeg);
    }

    public List<Cube> Subtract(Cube second)
    {
        List<Cube> result = new List<Cube>();

        // try to add all 6 possible Cubes
        // move X     
        Cube c;
        c = new Cube(Xbeg, Ybeg, Zbeg, second.Xbeg - 1, Yend, Zend);        
        if (c.Valid()) result.Add(c);
        c = new Cube(second.Xend + 1, Ybeg, Zbeg, Xend, Yend, Zend);
        if (c.Valid()) result.Add(c);
        // move Y
        c = new Cube(Math.Max(Xbeg, second.Xbeg), Ybeg, Zbeg, Math.Min(Xend, second.Xend), second.Ybeg - 1, Zend);
        if (c.Valid()) result.Add(c);
        c = new Cube(Math.Max(Xbeg, second.Xbeg), second.Yend + 1, Zbeg, Math.Min(Xend, second.Xend), Yend, Zend);
        if (c.Valid()) result.Add(c);
        // move Z
        c = new Cube(Math.Max(Xbeg, second.Xbeg), Math.Max(Ybeg, second.Ybeg), Zbeg, Math.Min(Xend, second.Xend), Math.Min(Yend, second.Yend), second.Zbeg - 1);
        if (c.Valid()) result.Add(c);
        c = new Cube(Math.Max(Xbeg, second.Xbeg), Math.Max(Ybeg, second.Ybeg), second.Zend + 1, Math.Min(Xend, second.Xend), Math.Min(Yend, second.Yend), Zend);
        if (c.Valid()) result.Add(c);

        return result;
    }

    public long OnLeds()
    {
        return (long)(Xend - Xbeg + 1) * (long)(Yend - Ybeg + 1) * (long)(Zend - Zbeg + 1);
    }

}


internal class AC22
{

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input22.txt");

        Leds leds = new Leds();

        for (int i = 0; i < lines.Length; i++)
        {
            bool switchOn = lines[i][0..3] == "on ";
            var split = lines[i].Trim().Split('=');
            var splitx = split[1].Split('.');
            int bx = Convert.ToInt32(splitx[0]);
            int ex = Convert.ToInt32(splitx[2][..^2]);
            var splity = split[2].Split('.');
            int by = Convert.ToInt32(splity[0]);
            int ey = Convert.ToInt32(splity[2][..^2]);
            var splitz = split[3].Split('.');
            int bz = Convert.ToInt32(splitz[0]);
            int ez = Convert.ToInt32(splitz[2]);

            long on;
            if (switchOn) on = leds.TurnOn(bx, ex, by, ey, bz, ez);
            else on = leds.TurnOff(bx, ex, by, ey, bz, ez);

            Console.WriteLine(on);
        }

    }

    public static void Run2()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input22.txt");

        List<Cube> cubes = new List<Cube>();

        for (int i = 0; i < lines.Length; i++)
        {
            bool switchOn = lines[i][0..3] == "on ";
            var split = lines[i].Trim().Split('=');
            var splitx = split[1].Split('.');
            int bx = Convert.ToInt32(splitx[0]);
            int ex = Convert.ToInt32(splitx[2][..^2]);
            var splity = split[2].Split('.');
            int by = Convert.ToInt32(splity[0]);
            int ey = Convert.ToInt32(splity[2][..^2]);
            var splitz = split[3].Split('.');
            int bz = Convert.ToInt32(splitz[0]);
            int ez = Convert.ToInt32(splitz[2]);

            Stack<Cube> addStack = new Stack<Cube>();
            addStack.Push(new Cube(bx, by, bz, ex, ey, ez));

            if (switchOn)
            {
                while (addStack.Count > 0)
                {
                    Cube toAdd = addStack.Pop();
                    bool intersecting = false;
                    foreach (Cube cube in cubes)
                    {
                        if (cube.Intersects(toAdd))
                        {
                            List<Cube> splitCube = toAdd.Subtract(cube);
                            foreach (Cube cube2 in splitCube) addStack.Push(cube2);
                            intersecting = true;
                            break;
                        }
                    }
                    if (!intersecting) cubes.Add(toAdd);
                }
            }
            else
            {
                Cube toAdd = addStack.Pop();

                bool intersecting = false;
                do
                {
                    intersecting = false;
                    for (int j = 0; j < cubes.Count; j++)
                    {
                        if (cubes[j].Intersects(toAdd))
                        {
                            intersecting = true;
                            foreach (Cube c in cubes[j].Subtract(toAdd)) cubes.Add(c);
                            cubes.RemoveAt(j);
                            break;
                        }
                    }
                } while (intersecting);
            }

            long onLeds = 0;
            foreach (Cube cube in cubes) onLeds += cube.OnLeds();
            Console.WriteLine(onLeds);
        }

    }
}



