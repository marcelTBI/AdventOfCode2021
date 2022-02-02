public class SnailNum
{
    public int depth;
    public SnailNum? left = null;
    public SnailNum? right = null;
    public SnailNum? parent = null;
    public int literal = -1;
    public int length = 0;

    public SnailNum(string snailNum, SnailNum? parent = null)
    {
        this.parent = parent;
        if (parent == null) depth = 0;
        else depth = parent.depth + 1;
        for (int i = 0; i < snailNum.Length;)
        {
            if (snailNum[i] == '[')
            {
                left = new SnailNum(snailNum[(i + 1)..], this);
                length = left.length + 1;
            }
            else if (snailNum[i] == ',')
            {
                right = new SnailNum(snailNum[(i + 1)..], this);
                length = right.length + 1;
            }
            else if (snailNum[i] == ']')
            {
                length = i + 1;
                break;
            }
            else
            {
                literal = snailNum[i] - '1' + 1;
                length = 1;
                break;
            }
            i += length;
        }
    }

    public SnailNum(int literal, SnailNum parent)
    {
        this.literal = literal;
        this.parent = parent;
        depth = parent.depth + 1;
    }

    public SnailNum(SnailNum first, SnailNum second)
    {
        depth = 0;
        left = first;
        left.IncreaseDepth();
        left.parent = this;
        right = second;
        right.IncreaseDepth();
        right.parent = this;
    }

    public void IncreaseDepth()
    {
        depth++;
        if (left != null) left.IncreaseDepth();
        if (right != null) right.IncreaseDepth();
    }

    public SnailNum GetAncestor()
    {
        if (parent == null) return this;
        else return parent.GetAncestor();
    }

    public void ExplodeLeft()
    {
        if (left == null) throw new Exception();
        if (right == null) throw new Exception();
        if (parent == null) throw new Exception();
        if (left.left == null) throw new Exception();
        if (left.right == null) throw new Exception();
        int leftLiteral = left.left.literal;
        int rightLiteral = left.right.literal;
        left = new SnailNum(0, this);
        right.AddLeft(rightLiteral);
        parent.TryAddLeft(leftLiteral, this);

    }

    public void ExplodeRight()
    {
        if (left == null) throw new Exception();
        if (right == null) throw new Exception();
        if (parent == null) throw new Exception();
        if (right.left == null) throw new Exception();
        if (right.right == null) throw new Exception();
        int leftLiteral = right.left.literal;
        int rightLiteral = right.right.literal;
        right = new SnailNum(0, this);
        left.AddRight(leftLiteral);
        parent.TryAddRight(rightLiteral, this);
    }

    public void AddLeft(int add)
    {
        if (literal != -1)
        {
            literal += add;
        }
        else
        {
            if (left == null) throw new Exception();
            left.AddLeft(add);
        }

    }

    public void AddRight(int add)
    {
        if (literal != -1)
        {
            literal += add;
        }
        else
        {
            if (right == null) throw new Exception();
            right.AddRight(add);
        }
    }

    public void TryAddLeft(int add, SnailNum child)
    {
        if (left != child)
        {
            if (left == null) throw new Exception();
            left.AddRight(add);
        }
        else if (parent != null)
        {
            parent.TryAddLeft(add, this);
        }
    }

    public void TryAddRight(int add, SnailNum child)
    {
        if (right != child)
        {
            if (right == null) throw new Exception();
            right.AddLeft(add);
        }
        else if (parent != null)
        {
            parent.TryAddRight(add, this);
        }
    }

    public int TryExplode()
    {
        int ret = 0;
        if (depth == 3 && left != null && left.literal == -1)
        {
            ExplodeLeft();
            ret += 1;
        }
        else if (depth == 3 && right != null && right.literal == -1)
        {
            ExplodeRight();
            ret += 1;
        }
        else
        {
            if (left != null && ret == 0) ret += left.TryExplode();
            if (right != null && ret == 0) ret += right.TryExplode();
        }

        return ret;
    }

    public int TrySplit()
    {
        int ret = 0;
        if (literal >= 10)
        {
            left = new SnailNum(literal / 2, this);
            right = new SnailNum(literal - left.literal, this);
            literal = -1;
            return 1;
        }
        else if (literal == -1)
        {
            if (left == null) throw new Exception();
            if (right == null) throw new Exception();
            if (ret == 0) ret += left.TrySplit();
            if (ret == 0) ret += right.TrySplit();
        }
        return ret;
    }

    public void Reduce()
    {
        int ret;
        do
        {
            ret = TryExplode();
            //Console.Write("Explode " + Convert.ToString(ret) + " ");  Print();
            if (ret == 0)
            {
                ret = TrySplit();
                //Console.Write("Split   " + Convert.ToString(ret) + " "); Print();
            }
        } while (ret != 0);
    }

    public void Print()
    {
        if (literal != -1)
        {
            Console.Write(literal);
        }
        else
        {
            Console.Write('[');
            if (left == null) throw new Exception();
            left.Print();
            Console.Write(',');
            if (right == null) throw new Exception();
            right.Print();
            Console.Write(']');
            //Console.Write(depth);
            if (parent == null) Console.WriteLine();
        }
    }

    public long Magnitude()
    {
        if (literal != -1)
        {
            return (long)literal;
        }
        if (left == null) throw new Exception();
        if (right == null) throw new Exception();
        return 3 * left.Magnitude() + 2 * right.Magnitude();
    }
}
internal class AC18
{
    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input18.txt");
        //lines[0] = "[[[[4,3],4],4],[7,[[8,4],9]]]";
        //lines[1] = "[1,1]";

        // parse first
        SnailNum first = new SnailNum(lines[0]);

        for (int i = 1; i < lines.Length; i++)
        {
            first = new SnailNum(first, new SnailNum(lines[i]));
            //first.Print();
            first.Reduce();
            //first.Print(); 
        }

        first.Print();
        Console.WriteLine(first.Magnitude());

    }

    public static void Run2()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input18.txt");
        //lines[0] = "[[[[4,3],4],4],[7,[[8,4],9]]]";
        //lines[1] = "[1,1]";

        long largest = 0;
        for (int i = 0; i < lines.Length; i++)
            for (int j = i + 1; j < lines.Length; j++)
            {
                SnailNum addition = new SnailNum(new SnailNum(lines[i]), new SnailNum(lines[j]));
                addition.Reduce();
                largest = Math.Max(addition.Magnitude(), largest);
            }

        Console.WriteLine(largest);

    }
}
