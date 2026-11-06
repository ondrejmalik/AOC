namespace AdventOfCode.Year2015;

public class Day13Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 13;
    public override string SolvePart1(string[] input) => Solve(input, addMyself: false);
    public override string SolvePart2(string[] input) => Solve(input, addMyself: true);
    private string Solve(string[] input, bool addMyself)
    {
        var (persons, pairs) = Parse(input);
        if (addMyself)
            persons.Add("Me"); // pairs.GetValueOrDefault will return 0 for "Me"!
        // Pin person 0 to eliminate circular rotation duplicates
        string first = persons.First();
        var others = persons.Skip(1).ToList();
        int maxHappiness = Lib.HeapsAlgorithm.GetPermutations(others)
            .Select(perm =>
            {
                // Full table with first person pinned at index 0
                List<string> table = [first, .. perm];
                int score = 0;
                //predtim jsem kontroloval levou a pravou u kazdyho sediciho u stolu
                //tohle funguje tak ze na kazdy edge dvou sedici priradi hodnotu souctu jak jsou spokojeni
                //napriklad Happiness(Alice,Bob)+Happiness(Bob,Alice)=54+(−7)=47 ty pak secte a vrati vysledek pro permutaci
                // zaroven me to zbavi vice duplicit jako v puvodnim kodu tohle pres modulo zvladne vztah posledniho s prvnim taky
                for (int i = 0; i < table.Count; i++)
                {
                    string a = table[i];
                    string b = table[(i + 1) % table.Count]; // circular wrap
                    score += pairs.GetValueOrDefault((a, b), 0) 
                           + pairs.GetValueOrDefault((b, a), 0);
                }
                return score;
            })
            .Max();
        return maxHappiness.ToString();
    }
    private (HashSet<string> Persons, Dictionary<(string, string), int> Pairs) Parse(string[] input)
    {
        HashSet<string> persons = [];
        Dictionary<(string, string), int> pairs = [];
        foreach (var line in input)
        {
            // "Alice would gain 54 happiness units by sitting next to Bob."
            var words = line[..^1].Split(' '); // strip trailing '.'
            string from = words[0];
            string to = words[10];
            int amount = int.Parse(words[3]) * (words[2] == "gain" ? 1 : -1);
            persons.Add(from);
            persons.Add(to);
            pairs[(from, to)] = amount;
        }
        return (persons, pairs);
    }
}

// Tohle bylo docela tezky nejdriv jsem chtel udelat class Person kde ma Left a Right jako jiny Person ale od toho jsem upustil
// Potom jsem udelal HashSet<string> person a List<Pair> kde Pair byl string form strin to int amount
// kdyz jsem udelal FindBestScore metodu zjistil jsem ze to takhle prohledava cely List pres FindAll pair.form == person a potom Find pair.To == left a pair.To = right takzy 3 linearni hledani na permutaci
// to jsem vyresil pres dicionary<(string,string),int> takze staci jeden lookup.
// dalsi optimalizace ta uz byla od gemini ze se zafixovala prvni osoba a permutace se delaly jen na zbytytku to znamena ze to snizilo 9x runtime.
// zaroven jsem pres gemini prisel na trik s tupple a ? ze muzu spustit kod a vratit jinou hodnotu v jednoradkove operaci