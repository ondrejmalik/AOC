namespace AdventOfCode.Year2015;

public class Day13 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 13;



    (HashSet<string>, Dictionary<(string, string), int>) Parse(string[] input)
    {
        HashSet<string> persons = new();

        Dictionary<(string, string), int> pairs = new();
        foreach (var line in input)
        {
            string[] splits = line.Split(' ');

            string name = splits[0];
            bool signPositive = splits[2] == "gain";

            int amount;
            amount = signPositive ? Int32.Parse(splits[3]) : -Int32.Parse(splits[3]);

            string otherPersonName = splits[10][..^1];
            string from = persons.TryGetValue(name, out var existingFrom)
                ? existingFrom
                //tohle napsal gemini a je to docela cool. Pouzil tuple aby si spustil .Add() ale tu hodnotu Item1 zahodi a pak accessne jenom ten druhy element
                : (persons.Add(name), name).name;

            var to = persons.TryGetValue(otherPersonName, out var existingOther)
                ? existingOther
                : (persons.Add(otherPersonName), otherPersonName).otherPersonName;

            pairs.Add((from, to), amount);
        }

        return (persons, pairs);
    }


    int FindBestScore(List<IList<string>> permutations, Dictionary<(string, string), int> pairs)
    {
        List<int> scores = new List<int>();
        foreach (var permutation in permutations)
        {
            int score = 0;

            {
                string person = permutation[0];
                string left = permutation[^1];
                string right = permutation[1];

                score += pairs[(person, left)];
                score += pairs[(person, right)];
            }
            for (int i = 1; i < permutation.Count - 1; i++)
            {
                string person = permutation[i];
                string left = permutation[i - 1];
                string right = permutation[i + 1];

                score += pairs[(person, left)];
                score += pairs[(person, right)];
            }

            {
                string person = permutation[^1];
                string left = permutation[^2];
                string right = permutation[0];

                score += pairs[(person, left)];
                score += pairs[(person, right)];
            }
            scores.Add(score);
        }

        return scores.Max();
    }

    public override string SolvePart1(string[] input)
    {
        HashSet<string> persons;
        Dictionary<(string, string), int> pairs;
        (persons, pairs) = Parse(input);

// oddeleni prvniho udelani permutaci a vraceni do kazde z permutaci nakonec takze je zafixovany
// tahle optimalizace je mozna protoze stul je kulaty a kdyby jsem to neudelal tak zbytecne pocitam i moznosti
// kdy se hoste posouvaji vsichni o jedno misto dokola dokud se nedostanou na puvodni na zasedaci poradek to ale nema vliv
        var firstPerson = persons.First();
        var restOfPersons = persons.Skip(1).ToList();
        var permutations = Lib.HeapsAlgorithm.GetPermutations(restOfPersons)
            .Select(perm =>
            {
                var fullTable = new List<string>(perm.Count + 1) { firstPerson };
                fullTable.AddRange(perm);
                return (IList<string>)fullTable;
            })
            .ToList();


        return FindBestScore(permutations, pairs).ToString();
    }


    public override string SolvePart2(string[] input)
    {
        HashSet<string> persons;
        Dictionary<(string, string), int> pairs;
        (persons, pairs) = Parse(input);
        //add myself
        string me = "Já";

        foreach (string person in persons)
        {
            pairs.Add((person, me), 0);
            pairs.Add((me, person), 0);
        }

        persons.Add(me);

        var firstPerson = persons.First();
        var restOfPersons = persons.Skip(1).ToList();
        var permutations = Lib.HeapsAlgorithm.GetPermutations(restOfPersons)
            .Select(perm =>
            {
                var fullTable = new List<string>(perm.Count + 1) { firstPerson };
                fullTable.AddRange(perm);
                return (IList<string>)fullTable;
            })
            .ToList();


        return FindBestScore(permutations, pairs).ToString();
    }
}
// Tohle bylo docela tezky nejdriv jsem chtel udelat class Person kde ma Left a Right jako jiny Person ale od toho jsem upustil
// Potom jsem udelal HashSet<string> person a List<Pair> kde Pair byl string form strin to int amount
// kdyz jsem udelal FindBestScore metodu zjistil jsem ze to takhle prohledava cely List pres FindAll pair.form == person a potom Find pair.To == left a pair.To = right takzy 3 linearni hledani na permutaci
// to jsem vyresil pres dicionary<(string,string),int> takze staci jeden lookup.
// dalsi optimalizace ta uz byla od gemini ze se zafixovala prvni osoba a permutace se delaly jen na zbytytku to znamena ze to snizilo 9x runtime.
// zaroven jsem pres gemini prisel na trik s tupple a ? ze muzu spustit kod a vratit jinou hodnotu v jednoradkove operaci