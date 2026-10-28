namespace AdventOfCode.Year2015;

public class Day15 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 15;

    class Ingredient
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Durability { get; set; }
        public int Flavor { get; set; }
        public int Texture { get; set; }
        public int Calories { get; set; }
    }

    List<Ingredient> PopulateIngredients(string[] lines)
    {
        List<Ingredient> ingredients = new List<Ingredient>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] splitName = line.Split(':');
            string[] splitProperties = splitName[1].Split(',');
            ingredients.Add(new()
            {
                Name = splitName[0],
                Capacity = Int32.Parse(splitProperties[0].Split(' ')[2]),
                Durability = Int32.Parse(splitProperties[1].Split(' ')[2]),
                Flavor = Int32.Parse(splitProperties[2].Split(' ')[2]),
                Texture = Int32.Parse(splitProperties[3].Split(' ')[2]),
                Calories = Int32.Parse(splitProperties[4].Split(' ')[2]),
            });
        }

        return ingredients;
    }


    long FindBestCombination(List<Ingredient> ingredients)
    {
        var possibleCompositions = Lib.IntegerPartitions.GenerateRecursive(ingredients.Count, 100);
        long total = 0;
        List<int> winningCombination = null;

        foreach (var combination in possibleCompositions)
        {
            if (combination[0] == 44 && combination[1] == 56)
            {
                Console.WriteLine("");
            }

            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            for (int i = 0; i < ingredients.Count; i++)
            {
                Ingredient ingredient = ingredients[i];
                a += combination[i] * ingredient.Capacity;
                b += combination[i] * ingredient.Durability;
                c += combination[i] * ingredient.Flavor;
                d += combination[i] * ingredient.Texture;
            }

            long sum = Math.Max(0, a) * Math.Max(0, b) * Math.Max(0, c) * Math.Max(0, d);

            if (total < sum)
            {
                total = Math.Max(total, sum);
                winningCombination = new List<int>(combination);
            }
        }

        foreach (var VARIABLE in winningCombination)
        {
            Console.WriteLine(VARIABLE);
        }

        return total;
    }

    long FindBestCombinationWithCalories(List<Ingredient> ingredients, int targetSumTeaspoons, int targetSumCalories)
    {
        var possibleCompositions = Lib.IntegerPartitions.GenerateSumCompositions(ingredients.Count, targetSumTeaspoons);
        long total = 0;
        List<int> winningCombination = null;

        foreach (var combination in possibleCompositions)
        {
            if (combination[0] == 44 && combination[1] == 56)
            {
                Console.WriteLine("");
            }

            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            int calories = 0;
            for (int i = 0; i < ingredients.Count; i++)
            {
                Ingredient ingredient = ingredients[i];
                a += combination[i] * ingredient.Capacity;
                b += combination[i] * ingredient.Durability;
                c += combination[i] * ingredient.Flavor;
                d += combination[i] * ingredient.Texture;
                calories += combination[i] * ingredient.Calories;
            }

            if (calories != targetSumCalories)
                continue;

            long sum = Math.Max(0, a) * Math.Max(0, b) * Math.Max(0, c) * Math.Max(0, d);

            if (total < sum)
            {
                total = Math.Max(total, sum);
                winningCombination = new List<int>(combination);
            }
        }

        foreach (var VARIABLE in winningCombination)
        {
            Console.WriteLine(VARIABLE);
        }

        return total;
    }

    public override string SolvePart1(string[] input)
    {
        List<Ingredient> ingredients = PopulateIngredients(input);
        long total = FindBestCombination(ingredients);

        return total.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        List<Ingredient> ingredients = PopulateIngredients(input);
        long total = FindBestCombinationWithCalories(ingredients, 100, 500);

        return total.ToString();
    }
}
// tady bylo slozity jenom hledani vsech moznosti s tim jsem si nechal pomoct od gemini.
// pak jsem se jeste zaselk na tom ze a,b,c,d musi byt pozitivni hodnoty jinak je musim vynulovat coz jsem neudelal a vychazely mi spatny vysledky.
// nakonec jsem se snazil pochopit jak funguje ta metoda sum compositions. zeptal jsem se na to gemini jak to snadno pochopit a naucit se rekurzivne volat for loop
// nejdriv jsem to udelal pres toList ale to musi alokovat cely najednou takze jsem se jeste ptal na to jak z toho udelat IEnumerable.
// to mi ai poradilo ze musim splnit 4 pravidla
// 1. pravidlo kdyz se vola return tak se misto toho pise yield break
// if (isBaseCase)
//{
//yield return item;
//yield break;
//}
// 2. pravidlo kdyloliv volam IENumerable funkci - treba tu samou rekurzivne tak musim misto volani udelat foreach pres tu funkci a v tele foreache zavolat yield return
// // Push pattern:
//Helper(nextDepth, list);
//
//// Pull pattern:
//foreach (var item in Helper(nextDepth))
//{
//    yield return item;
//}
// 3. kdyz je promena shared treba array je shared protoze je referencni tak musim udelat clone a cast
// yield return (int[])current.Clone();
// 4. Rozdeleni funkce na Check ktery pobezi hned a samotny solve ktery pobezi az kdyz nekdo zavola MoveNext() 
//public static IEnumerable<int[]> Solve(int n)
//{
//    if (n <= 0) throw new ArgumentOutOfRangeException(); // Thrown immediately
//    return Impl(n);
//}
//
//private static IEnumerable<int[]> Impl(int n)
//{
//    yield return ...;
//}