using System;

static class Extension
{
    public static IEnumerable<T> SliceColumn<T>(this T[,] array, int column)
    {
        for (var i = 0; i < array.GetLength(0); i++)
        {
            yield return array[i, column];
        }
    }

    public static T[,] FilterMatrix<T>(this T[,] array, int[] removeRows) {
        T[,] res = new T[array.GetLength(0) - removeRows.Length, array.GetLength(1)];

        int h = 0;
        for (var i = 0; i < array.GetLength(0); i++) 
        {
            bool contains = false;
            for (var k = 0; k < removeRows.Length; k++)
            {
                if (removeRows[k] == i) 
                {
                    contains = true;
                    break;
                }
            }
            if (contains) { 
                continue; 
            }
            for (int j = 0; j < array.GetLength(1); j++)
            {
                res[h, j] = array[i, j];
            }
            h++;
        }

        return res;
    }

}

internal class AC03
{

    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input3.txt");
        // create matrix
        bool[,] matrix = new bool[lines.Length, lines[0].Length];
        //fill matrix
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = lines[i][j] == '1';
            }
        }
        // get best numbers
        string binaryNumber = "";
        string oppositeNumber = "";
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            var cnt = matrix.SliceColumn(j).ToArray().Count(x => x == true);
            var deb = matrix.SliceColumn(j).ToArray();
            if (cnt >= lines.Length / 2)
            {
                binaryNumber += '1';
                oppositeNumber += '0';
            } else {
                binaryNumber += '0';
                oppositeNumber += '1';
            }
        }

        var gamma = Convert.ToInt32(binaryNumber, 2);
        var epsilon = Convert.ToInt32(oppositeNumber, 2);

        Console.WriteLine(gamma * epsilon);
    }

    public static void Run2(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input3.txt");
        // create matrix
        bool[,] matrix = new bool[lines.Length, lines[0].Length];
        //fill matrix
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = lines[i][j] == '1';
            }
        }
        bool[,] savedMatrix = matrix.Clone() as bool[,];

        
        for (int i = 0; i < lines[0].Length; i++)
        {
            if (matrix.GetLength(0) == 1)
            {
                break;
            }
            bool keepOnes = matrix.SliceColumn(i).ToArray().Count(x => x == true) >= matrix.GetLength(0) / 2.0;

            int[] removeRows = (from k in Enumerable.Range(0, matrix.GetLength(0)) where keepOnes ? !matrix[k, i] : matrix[k, i] select k).ToArray();

            matrix = matrix.FilterMatrix(removeRows);
        }

        string binaryNumber = "";
        for (int i = 0; i < matrix.GetLength(1); i++) {
            binaryNumber += matrix[0, i] ? '1' : '0';
        }
        var oxygen = Convert.ToInt32(binaryNumber, 2);


        matrix = savedMatrix;

        for (int i = 0; i < lines[0].Length; i++)
        {
            if (matrix.GetLength(0) == 1)
            {
                break;
            }
            bool keepOnes = matrix.SliceColumn(i).ToArray().Count(x => x == true) < matrix.GetLength(0) / 2.0;

            int[] removeRows = (from k in Enumerable.Range(0, matrix.GetLength(0)) where keepOnes ? !matrix[k, i] : matrix[k, i] select k).ToArray();

            matrix = matrix.FilterMatrix(removeRows);
        }

        binaryNumber = "";
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            binaryNumber += matrix[0, i] ? '1' : '0';
        }
        var scrubber = Convert.ToInt32(binaryNumber, 2);

        Console.WriteLine(scrubber * oxygen);
    }
}
