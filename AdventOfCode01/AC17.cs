internal class AC17
{
    public static int SimulateFall(int vx, int vy, int x1, int x2, int y1, int y2)
    {
        int x = 0;
        int y = 0;
        int maxy = 0;
        while (true)
        {
            x += vx;
            y += vy;
            maxy = Math.Max(maxy, y);
            if (x >= x1 && x <= x2 && y <= y1 && y >= y2) return maxy;
            if (y < y2) return -1;
            vx = Math.Max(0, vx-1);
            vy -= 1;
        }
    }

    public static void Run1()
    {
        // read file
        //string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input16.txt");
        //lines[0] = "9C0141080250320F1802104A08";
        /*int x1 = 20;
        int x2 = 30;
        int y1 = -5;
        int y2 = -10;*/

        int x1 = 81;
        int x2 = 129;
        int y1 = -108;
        int y2 = -150;

        int maximal = 0;
        int maxx = 0;
        int maxy = 0;
        int cnt = 0;
        for (int x = (int)Math.Sqrt(x1/2); x <= x2; x++)
            for (int y = y2; y <= -y2; y++)
            {
                int fallHeight = SimulateFall(x, y, x1, x2, y1, y2);
                if (fallHeight > maximal)
                {
                    maximal = fallHeight;
                    maxx = x;       
                    maxy = y;
                }
                if (fallHeight != -1)
                {
                    cnt++;
                }
            }

        Console.WriteLine(maximal);
        Console.WriteLine(maxx);
        Console.WriteLine(maxy);
        Console.WriteLine(cnt);
    }
}



