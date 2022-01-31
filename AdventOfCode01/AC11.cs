internal class AC11
{
    public static void Flash(int[,] energy, int x, int y) 
    {
        energy[x, y] = 11;
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                if (x + dx >= 0 && 10 > x + dx && y + dy >= 0 && 10 > y + dy)
                {
                    if (energy[x + dx, y + dy] < 10)
                    {
                        energy[x + dx, y + dy]++;
                        if (energy[x + dx, y + dy] == 10)
                        {
                            Flash(energy, x + dx, y + dy);
                        }
                    }
                }
            }
    }


    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input11.txt");

        // parse lines 
        int[,] energy = new int[10, 10];
        for (int i = 0; i < energy.GetLength(0); i++) {
            for (int j = 0; j < energy.GetLength(1); j++)
            {
                energy[i, j] = Convert.ToInt32(Char.ToString(lines[i][j]));
            }
        }

        // increase and count flashes
        int totalFlash = 0;
        for (int steps = 0; steps < 1000; steps++)
        {
            for (int i = 0; i < energy.GetLength(0); i++)
            {
                for (int j = 0; j < energy.GetLength(1); j++)
                {
                    energy[i, j]++;
                }
            }

            for (int i = 0; i < energy.GetLength(0); i++)
                for (int j = 0; j < energy.GetLength(1); j++)
                    if (energy[i, j] == 10) Flash(energy, i, j);

            int flashes = 0;
            //Console.WriteLine();
            for (int i = 0; i < energy.GetLength(0); i++)
            {
                for (int j = 0; j < energy.GetLength(1); j++)
                {
                    if (energy[i, j] >= 10)
                    {
                        energy[i, j] = 0;
                        flashes += 1;
                    }
                    //Console.Write(energy[i, j]);
                }
                //Console.WriteLine();
            }

            totalFlash += flashes;
            if (flashes == 100)
            {
                Console.WriteLine("All Flash " + Convert.ToString(steps + 1));
                break;
            }
            if (steps == 99)
            {
                Console.WriteLine(totalFlash);
            }
        }
    }
}
