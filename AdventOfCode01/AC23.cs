

class GameStateEqualityComparer : IEqualityComparer<GameState>
{
    public bool Equals(GameState x, GameState y)
    {
        return x == y;
    }

    public int GetHashCode(GameState obj)
    {
        return obj.GetHashCode();
    }
}

class GameState
{
    int[] hallway;
    List<Stack<int>> corridors;

    public GameState Clone()
    {
        return new GameState(this);
    }

    public GameState(GameState gs)
    {
        hallway = new int[7];
        corridors = new List<Stack<int>>();
        corridors.Add(new Stack<int>(gs.corridors[0].Reverse()));
        corridors.Add(new Stack<int>(gs.corridors[1].Reverse()));
        corridors.Add(new Stack<int>(gs.corridors[2].Reverse()));
        corridors.Add(new Stack<int>(gs.corridors[3].Reverse()));
        gs.hallway.CopyTo(hallway, 0);
    }

    public GameState()
    {
        hallway = new int[7];
        corridors = new List<Stack<int>>();
        corridors.Add(new Stack<int>());
        corridors.Add(new Stack<int>());
        corridors.Add(new Stack<int>());
        corridors.Add(new Stack<int>());
        corridors[0].Push(2);
        corridors[0].Push(4);
        corridors[0].Push(4);
        corridors[0].Push(4);

        corridors[1].Push(1);
        corridors[1].Push(2);
        corridors[1].Push(3);
        corridors[1].Push(1);

        corridors[2].Push(4);
        corridors[2].Push(1);
        corridors[2].Push(2);
        corridors[2].Push(2);

        corridors[3].Push(3);
        corridors[3].Push(3);
        corridors[3].Push(1);
        corridors[3].Push(3);
    }

    public int GetLast(int i)
    {
        if (corridors[i - 1].Count == 0) return 0;
        return corridors[i - 1].Reverse().First();
    }

    public Dictionary<GameState, long> FromHallway(long energy)
    {
        Dictionary<GameState, long> res = new Dictionary<GameState, long>();

        for (int h = 0; h < hallway.Length; h++)
        {
            if (hallway[h] > 0)
            {
                int last = GetLast(hallway[h]);
                // check if room is empty:
                if (last == 0 || last == hallway[h])
                {
                    // check if hallway is passable
                    bool passable = true;
                    List<int> needPassable = GetPassable(h, hallway[h] - 1);
                    foreach (int hall in needPassable)
                    {
                        if (hallway[hall] != 0)
                        {
                            passable = false;
                            break;
                        }
                    }

                    // if is passable, create the state
                    if (passable)
                    {
                        GameState ngs = new GameState(this);
                        ngs.hallway[h] = 0;
                        ngs.corridors[hallway[h] - 1].Push(hallway[h]);

                        // calculate energy as num steps * energy_req
                        int numSteps = 2 * needPassable.Count + 5 - corridors[hallway[h]-1].Count;
                        if (h == 0 || h == 6) numSteps--;
                        long newEnergy = numSteps * (long)Math.Pow(10, hallway[h] - 1);

                        res[ngs] = energy + newEnergy;
                    }
                }
            }
        }
        //Console.Write("H");
        //Console.WriteLine(res.Count);
        return res;
    }

    public Dictionary<GameState, long> FromRoom(long energy)
    {
        Dictionary<GameState, long> res = new Dictionary<GameState, long>();

        for (int r = 0; r < corridors.Count; r++)
        {
            if (corridors[r].Count > 0)
            {
                int top = corridors[r].Peek();
                for (int h = 0; h < hallway.Length; h++)
                {
                    // check if hallway is empty
                    if (hallway[h] != 0)
                    {
                        continue;
                    }
                    // check if hallway is passable
                    bool passable = true;
                    List<int> needPassable = GetPassable(h, r);
                    foreach (int hall in needPassable)
                    {
                        if (hallway[hall] != 0)
                        {
                            passable = false;
                            break;
                        }
                    }

                    // if is passable, create the state
                    if (passable)
                    {
                        GameState ngs = new GameState(this);
                        ngs.hallway[h] = top;
                        ngs.corridors[r].Pop();

                        // calculate energy as num steps * energy_req
                        int numSteps = 2 * needPassable.Count + 6 - corridors[r].Count;
                        if (h == 0 || h == 6) numSteps--;
                        long newEnergy = numSteps * (long)Math.Pow(10, top - 1);

                        res[ngs] = energy + newEnergy;
                    }
                }
            }
        }
        //Console.Write("R");
        //Console.WriteLine(res.Count);
        return res;
    }

    public List<int> GetPassable(int hallway, int room)
    {
        if (hallway - 1 <= room) return Enumerable.Range(hallway + 1, room - hallway + 1).ToList();
        else return Enumerable.Range(room + 2, hallway - room - 2).ToList();
    }

    public bool IsFinal()
    {
        for (int i = 0; i < 4; i++)
        {
            var arr = corridors[i].ToArray();
            if (arr.Length != 4 || arr.Sum() != 4 * (i+1)) return false;
        }
        return true;
    }

    public void Print(long energy)
    {
        Console.Write("############# ");
        Console.WriteLine(energy);
        Console.Write("#");
        Console.Write(hallway[0]);
        Console.Write(hallway[1]);
        Console.Write(".");
        Console.Write(hallway[2]);
        Console.Write(".");
        Console.Write(hallway[3]);
        Console.Write(".");
        Console.Write(hallway[4]);
        Console.Write(".");
        Console.Write(hallway[5]);
        Console.Write(hallway[6]);
        Console.WriteLine("#");
        Console.Write("###");
        Console.Write(corridors[0].Count);
        Console.Write("#");
        Console.Write(corridors[1].Count);
        Console.Write("#");
        Console.Write(corridors[2].Count);
        Console.Write("#");
        Console.Write(corridors[3].Count);
        Console.WriteLine("###");
    }
    public override bool Equals(object obj)
    {
        GameState? gs = obj as GameState;
        if (gs == null) return false;
        for (int i = 0; i < 7; i++)
        {
            if (this.hallway[i] != gs.hallway[i]) return false;
        }
        for (int i = 0; i < 4; i++)
        {
            var a = this.corridors[i].ToArray();
            var b = gs.corridors[i].ToArray();
            if (a.Length != b.Length) return false;
            for (int j = 0; j < a.Length; j++)
            {
                if (a[j] != b[j]) return false;
            }
        }
        return true;
    }

    public static bool operator !=(GameState l, GameState r)
    {
        return !(l == r);
    }

    public static bool operator ==(GameState l, GameState r)
    {
        if (ReferenceEquals(l, r)) return true;
        if (ReferenceEquals(r, null)) return false;
        if (ReferenceEquals(l, null)) return false;
        return l.Equals(r);
    }

    public override int GetHashCode()
    {
        int hash = 7;
        for (int i = 0; i < 7; i++) hash = 5 * hash + hallway[i];
        return hash;
    }
}

internal class AC23
{

    public static void Run2(string[] args)
    {
        // read file
        //string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode2021\AdventOfCode01\input22_s.txt");

        // initial setup
        Dictionary<GameState, long> stateD = new Dictionary<GameState, long>(new GameStateEqualityComparer());
        PriorityQueue<GameState, long> stateQ = new PriorityQueue<GameState, long>();
        stateQ.Enqueue(new GameState(), 0);
        stateD[new GameState()] = 0;

        // loop through states
        while (true)
        {
            if (!stateQ.TryDequeue(out GameState gs, out long energy)) break;
            //gs.Print(energy);
            if (!gs.IsFinal())
            {
                int rc = 0;
                int rcadded = 0;
                foreach (KeyValuePair<GameState, long> keyVal in gs.FromRoom(energy))
                {
                    rc++;
                    if (!stateD.ContainsKey(keyVal.Key) || stateD[keyVal.Key] > keyVal.Value)
                    {
                        stateD[keyVal.Key] = keyVal.Value;
                        stateQ.Enqueue(keyVal.Key, keyVal.Value);
                        rcadded++;
                    }

                }
                //Console.WriteLine(rc);
                //Console.WriteLine(rcadded);

                foreach (KeyValuePair<GameState, long> keyVal in gs.FromHallway(energy))
                {
                    if (!stateD.ContainsKey(keyVal.Key) || stateD[keyVal.Key] > keyVal.Value)
                    {
                        stateD[keyVal.Key] = keyVal.Value;
                        stateQ.Enqueue(keyVal.Key, keyVal.Value);
                    }
                }
            }
            else
            {
                Console.WriteLine(energy);
            }
            //Console.WriteLine(stateQ.Count);
        }
    }
}



