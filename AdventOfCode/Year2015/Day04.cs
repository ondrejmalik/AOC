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
                b[4] == '0' &&
                b[5] == '0'
               )
            {
                return i.ToString();
            }
        }


        return "not found";
    }

    public override string SolvePart2(string[] input)
    {
        return "Not implemented yet";
    }
}