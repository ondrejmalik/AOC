namespace AdventOfCode.Year2015;

public class Day17Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 17;


    public static IEnumerable<List<int>> Combinations(int[] input, int targetLiters)
    {
        foreach (var VARIABLE in Recursive(0, 0, targetLiters, input, new List<int>()))
        {
            yield return VARIABLE;
        }
    }

    public static IEnumerable<List<int>> Recursive(int currentSize, int startIndex, int targetLiters, int[] input,
        List<int> indexes)
    {
        if (currentSize == targetLiters)
        {
            yield return indexes;
            yield break;
        }

        if (startIndex >= input.Length || currentSize > targetLiters)
            yield break;

        for (int i = startIndex; i < input.Length; i++)
        {
            List<int> newIndexes = [.. indexes];
            newIndexes.Add(i);
            foreach (var VARIABLE in Recursive(currentSize + input[i], i + 1, targetLiters, input, newIndexes))
            {
                yield return VARIABLE;
            }
        }
    }

    public override string SolvePart1(string[] input)
    {
        List<int> containerSizes = new List<int>(input.Length);

        for (int i = 0; i < input.Length; i++)
            containerSizes.Add(Int32.Parse(input[i]));
        var a = Combinations([.. containerSizes], 150).ToList().Count;

        return a.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        List<int> containerSizes = new List<int>(input.Length);

        for (int i = 0; i < input.Length; i++)
            containerSizes.Add(Int32.Parse(input[i]));
        var valid = Combinations([.. containerSizes], 150).ToList();
        var minCount = valid.Min(combination => combination.Count);
        //!!! Tady byl bug v mem puvodnim reseni protoze var a = GenerateRecursive(containerSizes.ToArray()).ToList().MinBy(x => x.Count).Count; vrati count minimalni kombinace ne kolik tech kombinaci co maji target 150 bylo vyslo to jen nahodou
        //return valid.Count(combination => combination.Count == minCount).ToString();
        return minCount.ToString();
    }
}
// tady jsem se snazil napsat sam IEnumerable funkce moc mi to neslo ale nakonec jsem se zeptal gemini
// hlavni problem byl v tom ze jsem passoval remaining jako v Lib.GenerateCompositions misto toho jsem mel passovat startIndex s cim se pracuje mnohem lepe
// potom jsem taky prohodil poradi base case kde jsem fail mel nad success kde Size = 150 takze by se mohlo stat ze me to failne pri max stale povolene delce
// napada me ze part1 by mohla vracet jenom count a byla by rychlejsi ale zaje je to takhle modularnejsi pro part2
/*IDIOMATIC
 misto hardcoded 150 jsem to zmenil na argument targetLiters a opravil bug v part 2
*/