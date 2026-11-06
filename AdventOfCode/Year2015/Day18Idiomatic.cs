namespace AdventOfCode.Year2015;

public class Day18Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 18;

    int CountNeighbors(bool[,] grid, int x, int y, int w, int h)
    {
        int count = 0;
        for (int dy = -1; dy <= 1; dy++)
        for (int dx = -1; dx <= 1; dx++)
        {
            // 1. Skip the center cell itself:
            if (dx == 0 && dy == 0)
                continue;
            // 2. Compute the neighbor's actual coordinates:
            int nx = x + dx; // "neighbor x"
            int ny = y + dy; // "neighbor y"
            // 3. Boundary check:
            // Is the neighbor inside the grid boundaries?
            if (nx >= 0 && nx < w && ny >= 0 && ny < h)
            {
                // 4. If it's valid and lit, count it:
                if (grid[nx, ny])
                    count++;
            }
        }

        return count;
    }

    bool[,] StepSim(bool[,] currentStep, bool leaveEdgesOn = false)
    {
        bool[,] nextStep = (bool[,])currentStep.Clone();
        for (int y = 0; y < currentStep.GetLength(1); y++)
        {
            for (int x = 0; x < currentStep.GetLength(0); x++)
            {
                if (leaveEdgesOn && ((x == 0 && y == 0) ||
                                     (x == 0 && y == currentStep.GetLength(1) - 1) ||
                                     (x == currentStep.GetLength(0) - 1 && y == 0) ||
                                     (x == currentStep.GetLength(0) - 1 && y == currentStep.GetLength(1) - 1)
                    ))
                    continue;
                int litNeighbours =
                    CountNeighbors(currentStep, x, y, currentStep.GetLength(0), currentStep.GetLength(1));

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
            grid = StepSim(grid, true);

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

/*IDIOMATIC
prepsani slozite logiky do funkce count neighbours a odstraneni duplicitniho StepSim je to ale pomalejsi
*/