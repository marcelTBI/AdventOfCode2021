internal class AC14
{

    public static string ApplyRules(Dictionary<string, string> rules, string template)
    {
        string newTemplate = "";
        for (int i = 0; i < template.Length - 1; i++)
        {
            newTemplate += template[i] + rules[Char.ToString(template[i]) + Char.ToString(template[i + 1])];
        }
        return newTemplate + template[^1];
    }

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input14.txt");

        // parse
        string template = lines[0].Trim();
        Dictionary<string, string> rules = new Dictionary<string, string>();

        foreach (string line in lines[2..])
        {
            string[] parts = line.Split(' ');
            rules.Add(parts[0], parts[2]);
        }

        // apply rules X times
        for (int steps=0; steps < 10; steps++)
        {
            Console.WriteLine(steps);
            template = ApplyRules(rules, template);
        }

        // count 
        var cnt = template.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        Console.WriteLine(cnt.Values.Max() - cnt.Values.Min());
        // var max = cnt.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        // var min = cnt.Aggregate((l, r) <= l.Value > r.Value ? l : r).Key;

    }

    public static Dictionary<string, long> ApplyRules2(Dictionary<string, string> rules, Dictionary<string, long> counts)
    {
        Dictionary<string, long> countsRet = new Dictionary<string, long>();
        foreach (KeyValuePair<string, long> entry in counts)
        {
            var inside = rules[entry.Key];
            var leftTemp = entry.Key[0] + inside;
            var rightTemp = inside + entry.Key[1];
            if (!countsRet.ContainsKey(leftTemp)) countsRet[leftTemp] = 0;
            if (!countsRet.ContainsKey(rightTemp)) countsRet[rightTemp] = 0;
            countsRet[leftTemp] += entry.Value;
            countsRet[rightTemp] += entry.Value;
        }
        return countsRet;
    }

    public static void Run2()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input14.txt");

        // parse
        string template = lines[0].Trim();
        Dictionary<string, string> rules = new Dictionary<string, string>();
        foreach (string line in lines[2..])
        {
            string[] parts = line.Split(' ');
            rules.Add(parts[0], parts[2]);
        }

        Dictionary<string, long> counts = new Dictionary<string, long>();
        for (int i = 0; i < template.Length-1; i++)
        {
            var temp = template[i..(i + 2)];
            if (!counts.ContainsKey(temp))
            {
                counts[temp] = 0;
            }
            counts[temp]++;
        }

        // apply rules X times
        for (int steps = 0; steps < 40; steps++)
        {
            Console.WriteLine(steps);
            counts = ApplyRules2(rules, counts);
        }

        // count 
        Dictionary<char, long> cnts = new Dictionary<char, long>();
        foreach (KeyValuePair<string, long> entry in counts)
        {
            char ch1 = entry.Key[0];
            char ch2 = entry.Key[1];
            if (!cnts.ContainsKey(ch1)) cnts[ch1] = 0;
            if (!cnts.ContainsKey(ch2)) cnts[ch2] = 0;
            cnts[ch1] += entry.Value;
            cnts[ch2] += entry.Value;
        }
        cnts[template[0]]++;
        cnts[template[^1]]++;

        Console.WriteLine((cnts.Values.Max() - cnts.Values.Min())/2);

    }

}
