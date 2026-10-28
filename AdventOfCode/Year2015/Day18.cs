namespace AdventOfCode.Year2015;

public class Day18 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 18;


    bool[,] StepSim(bool[,] currentStep)
    {
        bool[,] nextStep = (bool[,])currentStep.Clone();
        for (int y = 0; y < currentStep.GetLength(1); y++)
        {
            for (int x = 0; x < currentStep.GetLength(0); x++)
            {
                int litNeighbours = 0;
                if (y - 1 >= 0)
                {
                    //-1 -1
                    if (x - 1 >= 0 && currentStep[x - 1, y - 1])
                        litNeighbours++;
                    //-1 0
                    if (currentStep[x, y - 1])
                        litNeighbours++;
                    //-1 1
                    if (x + 1 < currentStep.GetLength(0) && currentStep[x + 1, y - 1])
                        litNeighbours++;
                }

                //0 -1
                if (x - 1 >= 0 && currentStep[x - 1, y])
                    litNeighbours++;
                //0 1
                if (x + 1 < currentStep.GetLength(0) && currentStep[x + 1, y])
                    litNeighbours++;

                if (y + 1 < currentStep.GetLength(1))
                {
                    //1 -1
                    if (x - 1 >= 0 && currentStep[x - 1, y + 1])
                        litNeighbours++;
                    //1 0
                    if (currentStep[x, y + 1])
                        litNeighbours++;
                    //1 1
                    if (x + 1 < currentStep.GetLength(0) && currentStep[x + 1, y + 1])
                        litNeighbours++;
                }

                if (currentStep[x, y])
                {
                    if (!(litNeighbours == 2 || litNeighbours == 3))
                        nextStep[x, y] = false;
                }
                else
                {
                    if (litNeighbours == 3)
                        nextStep[x, y] = true;
                }
            }
        }

        return nextStep;
    }

    bool[,] StepSimPart2(bool[,] currentStep)
    {
        bool[,] nextStep = (bool[,])currentStep.Clone();
        for (int y = 0; y < currentStep.GetLength(1); y++)
        {
            for (int x = 0; x < currentStep.GetLength(0); x++)
            {
                if ((x == 0 && y == 0) ||
                    (x == 0 && y == currentStep.GetLength(1) - 1) ||
                    (x == currentStep.GetLength(0) - 1 && y == 0) ||
                    (x == currentStep.GetLength(0) - 1 && y == currentStep.GetLength(1) - 1)
                   )
                {
                    continue;
                }


                int litNeighbours = 0;
                if (y - 1 >= 0)
                {
                    //-1 -1
                    if (x - 1 >= 0 && currentStep[x - 1, y - 1])
                        litNeighbours++;
                    //-1 0
                    if (currentStep[x, y - 1])
                        litNeighbours++;
                    //-1 1
                    if (x + 1 < currentStep.GetLength(0) && currentStep[x + 1, y - 1])
                        litNeighbours++;
                }

                //0 -1
                if (x - 1 >= 0 && currentStep[x - 1, y])
                    litNeighbours++;
                //0 1
                if (x + 1 < currentStep.GetLength(0) && currentStep[x + 1, y])
                    litNeighbours++;

                if (y + 1 < currentStep.GetLength(1))
                {
                    //1 -1
                    if (x - 1 >= 0 && currentStep[x - 1, y + 1])
                        litNeighbours++;
                    //1 0
                    if (currentStep[x, y + 1])
                        litNeighbours++;
                    //1 1
                    if (x + 1 < currentStep.GetLength(0) && currentStep[x + 1, y + 1])
                        litNeighbours++;
                }

                if (currentStep[x, y])
                {
                    if (!(litNeighbours == 2 || litNeighbours == 3))
                        nextStep[x, y] = false;
                }
                else
                {
                    if (litNeighbours == 3)
                        nextStep[x, y] = true;
                }
            }
        }

        return nextStep;
    }

    public override string SolvePart1(string[] input)
    {
        bool[,] grid = new bool[input[0].Length, input.Length];

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                if (input[y][x] == '#')
                    grid[x, y] = true;

                if (input[y][x] == '.')
                    grid[x, y] = false;
            }
        }

        for (int i = 0; i < 100; i++)
            grid = StepSim(grid);

        int count = 0;
        foreach (bool b in grid)
        {
            if (b)
                count++;
        }

        return count.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        bool[,] grid = new bool[input[0].Length, input.Length];

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                if (input[y][x] == '#')
                    grid[x, y] = true;

                if (input[y][x] == '.')
                    grid[x, y] = false;
            }
        }

        grid[0, 0] = true;
        grid[0, grid.GetLength(1) - 1] = true;

        grid[grid.GetLength(0) - 1, 0] = true;
        grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1] = true;

        for (int i = 0; i < 100; i++)
            grid = StepSimPart2(grid);

        int count = 0;
        foreach (bool b in grid)
        {
            if (b)
                count++;
        }

        return count.ToString();
    }
}
// tady jsem prohodil x a y protoze jsem to chtel indexovat[x,y] ale hrozne me to popletlo
// taky se mi podaril off by one error protoze jsem checkoval jako krajni bod .GetLength() misto GetLength()-1