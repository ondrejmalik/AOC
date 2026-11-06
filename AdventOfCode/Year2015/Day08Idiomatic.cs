using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AdventOfCode.Year2015;

public class Day08Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 8;

    public override string SolvePart1(string[] input)
    {
        int sumCode = 0;
        int sumData = 0;
        foreach (var str in input)
        {
            sumCode += str.Length;
            sumData += Regex.Unescape(str).Length - 2;

            //Console.WriteLine($"{code} {data}");
        }


        return (sumCode - sumData).ToString();
    }

    public override string SolvePart2(string[] input)
    {
        int sumCode = 0;
        int sumData = 0;
        foreach (var line in input)
        {
            int code = line.Length + 2 + line.Count(c => c is '"' or '\\');
            int data = line.Length;

            int encodedLen = sumCode += code;
            sumData += data;

            //Console.WriteLine($"{code} {data}");
        }


        return (sumCode - sumData).ToString();
    }
}
/*IDIOMATIC
misto forloopu a hledani escape characteru je tu funkce regex.Unescape() ktera shodou nahod umi escapovat i hexadecimalni jako je to tady v tom priklade
druha cast je taky snadna proste prida k delce radku coz je vsechno bez dvou "" na zacatku a na konci finalniho stringu takze + 2 znaku za kazdy nalezeny escape znak to je  " a \ +1; nevyhoda je podle me ze z toho nedostanu ten puvodni code string
*/