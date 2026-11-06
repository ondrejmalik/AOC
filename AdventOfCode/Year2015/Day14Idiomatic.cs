namespace AdventOfCode.Year2015;

public class Day14Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 14;


    class RaceEntry(Reindeer reindeer)
    {
        public Reindeer Reindeer { get; } = reindeer;
        public int DistanceTraveled { get; set; }
        public int Points { get; set; }

        public void Tick(int currentSecond)
        {
            int lapTotal = Reindeer.SecondsFlying + Reindeer.SecondsResting;
            if (currentSecond % lapTotal < Reindeer.SecondsFlying)
            {
                DistanceTraveled += Reindeer.Speed;
            }
        }
    }

    List<RaceEntry> SimRace(List<Reindeer> reindeers, int secondsToSim)
    {
        var entries = reindeers.Select(r => new RaceEntry(r)).ToList();

        for (int i = 0; i < secondsToSim; i++)
        {
            foreach (var entry in entries)
            {
                entry.Tick(i);
            }

            int maxDistance = entries.Max(e => e.DistanceTraveled);
            foreach (var leader in entries.Where(e => e.DistanceTraveled == maxDistance))
            {
                leader.Points++;
            }
        }

        return entries;
    }

    record Reindeer(string Name, int Speed, int SecondsFlying, int SecondsResting)
    {
        public int GetDistanceAt(int seconds)
        {
            int cycle = SecondsFlying + SecondsResting;
            int fullCycle = seconds / cycle;
            int remaining = Math.Min(SecondsFlying, seconds % cycle);
            return (fullCycle * SecondsFlying + remaining) * Speed;
        }
    }

    List<Reindeer> Parse(string[] input)
    {
        List<Reindeer> result = new();
        for (int i = 0; i < input.Length; i++)
        {
            string line = input[i];
            string[] words = line.Split(' ');
            Reindeer reindeer = new Reindeer(words[0], Int32.Parse(words[3]), Int32.Parse(words[6]),
                Int32.Parse(words[13]));
            result.Add(reindeer);
        }

        return result;
    }

    string Solve(List<Reindeer> reindeers, int secondsToSim, bool findMax = true)
    {
        int result = findMax ? int.MinValue : int.MaxValue;
        foreach (var reindeer in reindeers)
        {
            var distance = reindeer.GetDistanceAt(secondsToSim);
            result = findMax
                ? Math.Max(result, distance)
                : Math.Min(result, distance);
        }

        return result.ToString();
    }


    public override string SolvePart1(string[] input)
    {
        return Solve(Parse(input), 2503)
            ;
    }

    public override string SolvePart2(string[] input)
    {
        return SimRace(Parse(input), 2503)
            .MaxBy(reindeer => reindeer.Points)?.Points.ToString() ?? "";
        ;
    }
}
// ok tohle bylo docela ez jediny co me napada je ze by points a Distance traveled nemusely byt primo v ty classe ale takhle je to vic simple
// v druhy casti jsem chctel Pouzit SortedSet ale ten nepovoluje hodnoty se stejnou prioritou (stejnou distance traveled) a potom jsem zkousel Priority queue ale ta je nejak imutable
// nakonec jsem to udelal tak jak jsem puvodne zamyslel pres MaxBy ale puvodne mi to prislo hloupy ze je to O(n)
// kod jsem si nechal predelat pres gemini protoze jsem chtel aby nebyly v calsse primo points a distance traveled a misto toho byly ve tride nad Reindeer ktery je diky tomu imutabe a snadno se muze porovnavat
// taky mi gemini poradil ze to jde resit pres GetDistanceAt pro kazdeho reindeere coz by bylo jeste rychlejsi protoze je to cely predictable

/*IDIOMATIC
part 1 jsem podle ai mel udelat pres O(1) GetDistanceAt protoze nemam predvivat co bude v part2 podle YAGNI you aint gonna need it
takze jsem Reindeerovi pridal funkci GetDistance at ktera spocita kolik celych cyklu zvladl a prida k tomu kolik sekund odletel v tom dalsim cyklu a excludne to kdy odpociva a vynasobi rychlosi
*/