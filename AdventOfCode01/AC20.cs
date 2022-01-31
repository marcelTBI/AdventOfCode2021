
public class Enhancement
{
    List<bool> alg;
    Dictionary<int, int> usage = new Dictionary<int, int>();

    public Enhancement(string alg)
    {
        this.alg = (from a in alg select a == '#').ToList();
    }

    public bool GetEnhance(int val)
    {
        if (!usage.ContainsKey(val)) usage[val] = 0;
        usage[val] += 1;
        return alg[val];
    }
}

public class Image
{
    List<List<bool>> image;
    bool defaultPixel = false;

    public Image(string[] imageString)
    {
        image = new List<List<bool>>();
        foreach (string line in imageString)
        {
            image.Add((from a in line select a == '#').ToList());
        }
    }

    public List<List<bool>> Clone()
    {
        List<List<bool>> image2 = new List<List<bool>>();
        for (int i = 0; i < image.Count; i++)
            image2.Add(new List<bool>(image[i]));
        return image2;
    }

    public void Enhance(Enhancement enh)
    {
        List<List<bool>> image2 = Clone();
        for (int i = 1; i < image.Count - 1; i++)
            for (int j = 1; j < image[i].Count - 1; j++)
            {
                int val = 0;
                for (int x = i - 1; x <= i + 1; x++)
                    for (int y = j - 1; y <= j + 1; y++)
                    {
                        val *= 2;
                        val += image[x][y] ? 1 : 0;
                    }
                image2[i][j] = enh.GetEnhance(val);
            }
        image = image2;
        if (enh.GetEnhance(0) && !defaultPixel) defaultPixel = true;
        else if (!enh.GetEnhance(511) && defaultPixel) defaultPixel = false;
    }

    public void Enlarge()
    {
        int length = image[0].Count;
        foreach (List<bool> line in image)
        {
            line.Insert(0, defaultPixel);
            line.Add(defaultPixel);
        }
        image.Insert(0, Enumerable.Repeat(defaultPixel, length + 2).ToList());
        image.Add(Enumerable.Repeat(defaultPixel, length + 2).ToList());
    }

    public void Shorten()
    {
        int length = image.Count;

        for (int i = 0; i < length; i++)
        {
            image[i].RemoveAt(image[i].Count - 1);
            image[i].RemoveAt(0);
        }
        image.RemoveAt(length - 1);
        image.RemoveAt(0);
    }

    public new string ToString()
    {
        string ret = "";
        foreach (List<bool> line in image)
        {
            ret += String.Join("", line.Select(ch => ch ? "#" : "."));
            ret += "\n";
        }
        return ret[..^1];
    }

    public int NumLightPixels()
    {
        int ret = 0;
        foreach (List<bool> line in image)
            ret += line.Count(c => c);
        return ret;
    }
}

internal class AC20
{

    public static void Run1(string[] args)
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input20.txt");

        // parse enhacement alg.
        Enhancement enh = new Enhancement(lines[0]);

        // parse image
        Image image = new Image(lines[2..]);

        Console.WriteLine(image.ToString());
        Console.WriteLine(image.NumLightPixels());

        // enhance
        for (int i = 0; i < 50; i++)
        {
            image.Enlarge();
            //Console.WriteLine(image.ToString()); 
            image.Enlarge();
            //Console.WriteLine(image.ToString());
            image.Enhance(enh);
            Console.WriteLine(image.ToString());
            Console.WriteLine(image.NumLightPixels());
            image.Shorten();
            Console.WriteLine(image.ToString());
            Console.WriteLine(image.NumLightPixels());
            //image.Enlarge();
        }
    }
}



