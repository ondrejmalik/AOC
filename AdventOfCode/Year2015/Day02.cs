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
/*notes
 * tady jsem si to chtel rozdelit do tupple ale nakonec se ukazalo ze je lepsi list protoze ho pak musim seradit v druhy casti
 * Musel jsem si nechat poradit od gemini protoze me nenapadlo v druhy casti ze cisla ve stringu se neradi jako celek ale podle velikost v characteru
 * takze 10 je mensi nez 2 protoze 1 ma mensi ascii nez 2
 * taky jsem to chtel udelat elegantneji ne pres nejaky razeni pole treba jestli by nestacilo 1 nebo 2 Math.Min a z toho vydedukovat ktery vyradit na to jsem ale neprisel
 */