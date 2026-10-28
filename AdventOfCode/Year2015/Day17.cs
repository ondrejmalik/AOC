namespace AdventOfCode.Year2015;

public class Day17 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 17;


    public static IEnumerable<List<int>> GenerateRecursive(int[] input)
    {
        foreach (var VARIABLE in Recursive(0, 0, input, new List<int>()))
        {
            yield return VARIABLE;
        }
    }

    public static IEnumerable<List<int>> Recursive(int currentSize, int startIndex, int[] input, List<int> indexes)
    {
        if (currentSize == 150)
        {
            yield return indexes;
            yield break;
        }

        if (startIndex >= input.Length || currentSize > 150)
            yield break;

        for (int i = startIndex; i < input.Length; i++)
        {
            List<int> newIndexes = indexes.ToList();
            newIndexes.Add(i);
            foreach (var VARIABLE in Recursive(currentSize + input[i], i + 1, input, newIndexes))
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
        var a = GenerateRecursive(containerSizes.ToArray()).ToList().Count;

        return a.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        List<int> containerSizes = new List<int>(input.Length);

        for (int i = 0; i < input.Length; i++)
            containerSizes.Add(Int32.Parse(input[i]));
        var a = GenerateRecursive(containerSizes.ToArray()).ToList().MinBy(x => x.Count).Count;
        return a.ToString();
    }
}
// tady jsem se snazil napsat sam IEnumerable funkce moc mi to neslo ale nakonec jsem se zeptal gemini
// hlavni problem byl v tom ze jsem passoval remaining jako v Lib.GenerateCompositions misto toho jsem mel passovat startIndex s cim se pracuje mnohem lepe
// potom jsem taky prohodil poradi base case kde jsem fail mel nad success kde Size = 150 takze by se mohlo stat ze me to failne pri max stale povolene delce
// napada me ze part1 by mohla vracet jenom count a byla by rychlejsi ale zaje je to takhle modularnejsi pro part2
