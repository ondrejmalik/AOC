namespace AdventOfCode.Year2015;

public class Day05Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 5;

    int VowelsContained(string str, char[] vowels)
    {
        int containsVowels = 0;
        foreach (char vowel in vowels)
        {
            foreach (var c in str)
            {
                if (c == vowel)
                    containsVowels += 1;
            }
        }

        return containsVowels;
    }

    bool ContainsRepeat(string str)
    {
        for (int i = 0; i < str.Length - 1; i++)
        {
            if (str[i] == str[i + 1])
                return true;
        }

        return false;
    }

    bool ContainsForbiden(string str, string[] shouldNotContain)
    {
        foreach (var subStr in shouldNotContain)
        {
            if (str.Contains(subStr))
                return true;
        }

        return false;
    }

    public override string SolvePart1(string[] input)
    {
        char[] vowels = ['a', 'e', 'i', 'o', 'u'];
        string[] shouldNotContain = ["ab", "cd", "pq", "xy"];

        int niceStrings = 0;
        foreach (var str in input)
        {
            if (VowelsContained(str, vowels) < 3) continue;

            if (!ContainsRepeat(str)) continue;

            if (ContainsForbiden(str, shouldNotContain)) continue;

            niceStrings += 1;
        }

        return niceStrings.ToString();
    }

    bool HasPair(string str) =>
        Enumerable.Range(0, str.Length - 2)
            .Any(i => str.IndexOf(str[i..(i + 2)], i + 2, StringComparison.Ordinal) >= 0);

    bool HasRepeat(string str) => Enumerable.Range(0, str.Length - 2).Any(i => str[i] == str[i + 2]);

    public override string SolvePart2(string[] input)
    {
        int niceStrings = 0;
        foreach (var str in input)
        {
            if (!HasPair(str)) continue;
            if (!HasRepeat(str)) continue;

            niceStrings += 1;
        }


        return niceStrings.ToString();
    }
}

/*notes po dokonceni
 * prvni cast mi trvala asi 20 minut nejvic casu zabralo rozdeleni do funkci
 * druha cast mi zabrala asi 40 minut nejvic zabralo prvni pravidlo a taky to jak ho pojmenovat jestli rule1 nebo dlouze
 * druhou cast jsem checl nejdriv resit pres HashSet (ta je jeno bez klice a nepovoluje duplikaty)
 * pak pres dictionary ale to jsem si nakreslil a uvedomil jsem si ze stejne by se ulozil kazdy index jako klic
 * tak jsem to nakonec udelal iterativne a uvedomil jsem si ze nemusim pri dalsich charN a charN1 koukat zpatky od zacatku na otherN a otherN1 staci koukat na ty dalsi
 */
/* IDIOMATIC
predelani method ta aby byly bez nested for loopu. je to reseny pres Enumerable.Range(0,str.Length-2)  to je proste ten outer loop z puvodniho reseni potom se vezme .Any to je ten return true kdyz najde opakovani toho stringu str[i..(i+2)] --POZOR tohle jsou 2 znaky protoze i+2 je exclusive
tenhle string pak pres IndexOf zacne prohledavat od i+2 inclusive taze ve stringu aabcdf ma string aa a porovnava v prvnim kroku s cd v druhem s df kdyz je vetsi nez 0 tak to znamena ze to naslo druhy aa
    bool ContainsPairNonOverlapping(string str) a bool ContainsLetterRepeatingWithLetterBetween(string str)
 */