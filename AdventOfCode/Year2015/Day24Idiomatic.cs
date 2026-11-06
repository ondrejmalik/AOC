namespace AdventOfCode.Year2015;

public class Day24Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 24;

    public override string SolvePart1(string[] input) => Solve(input, groupCount: 3);

    public override string SolvePart2(string[] input) => Solve(input, groupCount: 4);

    private static string Solve(string[] input, int groupCount)
    {
        // 1. Načtení a seřazení vah sestupně (od nejtěžšího)
        int[] weights = [.. input.Select(int.Parse).OrderByDescending(w => w)];
        int targetWeight = weights.Sum() / groupCount;

        int bestCount = int.MaxValue;
        long bestQe = long.MaxValue;

        // 2. Binární backtracking (0/1 výběr) jako čistá lokální funkce
        void Backtrack(int index, int currentSum, int count, long qe)
        {
            // Úspěch: Přesný součet vah pro první skupinu
            if (currentSum == targetWeight)
            {
                if (count < bestCount)
                {
                    bestCount = count;
                    bestQe = qe;
                }
                else if (count == bestCount && qe < bestQe)
                {
                    bestQe = qe;
                }

                return;
            }

            // Ořezání neperspektivních větví
            if (currentSum > targetWeight || count >= bestCount || index >= weights.Length)
            {
                return;
            }

            // Větev 1: VEZMU balíček weights[index]
            Backtrack(
                index + 1,
                currentSum + weights[index],
                count + 1,
                qe * weights[index]
            );

            // Větev 2: PŘESKOČÍM balíček weights[index]
            Backtrack(
                index + 1,
                currentSum,
                count,
                qe
            );
        }

        // 3. Spuštění od indexu 0
        Backtrack(index: 0, currentSum: 0, count: 0, qe: 1L);

        return bestQe.ToString();
    }
}