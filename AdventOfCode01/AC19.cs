public class Coordinate
{
    public int x;
    public int y;
    public int z;

    public Coordinate(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static (int, int, int) TurnX(int x, int y, int z, bool inv = false)
    {
        if (inv) return (x, -z, y);
        else return (x, z, -y);
    }

    public static (int, int, int) TurnY(int x, int y, int z, bool inv = false)
    {
        if (inv) return (-z, y, x);
        else return (z, y, -x);
    }
    public static (int, int, int) TurnZ(int x, int y, int z, bool inv = false)
    {
        if (inv) return (-y, x, z);
        else return (y, -x, z);
    }

    public Coordinate Transform(int transformNum)
    {
        int tx = transformNum % 4;
        int tyz = transformNum / 4;

        (int, int, int) coord = (x, y, z);

        for (int i = 0; i < tyz; i++) coord = i % 2 == 1 ? TurnZ(coord.Item1, coord.Item2, coord.Item3) : TurnY(coord.Item1, coord.Item2, coord.Item3);
        for (int i = 0; i < tx; i++) coord = TurnX(coord.Item1, coord.Item2, coord.Item3);

        //Console.Write(Convert.ToString(tx) + " " +Convert.ToString(ty) + " " + Convert.ToString(tz) + " ");

        return new Coordinate(coord.Item1, coord.Item2, coord.Item3);
    }

    public Coordinate TransformInv(int transformNum)
    {
        int tx = transformNum % 4;
        int tyz = transformNum / 4;

        (int, int, int) coord = (x, y, z);

        for (int i = 0; i < tx; i++) coord = TurnX(coord.Item1, coord.Item2, coord.Item3, true);
        for (int i = tyz - 1; i >= 0; i--) coord = i % 2 == 1 ? TurnZ(coord.Item1, coord.Item2, coord.Item3, true) : TurnY(coord.Item1, coord.Item2, coord.Item3, true);

        return new Coordinate(coord.Item1, coord.Item2, coord.Item3);
    }

    public override string ToString()
    {
        return Convert.ToString(x) + ',' + Convert.ToString(y) + ',' + Convert.ToString(z);
    }

    public override bool Equals(object? obj)
    {
        Coordinate? coord = obj as Coordinate;
        if (coord == null) return false;
        else return this.x == coord.x && this.y == coord.y && this.z == coord.z;
    }

    public override int GetHashCode()
    {
        return x + 1000 * y + 1000000 * z;
    }

    public static Coordinate operator -(Coordinate l)
    {
        return new Coordinate(-l.x, -l.y, -l.z);
    }

    public static Coordinate operator +(Coordinate l, Coordinate r)
    {
        return new Coordinate(l.x + r.x, l.y + r.y, l.z + r.z);
    }

    public int ManhattanDist(Coordinate c)
    {
        return Math.Abs(c.x - x) + Math.Abs(c.y - y) + Math.Abs(c.z - z);
    }
}

public class CoordinateDiff : Coordinate
{
    public int idxLeft;
    public int idxRight;

    public CoordinateDiff(Coordinate lhs, Coordinate rhs, int idxLeft, int idxRight) : base(0, 0, 0)
    {
        this.x = lhs.x - rhs.x;
        this.y = lhs.y - rhs.y;
        this.z = lhs.z - rhs.z;
        this.idxLeft = idxLeft;
        this.idxRight = idxRight;
    }

    public CoordinateDiff(int x, int y, int z, int idxLeft, int idxRight) : base(x, y, z)
    {
        this.idxLeft = idxLeft;
        this.idxRight = idxRight;
    }

    public new CoordinateDiff Transform(int transformNum)
    {
        Coordinate temp = base.Transform(transformNum);
        return new CoordinateDiff(temp.x, temp.y, temp.z, idxLeft, idxRight);
    }

    public new CoordinateDiff TransformInv(int transformNum)
    {
        Coordinate temp = base.TransformInv(transformNum);
        return new CoordinateDiff(temp.x, temp.y, temp.z, idxLeft, idxRight);
    }

    public override bool Equals(object? obj)
    {
        CoordinateDiff? coord = obj as CoordinateDiff;
        if (coord == null) return false;
        else return this.x == coord.x && this.y == coord.y && this.z == coord.z;
    }

    public static bool operator ==(CoordinateDiff? l, CoordinateDiff? r)
    {
        if (ReferenceEquals(l, r)) return true;
        if (r is null) return false;
        if (l is null) return false;
        return l.Equals(r);
    }

    public static CoordinateDiff operator -(CoordinateDiff l)
    {
        return new CoordinateDiff(-l.x, -l.y, -l.z, l.idxLeft, l.idxRight);
    }

    public static bool operator !=(CoordinateDiff? l, CoordinateDiff? r)
    {
        if (ReferenceEquals(l, r)) return true;
        if (r is null) return false;
        if (l is null) return false;
        return !l.Equals(r);
    }

    public override int GetHashCode()
    {
        return x + 1000 * y + 1000000 * z;
    }

}

public class Scanner
{
    public List<Coordinate> beacons;
    HashSet<CoordinateDiff> beaconDiffs;

    public Scanner()
    {
        beacons = new List<Coordinate>();
        beaconDiffs = new HashSet<CoordinateDiff>();
    }

    public void AddBeacon(Coordinate c)
    {
        for (int i = 0; i < beacons.Count; i++)
        {
            beaconDiffs.Add(new CoordinateDiff(beacons[i], c, i, beacons.Count));
        }
        beacons.Add(c);
    }

    public (int, int, int, bool)? CommonBeacons(Scanner scanner, int minFound = 11)
    {
        int lastJ = 0, lastLeft = 0, lastRight = 0;
        for (int i = 0; i < beacons.Count; i++)
        {
            for (int t = 0; t < 24; t++)
            {
                //var deb = beacons[i].Transform(t);
                int foundBeacons = 0;
                Dictionary<int, int> counts = new Dictionary<int, int>();

                for (int j = 0; j < beacons.Count; j++)
                {
                    if (i == j) continue;
                    var diff = new CoordinateDiff(beacons[i], beacons[j], i, j).Transform(t);
                    if (scanner.beaconDiffs.TryGetValue(diff, out CoordinateDiff? ret))
                    {
                        foundBeacons++;
                        if (!counts.ContainsKey(ret.idxLeft)) counts[ret.idxLeft] = 0;
                        if (!counts.ContainsKey(ret.idxRight)) counts[ret.idxRight] = 0;
                        counts[ret.idxLeft]++;
                        counts[ret.idxRight]++;
                        lastLeft = ret.idxLeft;
                        lastRight = ret.idxRight;
                        lastJ = j;
                        if (foundBeacons >= minFound)
                        {
                            int idx2 = counts.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

                            // now find the orientation
                            var a = new CoordinateDiff(scanner.beacons[lastLeft], scanner.beacons[lastRight], 0, 0);
                            var b = new CoordinateDiff(beacons[i].Transform(t), beacons[lastJ].Transform(t), 0, 0);
                            if (a == b)
                                return (i, idx2, t, true);
                            else return (i, idx2, t, false);
                        }
                    }
                }
            }
        }
        return null;
    }
}

internal class AC19
{
    static public List<Coordinate> ApplyTransform(List<Coordinate> beacons, CoordinateDiff diff, int transformationNum)
    {
        List<Coordinate> result = new List<Coordinate>();
        for (int i = 0; i < beacons.Count; i++)
        {
            Coordinate transformed = beacons[i].TransformInv(transformationNum);
            result.Add(new CoordinateDiff(transformed, -diff, 0, 0));
        }
        return result;
    }

    public static void Run1()
    {
        // read file
        string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\inputs\input19.txt");

        // parse file
        List<Scanner> scanners = new List<Scanner>();
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith("---"))
            {
                scanners.Add(new Scanner());
            }
            else
            {
                var split = line.Trim().Split(',');
                if (split.Length == 3)
                {
                    scanners[^1].AddBeacon(new Coordinate(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2])));
                }
            }
        }

        Dictionary<int, (CoordinateDiff, int, int)> connectedScanners = new Dictionary<int, (CoordinateDiff, int, int)>();
        Dictionary<int, List<Coordinate>> beaconPos = new Dictionary<int, List<Coordinate>>();
        Dictionary<int, Coordinate> scannerPos = new Dictionary<int, Coordinate>();
        connectedScanners[0] = (new CoordinateDiff(0, 0, 0, 0, 0), 0, 0);
        beaconPos[0] = scanners[0].beacons;
        scannerPos[0] = new Coordinate(0, 0, 0);

        Stack<int> toConnect = new Stack<int>();
        toConnect.Push(0);

        while (connectedScanners.Count != scanners.Count)
        {
            int tryConnect = toConnect.Pop();
            for (int i = 0; i < scanners.Count; i++)
            {
                if (connectedScanners.ContainsKey(i)) continue;
                (int, int, int, bool)? common = scanners[tryConnect].CommonBeacons(scanners[i]);
                if (common != null)
                {
                    CoordinateDiff cd;
                    if (common.Value.Item4)
                    {
                        cd = new CoordinateDiff(scanners[tryConnect].beacons[common.Value.Item1], scanners[i].beacons[common.Value.Item2].TransformInv(common.Value.Item3), 0, 0);
                    }
                    else
                    {
                        cd = new CoordinateDiff(scanners[tryConnect].beacons[common.Value.Item1], -scanners[i].beacons[common.Value.Item2].TransformInv(common.Value.Item3), 0, 0);
                    }
                    connectedScanners[i] = (cd, tryConnect, common.Value.Item3);
                    toConnect.Push(i);

                    beaconPos[i] = ApplyTransform(scanners[i].beacons, connectedScanners[i].Item1, connectedScanners[i].Item3);
                    scannerPos[i] = connectedScanners[i].Item1;
                    int convertTo = tryConnect;
                    while (convertTo != 0)
                    {
                        beaconPos[i] = ApplyTransform(beaconPos[i], connectedScanners[convertTo].Item1, connectedScanners[convertTo].Item3);
                        scannerPos[i] = connectedScanners[convertTo].Item1 + scannerPos[i].TransformInv(connectedScanners[convertTo].Item3);
                        convertTo = connectedScanners[convertTo].Item2;
                    }
                }
            }
        }

        HashSet<Coordinate> coordinates = new HashSet<Coordinate>();
        for (int i = 0; i < beaconPos.Count; i++)
        {
            for (int j = 0; j < beaconPos[i].Count; j++)
            {
                coordinates.Add(beaconPos[i][j]);
            }
            Console.WriteLine(coordinates.Count);
        }
        Console.WriteLine(coordinates.Count);

        int maxDist = 0;
        for (int i = 0; i < scannerPos.Count; i++)
        {
            for (int j = i + 1; j < scannerPos.Count; j++)
            {
                var dist = scannerPos[i].ManhattanDist(scannerPos[j]);
                if (dist > maxDist) maxDist = dist;
            }
        }

        Console.WriteLine(maxDist);
    }

}
