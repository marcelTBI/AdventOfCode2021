
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


internal class AC22
{

    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input22.txt");

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
            if (switchOn) on = leds.TurnOn(bx,ex,by,ey,bz,ez);
            else on = leds.TurnOff(bx, ex, by, ey, bz, ez);

            Console.WriteLine(on);
        }

    }
}



