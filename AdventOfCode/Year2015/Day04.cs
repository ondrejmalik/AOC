using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2015;

public class Day04 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 4;

    public override string SolvePart1(string[] input)
    {
        String secret = input[0];
        StringBuilder s = new StringBuilder();
        s.Append(secret);
        for (int i = 0; i < Int32.MaxValue; i++)
        {
            s.Length = secret.Length;
            s.Append(i.ToString());
            byte[] data = Encoding.ASCII.GetBytes(s.ToString());
            var a = MD5.HashData(data);
            var b = Convert.ToHexString(a);
            if (b[0] == '0' &&
                b[1] == '0' &&
                b[2] == '0' &&
                b[3] == '0' &&
                b[4] == '0'
               )
            {
                return i.ToString();
            }
        }


        return "not found";
    }

    public override string SolvePart2(string[] input)
    {
        String secret = input[0];
        StringBuilder s = new StringBuilder();
        s.Append(secret);
        for (int i = 0; i < Int32.MaxValue; i++)
        {
            s.Length = secret.Length;
            s.Append(i.ToString());
            byte[] data = Encoding.ASCII.GetBytes(s.ToString());
            var a = MD5.HashData(data);
            var b = Convert.ToHexString(a);
            if (b[0] == '0' &&
                b[1] == '0' &&
                b[2] == '0' &&
                b[3] == '0' &&
                b[4] == '0' &&
                b[5] == '0'
               )
            {
                return i.ToString();
            }
        }


        return "not found";
    }
}
/*notes
 * tady jsem nevedel jak convertovat string na ascii a potom jak ho convertovat na hex
 * nejdriv jsem to delal pres BitConverter ale ten tam mel - a to se mi nelibilo
 * taky jsem mel problem se stringama aby jsem nedelal moc alokaci.
 * protoze stingy jsou imutable tak jsem pouzil string builder s pevnou casti pro secret
 * a chtel jsem tam insertit na pozici 6 (coz jsem musel predelat dynamicky protoze secrety maji jinou delku na secret.Length
 * insert nebyl dobry napad protoze to hazelo index out of bounds
 * musel jsem pouzit append ale pak jsem nevedel jak udelat aby se to vzdycky jen prepsalo
 * to jsem si nechal poradit od gemini ze to jde pres s.Length = secret.Length
 * taky me napadlo jestli kdybych si ulozil int lenght = secret.Length tak jestli by to nebylo rychlejsi protoze to je na stacku misto na heapu v loopu
 * druha cast stacilo pridat jeden && pro b[5]
 */