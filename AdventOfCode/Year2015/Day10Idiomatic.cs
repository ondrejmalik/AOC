using System.Text;
using NUnit.Framework;

namespace AdventOfCode.Year2015;

public class Day10Idiomatic : BaseSolver
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

//run lenght encoding
    string Next(string s)
    {
        var sb = new StringBuilder(s.Length * 2);
        int count = 1;
        for (int i = 0; i < s.Length; i++)
        {
            if (i + 1 < s.Length && s[i] == s[i + 1])
                count++;
            else
            {
                sb.Append(count).Append(s[i]); // prida kolikrat se obevuje a hodnotu
                count = 1; //resetuje pocet na 1 kdyby se dva znaky po sobe lisily tak napise 1x treba 11 nebo 12
            }
        }

        return sb.ToString();
    }

    public override string SolvePart1(string[] input)
    {
        int i = 0;
        string next = input[0];
        while (i < 40)
        {
            next = Next(next);

            //Console.WriteLine(new string(chars.ToArray()));
            i++;
        }

        return $"{next.Length}";
    }


    public override string SolvePart2(string[] input)
    {
        int i = 0;
        string next = input[0];
        while (i < 50)
        {
            next = Next(next);

            //Console.WriteLine(new string(chars.ToArray()));
            i++;
        }

        return $"{next.Length}";
    }
}
// prvni cast jsem ze zacatku chctel delat z nejakeho duvodu rekurzivne ale to jsem nakonec opustil
// potom jsem si nevedel rady s tim jestli mam kontrolovat znak pred nebo za soucastnym ale nakonec jsem prisel na to ze bych mel udelat 2 loopy 1. jako soucastny a druhy jako offset
// kdyz uz jsem 1. cast dokoncil tak trvala hrozne dlouho pak jsem musel sprovoznit profiler pres main metodu. ze sampler profileru jsem zjistil ze 40% casu zabere concast stringu pres $"{repeat}{value}" string interpolation.
// to uz jsem se plat gemini jak na to abych nemusel interpolovat protoze v C# nejde modifikovat stringy bez nove alokace. gemini mi poradil ze muzu pracovat se string builderem nebo pres List<char>
// nakonec jsem mi gemini rekl ze ani nemusim convertovat char na int a staci jen porovnat char value a next jediny co pak musim convertovat je repeats na char pres '0' + repeats