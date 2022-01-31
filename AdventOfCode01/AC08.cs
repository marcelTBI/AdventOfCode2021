using System;

internal class AC08
{
    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Silence\source\repos\AdventOfCode01\AdventOfCode01\input8.txt");

        // parse lines
        List<string[]> digits = new List<string[]>();
        List<string[]> quartets = new List<string[]>();
        foreach (string line in lines)
        {
            string[] split = line.Split('|');
            quartets.Add(split[1].Trim().Split(' '));
            digits.Add(split[0].Trim().Split(' '));
        }

        // get lengths of quartets
        int appear = 0;
        foreach (string[] quartet in quartets) {
            foreach (string dig in quartet)
            {
                if (dig.Length == 7 || dig.Length == 4 || dig.Length == 3 || dig.Length == 2)
                {
                    appear++;
                }
            }
        }
        

        // print appearance
        Console.WriteLine(appear);
    }

    public static bool Contain(string a, string b) {
        foreach (char c in b)
        {
            if (!a.Contains(c)) return false;
        }
        return true;
    }

    public static int AssignNum(string str, Dictionary<int, string> allocatedFar)
    {
        if (str.Length == 7) return 8;
        if (str.Length == 4) return 4;
        if (str.Length == 3) return 7;
        if (str.Length == 2) return 1;
        try
        {
            if (str.Length == 6 && Contain(str, allocatedFar[4])) return 9;
            if (str.Length == 6 && !Contain(str, allocatedFar[1])) return 6;
            if (str.Length == 6) return 0;
            if (str.Length == 5 && Contain(str, allocatedFar[7])) return 3;
            if (str.Length == 5 && Contain(allocatedFar[6], str)) return 5;
            if (str.Length == 5) return 2;
        } catch
        {
            return -1;
        }

        return -1;
    }

    public static Dictionary<string, TKey> Reverse<TKey> (IDictionary<TKey, string> source)
    {
        var dictionary = new Dictionary<string, TKey>();
        foreach (var entry in source)
        {
            if (!dictionary.ContainsKey(SortString(entry.Value)))
                dictionary.Add(SortString(entry.Value), entry.Key);
        }
        return dictionary;
    }

    static string SortString(string input)
    {
        char[] characters = input.ToArray();
        Array.Sort(characters);
        return new string(characters);
    }

    public static void Run2(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Silence\source\repos\AdventOfCode01\AdventOfCode01\input8.txt");

        // parse lines
        List<string[]> digits = new List<string[]>();
        List<string[]> quartets = new List<string[]>();
        foreach (string line in lines)
        {
            string[] split = line.Split('|');
            quartets.Add(split[1].Trim().Split(' '));
            digits.Add(split[0].Trim().Split(' '));
        }

        int sum = 0;
        for (int i=0; i<digits.Count(); i++)
        {
            var dgs = digits[i];
            var qua = quartets[i];

            // go through allocation
            Dictionary<int, string> allocatedFar = new Dictionary<int, string>();
            while (allocatedFar.Count() != 10)
            {
                foreach (string d in dgs)
                {
                    int num = AssignNum(d, allocatedFar);
                    if (num != -1 && !allocatedFar.ContainsKey(num))
                    {
                        allocatedFar.Add(num, d);
                    }
                }
            }

            // swap allocation 
            Dictionary<string, int> allocatedInv = Reverse(allocatedFar);

            // assign to digits
            var finalNum = 0;
            foreach (string q in qua)
            {
                int num = allocatedInv[SortString(q)];
                finalNum = 10 * finalNum + num;
            }
            sum += finalNum;

        }


        // print appearance
        Console.WriteLine(sum);
    }
}
