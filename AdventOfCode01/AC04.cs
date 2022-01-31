using System;


public class Bingo
{
    public List<List<int>> numbers = new List<List<int>>();
    public List<List<int>> check = new List<List<int>>();

    public bool alreadyWon = false;

    public bool Add(List<int> row)
    {
        if (numbers.Count == 5) {
            throw new InvalidOperationException("Already full");    
        }
        numbers.Add(row);
        check.Add(new List<int> {0,0,0,0,0});
        return numbers.Count == 5; 
    }

    public bool Mark(int num)
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 0; j < numbers[i].Count; j++)
            {
                if (numbers[i][j] == num) {
                    check[i][j] = 1;
                    if (check[i][0] + check[i][1] + check[i][2] + check[i][3] + check[i][4] == 5) 
                    {
                        alreadyWon = true;
                        return true;
                    }
                    if (check[0][j] + check[1][j] + check[2][j] + check[3][j] + check[4][j] == 5)
                    {
                        alreadyWon = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public List<int> GetUnchecked()
    {
        List<int> res = new List<int>();
        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 0; j < numbers[i].Count; j++)
            {
                if (check[i][j] == 0)
                {
                    res.Add(numbers[i][j]);
                }
            }
        }

        return res;
    }
}

internal class AC04
{

    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input4.txt");

        // read numbers
        var numbers = (from str in lines[0].Split(',') select Convert.ToInt32(str)).ToArray();

        List<Bingo> bingos = new List<Bingo>();
        bool startNewBingo = true;

        // read bingos 
        for (int i = 1; i < lines.Length; i++) {
            if (lines[i].Trim() == "")
            { 
                continue; 
            }
            var a = lines[i].Trim().Split();
            var ints = (from str in lines[i].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) select Convert.ToInt32(str)).ToList();
            if (ints.Count() == 5)
            {
                if (startNewBingo)
                {
                    bingos.Add(new Bingo());
                    startNewBingo = false;
                }
                startNewBingo = bingos[^1].Add(ints);
            }
        }

        // mark bingos
        foreach (int num in numbers)
        {
            foreach (Bingo bingo in bingos)
            {
                if (!bingo.alreadyWon && bingo.Mark(num))
                {
                    Console.WriteLine(bingo.GetUnchecked().Sum() * num);
                }
            }
        }

    }

}
