internal class AC10
{
    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input10.txt");

        // go through lines and build 
        Dictionary<char, int> scores = new Dictionary<char, int>() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
        Dictionary<char, char> opposite = new Dictionary<char, char>() { { ')', '(' }, { ']', '[' }, { '}', '{' }, { '>', '<' }, { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' } };
        Dictionary<char, int> scores2 = new Dictionary<char, int>() { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };
        int points = 0;

        List<long> scoresIncomplete = new List<long>();
        foreach (string line in lines)
        {
            Stack<char> pars = new Stack<char>();

            bool corrupted = false;
            for (int i = 0; i < line.Length; i++)
            {
                if ("([{<".Contains(line[i]))
                {
                    pars.Push(line[i]);
                }
                if (")]}>".Contains(line[i]))
                {
                    char first = pars.Pop();
                    if (first != opposite[line[i]])
                    {
                        points += scores[line[i]];
                        corrupted = true;
                        break;
                    }
                }
            }

            if (!corrupted)
            {
                long score = 0;
                while (pars.Count > 0)
                {
                    score *= 5;
                    score += scores2[pars.Pop()];
                }
                scoresIncomplete.Add(score);
            }
        }

        // print output 1 
        Console.WriteLine(points);

        // print output 2
        scoresIncomplete.Sort();
        Console.WriteLine(scoresIncomplete[(scoresIncomplete.Count - 1) / 2]);
    }
}
