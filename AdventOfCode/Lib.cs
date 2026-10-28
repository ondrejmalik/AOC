namespace AdventOfCode;

public class Lib
{
    public static class HeapsAlgorithm
    {
        public static IEnumerable<IList<T>> GetPermutations<T>(IList<T> list)
        {
            var array = new T[list.Count];
            list.CopyTo(array, 0);

            return Generate(array.Length, array);
        }

        private static IEnumerable<IList<T>> Generate<T>(int k, T[] array)
        {
            if (k == 1)
            {
                // Yield a clone so downstream consumers don't have their copy mutated
                yield return (T[])array.Clone();
                yield break;
            }

            foreach (var p in Generate(k - 1, array))
                yield return p;

            for (int i = 0; i < k - 1; i++)
            {
                if (k % 2 == 0)
                    Swap(array, i, k - 1);
                else
                    Swap(array, 0, k - 1);

                foreach (var p in Generate(k - 1, array))
                    yield return p;
            }
        }

        private static void Swap<T>(T[] array, int a, int b)
        {
            (array[a], array[b]) = (array[b], array[a]);
        }
    }

    public static class IntegerPartitions
    {
        public static IEnumerable<int[]> GenerateSumCompositions(int n, int targetSum)
        {
            int[] current = new int[n];
            return Generate(0, targetSum);

            IEnumerable<int[]> Generate(int index, int remainingSum)
            {
                // Base case: for the last element, it must take whatever remains
                if (index == n - 1)
                {
                    current[index] = remainingSum;
                    int[] result = new int[n];
                    Array.Copy(current, result, n);
                    yield return result;
                    yield break;
                }

                // Loop through all valid values for the current slot
                for (int val = 0; val <= remainingSum; val++)
                {
                    current[index] = val;
                    foreach (var composition in Generate(index + 1, remainingSum - val))
                    {
                        yield return composition;
                    }
                }
            }
        }

        public static IEnumerable<int[]> GenerateRecursive(int index, int targetSum)
        {
            foreach (var VARIABLE in Recursive(0, new int[index], targetSum))
            {
                yield return VARIABLE;
            }
        }

        public static IEnumerable<int[]> Recursive(int index, int[] current, int remaining)
        {
            if (index == current.Length - 1)
            {
                current[index] = remaining;
                yield return (int[])current.Clone();
                yield break;
            }

            for (int val = 0; val <= remaining; val++)
            {
                current[index] = val;
                foreach (var VARIABLE in Recursive(index + 1, current, remaining - val))
                {
                    yield return VARIABLE;
                }
            }
        }
    }
}