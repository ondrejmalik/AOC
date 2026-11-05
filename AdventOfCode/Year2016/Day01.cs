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

    public override string SolvePart1(string[] input)
    {
        List<Direction> directions = new List<Direction>();
        foreach (var split in input[0].Split(", "))
        {
            directions.Add(new Direction()
            {
                IsRight = split[0] == 'R',
                Count = split[1] - 48
            });
        }

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

        Console.WriteLine($"{x} {y}");
        Console.WriteLine(Math.Abs(x) + Math.Abs(y));
        return "Not implemented yet";
    }

    public override string SolvePart2(string[] input)
    {
        return "Not implemented yet";
    }
}