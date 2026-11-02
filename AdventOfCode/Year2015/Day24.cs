using System.Diagnostics;

namespace AdventOfCode.Year2015;

public class Day24 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 24;

    IEnumerable<List<int>> GenerateCombinationsWithSumOfN(List<int> ints, int n)
    {
        for (int startIndex = 0; startIndex < ints.Count; startIndex++)
        {
            foreach (var combination in CombinationsWithSumOfN(
                         ints,
                         [ints[startIndex]],
                         n,
                         startIndex,
                         ints[startIndex]))
            {
                yield return combination;
            }
        }
    }

    List<List<int>> GenerateCombinationsWithSumOfNStandard(List<int> ints, int n)
    {
        var combinations = new List<List<int>>();

        Parallel.For(
            0,
            ints.Count,
            () => new List<List<int>>(),
            (startIndex, _, localCombinations) =>
            {
                localCombinations.AddRange(
                    CombinationsWithSumOfNStandard(
                        ints,
                        [ints[startIndex]],
                        n,
                        startIndex,
                        ints[startIndex]));
                return localCombinations;
            },
            localCombinations =>
            {
                lock (combinations)
                {
                    combinations.AddRange(localCombinations);
                }
            });

        return combinations;
    }

    IEnumerable<List<int>> CombinationsWithSumOfN(
        List<int> ints,
        List<int> combination,
        int target,
        int index,
        int sum)
    {
        if (sum > target)
            yield break;

        if (sum == target)
        {
            yield return combination;
            yield break;
        }


        for (int i = index + 1; i < ints.Count; i++)
        {
            List<int> newCombination =
            [
                .. combination,
                ints[i]
            ];

            bool yieldedAnything = false;
            foreach (var nestedCombination in CombinationsWithSumOfN(
                         ints,
                         newCombination,
                         target,
                         i,
                         sum + ints[i]))
            {
                yieldedAnything = true;
                yield return nestedCombination;
            }

            if (!yieldedAnything)
                break;
        }

        if (index + 1 == ints.Count)
            yield break;
        foreach (var nestedCombination in CombinationsWithSumOfN(
                     ints,
                     combination,
                     target,
                     index + 1,
                     sum))
        {
            yield return nestedCombination;
        }
    }

    List<List<int>> CombinationsWithSumOfNStandard(
        List<int> ints,
        List<int> combination,
        int target,
        int index,
        int sum)
    {
        var combinations = new List<List<int>>();

        if (sum > target)
            return combinations;

        if (sum == target)
        {
            combinations.Add(combination);
            return combinations;
        }


        for (int i = index + 1; i < ints.Count; i++)
        {
            List<int> newCombination =
            [
                .. combination,
                ints[i]
            ];

            var nestedCombinations = CombinationsWithSumOfNStandard(
                ints,
                newCombination,
                target,
                i,
                sum + ints[i]);

            if (nestedCombinations.Count == 0)
                break;

            combinations.AddRange(nestedCombinations);
        }

        if (index + 1 == ints.Count)
            return combinations;
        combinations.AddRange(
            CombinationsWithSumOfNStandard(
                ints,
                combination,
                target,
                index + 1,
                sum));

        return combinations;
    }

    public override string SolvePart1(string[] input)
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        List<int> weights = input.Select(int.Parse).ToList();
        int weightSum = weights.Sum();
        int groupWeight = weightSum / 3;
        var combinationsWithSumOfN = GenerateCombinationsWithSumOfN(weights, groupWeight).ToList();

        var orderedInts = combinationsWithSumOfN.OrderBy(ints => ints.Count).ToList();

        int minCount = orderedInts[0].Count;

        var enumerable = orderedInts.Where(ints => ints.Count == minCount);

        //get quantum entanglement
        List<int> theBestList = null!;
        long minQe = long.MaxValue;

        foreach (var group in enumerable)
        {
            long qa = 1;
            foreach (var weight in group)
                qa *= weight;

            if (qa > minQe) continue;
            minQe = qa;
            theBestList = group;
        }

        foreach (var weight in theBestList)
        {
            Console.WriteLine(weight);
        }

        Console.WriteLine(s.Elapsed);

        return minQe.ToString();

        return "";
    }

    public override string SolvePart2(string[] input)
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        List<int> weights = input.Select(int.Parse).ToList();
        int weightSum = weights.Sum();
        //just changed to 4 groups
        int groupWeight = weightSum / 4;
        var combinationsWithSumOfN = GenerateCombinationsWithSumOfN(weights, groupWeight).ToList();

        var orderedInts = combinationsWithSumOfN.OrderBy(ints => ints.Count).ToList();

        int minCount = orderedInts[0].Count;

        var enumerable = orderedInts.Where(ints => ints.Count == minCount);

        //get quantum entanglement
        List<int> theBestList = null!;
        long minQe = long.MaxValue;

        foreach (var group in enumerable)
        {
            long qe = 1;
            foreach (var weight in group)
                qe *= weight;
            if (qe > minQe) continue;
            minQe = qe;
            theBestList = group;
        }

        foreach (var weight in theBestList)
        {
            Console.WriteLine(weight);
        }

        Console.WriteLine(s.Elapsed);

        return minQe.ToString();
    }
}
// tohle neni optimalni ale tak funguje to ale potrebuje to 5gb ram xD a 5 minut runtime 99% casu se stravi v alokacich a List ctor
// jak se toho listu zbavit nevim
// uvazoval jsem takhle vim ze musim ziskat takovou hodnotu aby byla soucet vah/poctem skupin to byl muj startovni bod.
// nejdriv jsem to chtel delat permutacema a ziskat prvnich x elementu a scitat dokud nebudou cilova hodnota nebo kdyz vetsi tak je zahodim ale to jsem opustil po tom co jsem zjistil ze v puzzle inputu je 29 vah takze factorial 29
// potom me napadlo ze by vlastne stacilo N for loopů ale nevedel jsem jak na to N pak jsem se podival do Lib.a tam me napadlo ze mam pouzit rekurzi a posouvaci index v argumentu tak jsem udelal recurse metodu a generate recurse coz je wrapper ktery ma argumenty jen ty ktere nejsou nejaky default nebo 0 pro jednodussi pouziti prijde mi to jako dobry pattern
// rozhodl jsem se pro enumerable ale mohlo to byt klidne List<int> a normalni return misto yield return. Spis jsem si chtel procvicit ten return pattern a uvazovat v te pull mentalite
// dost me trapila ta logika for loopu dole. passoval jsem totiz do argumentu rekurzni metody index + i ale mel jsem passnout jen i protoze to uz je index + i
// pak jsem taky zapasil s tim kde vkladat do listu newCombination ten novy prvek. davalo mi smysl ho davat v kazde iteraci toho loopu ale to mi rozbilo prvni volani z GenerateMetody. vyresil jsem to az kdyz jsem musel udelat verzi ktera nic nevlozi do newConmbination a jen incrementuje index. stacilo proste vlozit ten nulty element uz pri volani z Generate
// gemini mi pak poradil kdyz jsem se ho zeptal jak udelat optimalizaci tak aby preslal loopovat kdyz yield nic nevrati pres             bool yieldedAnything = false; ktery pak setne na true kdyz se dostane do foreach loopu a checkne po dobehnuti loopu kdyz je false tak ho breakne
// zbytek jsem udelal tak ze jsem seradil validni kombinace podle poctu prvku ziskal minimum a vybral ty ktere maji to minimum z tech jsem spocetl quantum entanglement nejdriv pres weights.Aggregate() coz ma byt to co je v rustu fold ale to byla chyba protoze Aggregate me nuti pouzivat T jako acc ktery je T stejny jako List<T> coz je v tuhle chvili int a ten ma maly range (vysledek vysel vetsi)
// takze jsem musel pouzit foreach a long vytvorit manualne mak jsem pres if checknul u kazde kombinace jestli qe je mensi nez minQe a ulozil ho a zaroven ulozil ten list jako best list ((tady jsem si byl naprosto jistej ze best list prejmenuju samozrejme jsem to neudelal xd))
// Math.Min jsem pouzit nemohl protoze jsem delal 2 operace ulozil list a hodnotu do minQe -- takze mozna to dela vic branching assembly??? nevim musel bych se podivat a jako tresnicku na dortu mi warning suggestion poradil ze ten if muzu flipnout na early return/continue takze never nesting pattern a to je fajn
// k tomuhle bych se chtel vratit a internalizovat si reseni bez 5gb alokaci
