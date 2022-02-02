public class Packet
{
    public int version;
    public int id;
    public string remainder;
    public long literal;
    public bool lengthType;
    readonly List<Packet> packets = new List<Packet>();

    public Packet(string binary)
    {
        this.version = Convert.ToInt32(binary[0..3], 2);
        this.id = Convert.ToInt32(binary[3..6], 2);

        if (id == 4)
        {
            int i = 1;
            long num = 0;
            do
            {
                i += 5;
                num *= 16;
                num += Convert.ToInt32(binary[(i + 1)..(i + 5)], 2);
            } while (binary[i] == '1');
            this.literal = num;

            this.remainder = binary[(i + 5)..];
        }
        else
        {
            this.lengthType = binary[6] == '1';

            if (!lengthType)
            {
                int bitsSubpackets = Convert.ToInt32(binary[7..22], 2);
                var rem = binary[22..(22 + bitsSubpackets)];
                while (rem.Length > 0)
                {
                    packets.Add(new Packet(rem));
                    rem = packets[^1].remainder;
                }
                this.remainder = binary[(22 + bitsSubpackets)..];
            }
            else
            {
                int numSubpackets = Convert.ToInt32(binary[7..18], 2);
                var rem = binary[18..];
                for (int i = 0; i < numSubpackets; i++)
                {
                    packets.Add(new Packet(rem));
                    rem = packets[^1].remainder;
                }
                this.remainder = rem;
            }
        }
    }

    public int VersionSum()
    {
        return version + (from packet in packets select packet.VersionSum()).Sum();
    }

    public long Compute()
    {
        if (id == 4) return literal;
        if (id == 0) return packets.Aggregate(0L, (sum, pckt) => sum + pckt.Compute());
        if (id == 1) return packets.Aggregate(1L, (prod, pckt) => prod * pckt.Compute());
        if (id == 2) return packets.Aggregate(Int64.MaxValue, (min, pckt) => Math.Min(min, pckt.Compute()));
        if (id == 3) return packets.Aggregate(0L, (max, pckt) => Math.Max(max, pckt.Compute()));
        if (id == 5) return packets[0].Compute() > packets[1].Compute() ? 1L : 0L;
        if (id == 6) return packets[0].Compute() < packets[1].Compute() ? 1L : 0L;
        if (id == 7) return packets[0].Compute() == packets[1].Compute() ? 1L : 0L;
        return 0;
    }

    public void Print()
    {
        if (id == 4) Console.Write(literal);
        if (id != 4)
        {
            Console.Write("(");
            foreach (var packet in packets)
            {
                packet.Print();
                if (id == 0) Console.Write("+");
                if (id == 1) Console.Write("*");
                if (id == 2) Console.Write("MAX");
                if (id == 3) Console.Write("MIN");
                if (id == 5) Console.Write(">");
                if (id == 6) Console.Write("<");
                if (id == 7) Console.Write("==");
            }
            Console.Write(")");
        }
    }

}

internal class AC16
{
    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input16.txt");
        //lines[0] = "9C0141080250320F1802104A08";

        // parse line
        string binarystring = String.Join(String.Empty, lines[0].Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

        // parse packet
        Packet packet = new Packet(binarystring);

        // find version sum
        Console.WriteLine(packet.VersionSum());

        // find computation
        Console.WriteLine(packet.Compute());

        // display them
        packet.Print();
    }
}
