using System.Globalization;

namespace AdventOfCode.Year2016;

public class Day01 : BaseSolver
{
    public override int Year => 2016;
    public override int Day => 1;

    public enum DirectionFacing
    {
        North,
        East,
        South,
        West
    }

    struct Direction()
    {
        public bool IsRight { get; set; }
        public int Count { get; set; }
    }

    List<Direction> Parse(string[] input)
    {
        List<Direction> directions = new List<Direction>();
        foreach (var direction in input[0].Split(", "))
        {
            // JA TAM DAL JAKO COUNT JEN char direction[1] TAKZE TO NEBRALO ZBYTEK JAKO CISLO ALE JEN PRVNI ZNAK
            directions.Add(new Direction()
            {
                IsRight = direction[0] == 'R',
                Count = int.Parse(direction[1..])
            });
        }

        return directions;
    }

    string SolvePart2(List<Direction> directions)
    {
        HashSet<(int, int)> locationVisited = new HashSet<(int, int)>();
        int x = 0;
        int y = 0;
        DirectionFacing facing = DirectionFacing.North;
        foreach (var direction in directions)
        {
            if (direction.IsRight)
            {
                if ((int)facing == 3)
                    facing = 0;
                else
                    facing += 1;
            }
            else
            {
                if ((int)facing == 0)
                    facing = DirectionFacing.West;
                else
                    facing -= 1;
            }

            if (facing == DirectionFacing.North)
            {
                for (int yi = y; yi > y - direction.Count; yi--)
                {
                    if (!locationVisited.Add((x, yi)))
                    {
                        return (Math.Abs(x) + Math.Abs(yi)).ToString();
                    }
                }

                y -= direction.Count;
            }

            else if (facing == DirectionFacing.East)
            {
                for (int xi = x; xi < x + direction.Count; xi++)
                {
                    if (!locationVisited.Add((xi, y)))
                    {
                        return (Math.Abs(x) + Math.Abs(xi)).ToString();
                    }
                }

                x += direction.Count;
            }

            else if (facing == DirectionFacing.South)
            {
                for (int yi = y; yi < y + direction.Count; yi++)
                {
                    if (!locationVisited.Add((x, yi)))
                    {
                        return (Math.Abs(x) + Math.Abs(yi)).ToString();
                    }
                }

                y += direction.Count;
            }

            else if (facing == DirectionFacing.West)
            {
                for (int xi = x; xi > x - direction.Count; xi--)
                {
                    if (!locationVisited.Add((xi, y)))
                    {
                        return (Math.Abs(x) + Math.Abs(xi)).ToString();
                    }
                }

                x -= direction.Count;
            }
        }

        return 0.ToString();
    }

    string Solve(List<Direction> directions)
    {
        int x = 0;
        int y = 0;
        DirectionFacing facing = DirectionFacing.North;
        foreach (var direction in directions)
        {
            if (direction.IsRight)
            {
                if ((int)facing == 3)
                    facing = 0;
                else
                    facing += 1;
            }
            else
            {
                if ((int)facing == 0)
                    facing = DirectionFacing.West;
                else
                    facing -= 1;
            }

            if (facing == DirectionFacing.North)
                y -= direction.Count;
            else if (facing == DirectionFacing.East)
                x += direction.Count;
            else if (facing == DirectionFacing.South)
                y += direction.Count;
            else if (facing == DirectionFacing.West)
                x -= direction.Count;
        }


        return (Math.Abs(x) + Math.Abs(y)).ToString();
    }


    public override string SolvePart1(string[] input) => Solve(Parse(input));
    public override string SolvePart2(string[] input) => SolvePart2(Parse(input));
}