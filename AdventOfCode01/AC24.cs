
public class Instruction
{
    public string instruction;
    int dest;
    int second;
    bool applyState;

    public Instruction(string line)
    {
        string[] split = line.Trim().Split(" ");
        instruction = split[0];
        dest = split[1][0] - 'w';
        if (split.Length > 2)
        {
            try
            {
                second = Convert.ToInt32(split[2]);
                applyState = false;
            }
            catch (FormatException)
            {
                second = split[2][0] - 'w';
                applyState = true;
            }

        }
    }

    public (long[], int) Perform(long[] state, char input)
    {
        int consumed = 0;
        switch (instruction)
        {
            case "inp": state[dest] = input - '0'; consumed = 1; break;
            case "add": state[dest] = state[dest] + (applyState ? state[second] : second); break;
            case "mul": state[dest] = state[dest] * (applyState ? state[second] : second); break;
            case "div": state[dest] = state[dest] / (applyState ? state[second] : second); break;
            case "mod": state[dest] = state[dest] % (applyState ? state[second] : second); break;
            case "eql": state[dest] = state[dest] == (applyState ? state[second] : second) ? 1 : 0; break;
            default: throw new ArgumentException();
        }
        return (state, consumed);
    }
}


public class Program
{
    List<Instruction> instructions = new List<Instruction>();

    public Program(string[] lines, int stop = 50, int skip = 0)
    {
        foreach (string line in lines)
        {
            if (line.StartsWith("inp") && skip > 0) { skip--; continue; }

            if (line.StartsWith("inp")) stop--;
            if (stop < 0) break;
            instructions.Add(new Instruction(line));            
        }
    }

    long[] Preprocess(string numberStr, long[] state) {
        long i1 = numberStr[0] - '0';
        long i2 = numberStr[1] - '0';
        long i3 = numberStr[2] - '0';
        long i4 = numberStr[3] - '0';
        long x = (i3 != 9 || i4 != 1 ? 1 : 0);
        state[0] = i4;
        state[1] = x;
        state[2] = (i4 + 1) * x;
        state[3] = ((i1 + 13) * 26 + (i2 + 10)) * (25 * x + 1) + (i4 + 1) * x;
        return state;
    }

    public long[] ProcessNumber(long number, long[]? begState = null)
    {
        string numberStr = Convert.ToString(number) + "E";
        long[] state;
        if (begState == null) state = new long[4];
        else state = begState;
        int idx = 0;
        //state = Preprocess(numberStr, state);
        //idx += 4;
        foreach (Instruction instruction in instructions)
        {
            (state, int consumed) = instruction.Perform(state, numberStr[idx]);
            idx += consumed;
        }
        return state;
    }
}

public class Theory
{
    public static bool Theoretize(long number, long[] state)
    {
        string numberStr = Convert.ToString(number) + "E";
        long i1 = numberStr[0] - '0';
        long i2 = numberStr[1] - '0';
        long i3 = numberStr[2] - '0';
        long i4 = numberStr[3] - '0';

        // return state[0] == i1 && state[1] == 1 && state[2] == i1 + 13 && state[3] == i1 + 13;
        // return state[0] == i2 && state[1] == 1 && state[2] == i2 + 10 && state[3] == (i1 + 13) * 26 + i2 + 10;
        //return state[0] == i3 && state[1] == 1 && state[2] == i3 + 3 && state[3] == ((i1 + 13) * 26 + (i2 + 10)) * 26 + (i3 + 3);
        long x = (i3 != 9 || i4 != 1 ? 1 : 0);
        //long x5 = ((i1 + 13) * 26 + (i2 + 10)) * (25 * x + 1) + (i4 + 1) * x
        return state[0] == i4 && state[1] == x && state[2] == (i4+1)* x && state[3] == ((i1 + 13) * 26 + (i2 + 10)) * (25 * x + 1) + (i4 + 1) * x;
        //return state[0] == i5 && state[1] == x5 && state[2]
    }
}


internal class AC24
{

    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\input24.txt");

        int instrNum = 50;
        Program program = new Program(lines, instrNum);

        long number = 69914999975369L;
        // long number = 14911675311114L;

        long[] state = program.ProcessNumber(number);
        Console.WriteLine($"{number} {state[0]} {state[1]} {state[2]} {state[3]}");
        /*do
        {
            long[] state = program.ProcessNumber(number);
            string theory = Theory.Theoretize(number, state) ? "OK" : "WRONG!";
            if (theory != "OK") throw new Exception(theory);  
            // Console.WriteLine($"{number} {state[0]} {state[1]} {state[2]} {state[3]} {theory}");

            if (state[3] == 0) break;
            do
            {
                number--;
            } while (number.ToString().Contains('0'));
        } while (number >= Math.Pow(10, instrNum - 1));
        */
    }
}

// UNFINISHED - I did the task on paper in the end. 



