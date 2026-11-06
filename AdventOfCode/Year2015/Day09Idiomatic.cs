namespace AdventOfCode.Year2015;

public class Day09Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 9;


    private static (int[,] Dist, int Count) ParseGraph(string[] input)
    {
        Dictionary<string, int> cityIds = new(StringComparer.Ordinal);

        //podiva se do dictionary jestli najde id tak vrati id kdyz ne tak ulozi do citiIds[name] dalsi cislo ktere jeste nebylo pouzito takze pocet prvku 
        int GetOrAddId(string name) =>
            cityIds.TryGetValue(name, out int id) ? id : (cityIds[name] = cityIds.Count);

        List<(int u, int v, int dist)> edges = [];
        foreach (string line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            // Line format: "London to Dublin = 464"
            string[] parts = line.Split(" = ");
            string[] cities = parts[0].Split(" to ");

            edges.Add((GetOrAddId(cities[0]), GetOrAddId(cities[1]), int.Parse(parts[1])));
        }

        int n = cityIds.Count;
        int[,] dist = new int[n, n];
        foreach (var (u, v, d) in edges)
        {
            dist[u, v] = d;
            dist[v, u] = d; // Undirected edge
        }

        return (dist, n);
    }

    // Step 2: Held-Karp Algorithm (O(N^2 * 2^N))
    private static int SolveHeldKarp(int[,] dist, int n, bool findMax)
    {
        int allVisitedMask = (1 << n) - 1;
        int worstValue = findMax ? int.MinValue : int.MaxValue;
        // memo[u, mask] = best distance to finish the tour from city 'u' with 'mask' visited
        int[,] memo = new int[n, 1 << n];
        for (int u = 0; u < n; u++)
        for (int mask = 0; mask <= allVisitedMask; mask++)
            memo[u, mask] = -1;

        int Dfs(int u, int visitedMask)
        {
            // Base case: All cities have been visited
            if (visitedMask == allVisitedMask)
                return 0;
            // Return cached result if already computed
            if (memo[u, visitedMask] != -1)
                return memo[u, visitedMask];
            int best = worstValue;
            for (int v = 0; v < n; v++)
            {
                // Check if city 'v' is not yet visited
                if ((visitedMask & (1 << v)) == 0)
                {
                    int candidate = dist[u, v] + Dfs(v, visitedMask | (1 << v));
                    best = findMax ? Math.Max(best, candidate) : Math.Min(best, candidate);
                }
            }

            return memo[u, visitedMask] = best;
        }

        // The tour can start at any city
        int overallBest = worstValue;
        for (int startCity = 0; startCity < n; startCity++)
        {
            int tourCost = Dfs(startCity, 1 << startCity);
            overallBest = findMax ? Math.Max(overallBest, tourCost) : Math.Min(overallBest, tourCost);
        }

        return overallBest;
    }

    public override string SolvePart1(string[] input)
    {
        {
            // 1. Parse the input into a lookup dictionary of edges/distances
            var distances = new Dictionary<(string From, string To), int>();

            foreach (var line in input)
            {
                // Line format: "London to Dublin = 464"
                var parts = line.Split(" = ");
                var cities = parts[0].Split(" to ");
                string from = cities[0];
                string to = cities[1];
                int dist = int.Parse(parts[1]);

                // Roads go both ways (undirected graph)
                distances[(from, to)] = dist;
                distances[(to, from)] = dist;
            }

            // 2. Extract the list of all unique cities from the dictionary keys
            var allCities = distances.Keys
                .SelectMany(pair => new[] { pair.From, pair.To })
                .Distinct()
                .ToList();

            // 3. Generate all permutations and compute total route distance
            var routeLengths = Lib.HeapsAlgorithm.GetPermutations(allCities)
                .Select(route => route
                    .Zip(route.Skip(1), (a, b) => distances[(a, b)])
                    .Sum()
                );

            Console.WriteLine(routeLengths.Min()); // Part 1
        }
        {
            var (dist, n) = ParseGraph(input);
            return SolveHeldKarp(dist, n, findMax: false).ToString();
        }
    }

    public override string SolvePart2(string[] input)
    {
        {
            var distances = new Dictionary<(string From, string To), int>();

            foreach (var line in input)
            {
                var parts = line.Split(" = ");
                var cities = parts[0].Split(" to ");
                string from = cities[0];
                string to = cities[1];
                int dist = int.Parse(parts[1]);

                distances[(from, to)] = dist;
                distances[(to, from)] = dist;
            }

            var allCities = distances.Keys
                .SelectMany(pair => new[] { pair.From, pair.To })
                .Distinct()
                .ToList();

            var routeLengths = Lib.HeapsAlgorithm.GetPermutations(allCities)
                .Select(route => route
                    .Zip(route.Skip(1), (a, b) => distances[(a, b)])
                    .Sum()
                );

            Console.WriteLine(routeLengths.Max()); // Part 1
        }
        {
            var (dist, n) = ParseGraph(input);
            return SolveHeldKarp(dist, n, findMax: true).ToString();
        }
    }
}
/* note
 tohle mi delalo problem celkem jsem to delal pres 3 hodiny ale mezitim jsem se sel umyt
 na zacatku jsem nevedel jak mam udelat graf jestli udelat class graf s List<verticie> a i List<edge>
 ale pak me napadlo ze logicky to je vlastne linked list s vice dalsima nodama takze jsem nakonec vzal Node a do toho nacpal list<node>
 a v grafu je taky List<node> napadlo me ze se to da delat rekurzivne a neni to djikstra protoze ten nenavstivi kazdy vertex ani kostra grafu
 problem mi delaly reference a vytvareni objektu DULEZITE je od ted nejdriv kontrolovat jestli ten objekt existuje a pak az ho vytvorit ja to delal naopak a musel se zeptat gemini
 dulezity bylo si vedomit ze edge je oboustrana takze londyn dublin funguje i jako dublin londyn
 pak uz stacilo jen projit kazdy node a z toho rekurzivne prohledavat kazdy edge
 ale jen ty nody ktery jsou v unchecked listu.
 ten bylo treba pres .ToList() prekopirovat aby to nebyla ta sama reference
 kdyz uz byl unchecked prazdny mohl jsem vypsat vysledek a ulozit ho do public promeny MinLength
 docela fanj bylo dat Traverse metodu do tridy graf aby ho videlaa mohla pouzivat MinLength
 dobra optimalizace od gemini by byla zmenit HashSet na Dictionary aby v metode GetOrCreate nemusela prohledavat cely Set a klic by byl string nazvu mesta treba "londyn",Node - "Londyn" - 3 Edges
*/
/* IDIOMATIC
 tohle slo udelat pres Permutace protze tu je jen 8 mest kodu zatim nerozumim
 misto hashSetu jsem mel pouzit Dictionary<int,int> pro rychlejsi lookup a node se uplne zbavit. ten HeldKarp bezi asi 4ms oproti memu za 80 a oproti permutacim za 220
 */