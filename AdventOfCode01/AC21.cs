
public class Dice
{
    int current = 1;
    public int rolls = 0;
    public int GetNumber()
    {
        int ret = current;
        current++;
        rolls++;
        if (current > 100)
        {
            current -= 100;
        }
        return ret;
    }
}


public class Player
{
    public int score = 0;
    public int position;

    public Player(int position)
    {
        this.position = position;
    }

    public bool Turn(Dice dc)
    {
        position = (position + dc.GetNumber() + dc.GetNumber() + dc.GetNumber() - 1) % 10 + 1;
        score += position;
        return score >= 1000;
    }
}


public class State
{
    public const int MaxScore = 21;
    int position1;
    int position2;
    public int score1;
    public int score2;

    public State(int position1, int position2, int score1, int score2)
    {
        this.position1 = position1;
        this.position2 = position2;
        this.score1 = score1;
        this.score2 = score2;
    }
    public State(State state)
    {
        this.position1 = state.position1;
        this.position2 = state.position2;
        this.score1 = state.score1;
        this.score2 = state.score2;
    }

    public new string ToString()
    {
        return position1.ToString() + " " + score1.ToString() + ", " + position2.ToString() + " " + score2.ToString();
    }

    public bool Finished()
    {
        return score1 >= MaxScore || score2 >= MaxScore;
    }

    public State Clone()
    {
        return new State(this);
    }

    public bool ApplyPlayer1(int partScore)
    {
        position1 = (position1 + partScore - 1) % 10 + 1;
        score1 += position1;
        return score1 >= MaxScore;
    }

    public bool ApplyPlayer2(int partScore)
    {
        position2 = (position2 + partScore - 1) % 10 + 1;
        score2 += position2;
        return score2 >= MaxScore;
    }

}

public class Players
{
    public Dictionary<State, long> scorePos;
    public long wins1;
    public long wins2;

    public Players(int position1, int position2)
    {
        this.scorePos = new Dictionary<State, long>();
        this.scorePos[new State(position1, position2, 0, 0)] = 1;
        wins1 = 0;
        wins2 = 0;
    }

    public bool Turn()
    {
        Dictionary<int, long> outcomes = new Dictionary<int, long>() { { 3, 1L }, { 4, 3L }, { 5, 6L }, { 6, 7L }, { 7, 6L }, { 8, 3L }, { 9, 1L } };
        Dictionary<State, long> newScorePos = new Dictionary<State, long>();
        foreach (KeyValuePair<State, long> scorePosEntry in scorePos)
        {
            if (scorePosEntry.Key.Finished()) throw new Exception("");
            foreach (KeyValuePair<int, long> outcomeEntry in outcomes)
            {
                State state = scorePosEntry.Key.Clone();
                bool end = state.ApplyPlayer1(outcomeEntry.Key);

                if (end)
                {
                    wins1 += scorePosEntry.Value * outcomeEntry.Value;
                }
                else
                {
                    foreach (KeyValuePair<int, long> outcomeEntry2 in outcomes)
                    {
                        State state2 = state.Clone();
                        end = state2.ApplyPlayer2(outcomeEntry2.Key);
                        if (end) wins2 += scorePosEntry.Value * outcomeEntry.Value * outcomeEntry2.Value;
                        else
                        {
                            if (!newScorePos.ContainsKey(state2)) newScorePos[state2] = 0;
                            newScorePos[state2] += scorePosEntry.Value * outcomeEntry.Value * outcomeEntry2.Value;
                        }
                    }
                }

            }
        }
        scorePos = newScorePos;
        return scorePos.Count == 0;
    }

    public long UniverseCount()
    {
        return wins1 + wins2 + scorePos.Values.Sum();
    }
}

internal class AC21
{

    public static void Run1()
    {
        // read file
        //string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input20.txt");

        // starting positions
        int p1 = 4;
        int p2 = 8;

        p1 = 2;
        p2 = 8;

        Player player1 = new Player(p1);
        Player player2 = new Player(p2);
        Dice dc = new Dice();

        bool end = false;
        while (!end)
        {
            end = player1.Turn(dc);
            if (!end) end = player2.Turn(dc);
        }

        Console.WriteLine(Math.Min(player1.score, player2.score) * dc.rolls);
    }


    public static void Run2()
    {
        // read file
        //string[] lines = File.ReadAllLines(@"C:\Users\Vacuumlabs\source\repos\AdventOfCode01\AdventOfCode01\input20.txt");

        // starting positions
        int p1 = 4;
        int p2 = 8;

        p1 = 2;
        p2 = 8;

        Players players = new Players(p1, p2);

        bool end = false;
        while (!end)
        {
            end = players.Turn();
        }

        Console.WriteLine(Math.Max(players.wins1, players.wins2));
    }
}



