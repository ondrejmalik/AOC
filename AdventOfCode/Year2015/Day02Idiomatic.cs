namespace AdventOfCode.Year2015;

public class Day02Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 2;

    public override string SolvePart1(string[] input)
    {
        int sum = 0;
        foreach (var dimensions in input)
        {
            int[] dims = [.. dimensions.Split('x').Select(int.Parse)];
            dims.Sort();
            var (x, y, z) = (dims[0], dims[1], dims[2]);
            //2*l*w + 2*w*h + 2*h*l
            int area = 2 * (x * y + y * z + z * x) + x * y;

            sum += area;
        }

        return sum.ToString();
    }


    public override string SolvePart2(string[] input)
    {
        int sum = 0;
        foreach (var dimensions in input)
        {
            int[] dims = [.. dimensions.Split('x').Select(int.Parse)];
            dims.Sort();
            var (x, y, z) = (dims[0], dims[1], dims[2]);
            int wrap = 2 * (x + y);
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
    /*IDIOMATIC
     misto array stringu vytvori int[] a pres kolekce [..dimensions.Split('x').Select(int.Parse)] ze stringu jsou rovnou inty
     ty se pak seradi a nemusim resit ktery je mensi a ktery vetsi zaroven to resi problem co jsem mel ze nemusim radit podle characteru ale podle hodnoty cisla
     nakonec se prepsaly ty rovnice aby se neopakovali konstanty
     */
