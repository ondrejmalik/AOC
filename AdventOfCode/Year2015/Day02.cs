namespace AdventOfCode.Year2015;

public class Day02 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 2;

    public override string SolvePart1(string[] input)
    {
        int sum = 0;
        foreach (var dimensions in input)
        {
            var xyz = dimensions.Split('x');
            var (l, w, h) = (Int32.Parse(xyz[0]), Int32.Parse(xyz[1]), Int32.Parse(xyz[2]));
            //2*l*w + 2*w*h + 2*h*l
            int area = 2 * l * w + 2 * w * h + 2 * h * l;

            int extra = Math.Min(Math.Min(l * w, l * h), w * h);

            sum += area + extra;
        }

        return sum.ToString();
    }


    public override string SolvePart2(string[] input)
    {
        int sum = 0;
        foreach (var dimensions in input)
        {
            var xyz = dimensions.Split('x').Select(Int32.Parse).ToArray();
            xyz.Sort();
            var (x, y, z) = (xyz[0], xyz[1], xyz[2]);
            int wrap = x + x + y + y;
            int bow = x * y * z;
            sum += wrap + bow;
        }

        return sum.ToString();
    }
}