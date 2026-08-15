namespace AdventOfCode.Year2015;

public class Day01 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 1;

    public override string SolvePart1(string[] input)
    {
        int i = 0;
        foreach (var line in input)
        {
            foreach (var c in line)
            {
                if (c == '(')
                    i += 1;
                else
                    i -= 1;
            }
        }

        return i.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        int floor = 0;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '(')
                    floor += 1;
                else
                    floor -= 1;

                if (floor == -1)
                {
                    return (i + j + 1).ToString();
                }
            }
        }

        return "0";
    }
}