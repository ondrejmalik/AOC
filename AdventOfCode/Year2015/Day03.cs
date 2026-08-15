namespace AdventOfCode.Year2015;

public class Day03 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 3;

    public override string SolvePart1(string[] input)
    {
        int x = 0;
        int y = 0;
        List<(int, int)> visitedLocations = new List<(int, int)>();
        visitedLocations.Add((x, y));
        foreach (char c in input[0])
        {
            switch (c)
            {
                case '^':
                    y += 1;
                    break;
                case 'v':
                    y -= 1;
                    break;
                case '>':
                    x += 1;
                    break;
                case '<':
                    x -= 1;
                    break;
            }

            if (!visitedLocations.Contains((x, y)))
            {
                visitedLocations.Add((x, y));
            }
        }

        return visitedLocations.Count().ToString();
    }

    public override string SolvePart2(string[] input)
    {
        int xSanta = 0;
        int ySanta = 0;

        int xRobo = 0;
        int yRobo = 0;
        List<(int, int)> visitedLocations = new List<(int, int)>();
        visitedLocations.Add((xSanta, ySanta));
        for (int i = 0; i < input[0].Length; i++)
        {
            char c = input[0][i];
            if (i % 2 == 0) //santa
            {
                switch (c)
                {
                    case '^':
                        ySanta += 1;
                        break;
                    case 'v':
                        ySanta -= 1;
                        break;
                    case '>':
                        xSanta += 1;
                        break;
                    case '<':
                        xSanta -= 1;
                        break;
                }

                if (!visitedLocations.Contains((xSanta, ySanta)))
                {
                    visitedLocations.Add((xSanta, ySanta));
                }
            }
            else //robo santa
            {
                switch (c)
                {
                    case '^':
                        yRobo += 1;
                        break;
                    case 'v':
                        yRobo -= 1;
                        break;
                    case '>':
                        xRobo += 1;
                        break;
                    case '<':
                        xRobo -= 1;
                        break;
                }

                if (!visitedLocations.Contains((xRobo, yRobo)))
                {
                    visitedLocations.Add((xRobo, yRobo));
                }
            }
        }

        return visitedLocations.Count().ToString();
    }
}