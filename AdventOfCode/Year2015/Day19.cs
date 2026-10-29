using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015;

public class Day19 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 19;

    HashSet<string> CombinationsPart1(string input, Dictionary<string, HashSet<string>> dictionary)
    {
        HashSet<string> combinations = new HashSet<string>();
        foreach (var kvp in dictionary)
        {
            string key = kvp.Key;
            HashSet<string> values = kvp.Value;

            combinations.UnionWith(FindCombinations(input, key, values));
        }

        return combinations;
    }

    HashSet<string> FindCombinations(string input, string key, HashSet<string> values)
    {
        HashSet<string> combinations = new();
        var regex = new Regex(Regex.Escape(key));
        foreach (string value in values)
        {
            for (int startIndex = 0; startIndex < input.Length;)
            {
                startIndex = input.IndexOf(key, startIndex);
                if (startIndex == -1)
                    break;
                string newCombination = regex.Replace(input.Substring(startIndex), value, 1);


                var sb = new StringBuilder(input.Substring(0, startIndex));
                sb.Append(newCombination);
                combinations.Add(sb.ToString());
                startIndex += key.Length;
            }
        }

        return combinations;
    }

    public static class MinCounter
    {
        private static int _minCount = int.MaxValue;

        public static int MinCount
        {
            get => _minCount;
            set => _minCount = Math.Min(_minCount, value);
        }
    }

    bool CombinationsPart2(string finish, string currentString,
        Dictionary<string, HashSet<string>> dictionary, int steps)
    {
        //Console.WriteLine(steps);
        if (currentString == finish)
        {
            MinCounter.MinCount = steps;
            return true;
        }

        if (currentString.Length > finish.Length)
        {
            return false;
        }


        foreach (var kvp in dictionary)
        {
            string key = kvp.Key;
            var regex = new Regex(Regex.Escape(key));

            foreach (var value in kvp.Value)
            {
                for (int startIndex = 0; startIndex < currentString.Length;)
                {
                    startIndex = currentString.IndexOf(key, startIndex, StringComparison.Ordinal);

                    if (startIndex == -1)
                        break;

                    string newCombination = regex.Replace(currentString.Substring(startIndex), value, 1);

                    var sb = new StringBuilder(currentString.Substring(0, startIndex));
                    sb.Append(newCombination);
                    string newString = sb.ToString();
                    CombinationsPart2(finish, newString, dictionary, steps + 1);


                    startIndex += key.Length;
                }
            }
        }

        return true;
    }

    bool CombinationsPart2Backwards(string currentString,
        List<(string, string)> list, int steps)
    {
        if (currentString == "e")
        {
            //Console.WriteLine(steps);
            MinCounter.MinCount = steps;
            return true;
        }

        foreach (var kvp in list)
        {
            string key = kvp.Item1;
            string value = kvp.Item2;

            for (int startIndex = 0; startIndex < currentString.Length;)
            {
                startIndex = currentString.IndexOf(key, startIndex, StringComparison.Ordinal);

                if (startIndex == -1)
                    break;

                string newString = string.Concat(
                    currentString.AsSpan(0, startIndex),
                    value,
                    currentString.AsSpan(startIndex + key.Length)
                );

                if (CombinationsPart2Backwards(newString, list, steps + 1))
                    return true;

                startIndex += key.Length;
            }
        }

        return true;
    }

    public override string SolvePart1(string[] input)
    {
        Dictionary<string, HashSet<string>> dictionary = new();
        for (int i = 0; i < input.Length - 2; i++)
        {
            string line = input[i];
            string[] splits = line.Split("=>");
            string key = splits[0].TrimEnd();
            string value = splits[1].TrimStart();
            if (dictionary.ContainsKey(key))
            {
                var a = dictionary[key];
                a.Add(value);
            }
            else
            {
                dictionary.Add(key, new HashSet<string>() { value });
            }
        }

        string start = input[^1];

        return CombinationsPart1(start, dictionary).Count.ToString();
        ;
    }

    public override string SolvePart2(string[] input)
    {
        List<(string, string)> list = new();
        for (int i = 0; i < input.Length - 2; i++)
        {
            string line = input[i];
            string[] splits = line.Split("=>");
            string key = splits[1].Trim();
            string value = splits[0].Trim();
            list.Add((key, value));
        }

        string finish = input[^1];

        //return CombinationsPart2(finish, "e", dictionary, 0).ToString();
        Stopwatch s = new Stopwatch();
        s.Start();
        CombinationsPart2Backwards(finish, list, 0);

        Console.WriteLine(s.Elapsed);
        return MinCounter.MinCount.ToString();
    }
}
//Tohle bylo relativne tezky nejdriv jsem to chtel udelat bez modifikaci stringu ale to byl spatny napad - chtel jsem proste vynasobit pocet values v kvp s occurences proto ty HashSety v value ale protoze ten replace meni char tak se to rozbije
//pak jsem pristoupil k dict string List string prislo mi to v part1 logictejsi nez prosty list(string,string) protoze se tam casto vyskytuji stejne klice jestli to je skutecne rychlejsi nevim
//pres HashSet v resultu kontroluju duplicity
// druhou cast jsem nejdriv chctel delat pres replace key na value dokud nevijde e ale nakonec mi prislo rychlejsi jit od value na key a string tak zmencovat
// nejdriv jsem to delal pres regex.Replace ale kdyz jsem se pak koukal na profiler tak jsem zjistil ze nejvic casu se stravi v regex ctor takze jsem se zeptal az po vyreseni gemini na optimalizace.
// ten mi poradil ze se ho muzu zbavit pres string.concat a passnout .AsSpan()
// dale mi poradil ze muzu flattennout dictionary na List(string,string)> s tim tady souhlasim protoze key ma vzdy jenom jeden value a tady to rychlejsi bude zaroven jsem se tim zbavil hashSetu
// nakonec mi poradil ze muzu pridat podminku do kekurze kdyz calle() vrati true jakoze naslo e tak muzu rovnou vracet a neprohledavat dal v loopu nenapadlo me to protoze jsem si myslel ze se tim z nejakeho duvodu pripravim o vysledek s mene steps ale logicky ne protoze kazdy call prida 1 step a uz takhle vim ze reseni je steps + 1