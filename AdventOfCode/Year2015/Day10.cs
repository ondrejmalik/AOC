using System.Text;
using NUnit.Framework;

namespace AdventOfCode.Year2015;

public class Day10 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 10;

    const int OFFSET_START = 1;

    List<char> Iteration(List<char> chars)
    {
        char value;
        char next;
        int repeats = OFFSET_START;
        int i = 0;
        List<char> output = new((int)(chars.Count * 1.4));
        while (i < chars.Count)
        {
            value = chars[i];

            for (int offset = OFFSET_START; i + offset < chars.Count; offset++)
            {
                next = chars[i + offset];
                if (value == next)
                    repeats += 1;
                else
                {
                    break;
                }
            }

            output.Add((char)('0' + repeats));
            output.Add(value);

            i += repeats;
            repeats = OFFSET_START;
        }

        return output;
    }

    public override string SolvePart1(string[] input)
    {
        List<char> chars = input[0].ToList();
        int i = 0;
        while (i < 40)
        {
            chars = Iteration(chars.ToList());

            //Console.WriteLine(new string(chars.ToArray()));
            i++;
        }

        return $"{chars.Count}";
    }


    public override string SolvePart2(string[] input)
    {
        List<char> chars = input[0].ToList();
        int i = 0;
        while (i < 50)
        {
            chars = Iteration(chars);

            //Console.WriteLine(new string(chars.ToArray()));
            i++;
        }

        return $"{chars.Count}";
    }
}
// prvni cast jsem ze zacatku chctel delat z nejakeho duvodu rekurzivne ale to jsem nakonec opustil
// potom jsem si nevedel rady s tim jestli mam kontrolovat znak pred nebo za soucastnym ale nakonec jsem prisel na to ze bych mel udelat 2 loopy 1. jako soucastny a druhy jako offset
// kdyz uz jsem 1. cast dokoncil tak trvala hrozne dlouho pak jsem musel sprovoznit profiler pres main metodu. ze sampler profileru jsem zjistil ze 40% casu zabere concast stringu pres $"{repeat}{value}" string interpolation.
// to uz jsem se plat gemini jak na to abych nemusel interpolovat protoze v C# nejde modifikovat stringy bez nove alokace. gemini mi poradil ze muzu pracovat se string builderem nebo pres List<char>
// nakonec jsem mi gemini rekl ze ani nemusim convertovat char na int a staci jen porovnat char value a next jediny co pak musim convertovat je repeats na char pres '0' + repeats