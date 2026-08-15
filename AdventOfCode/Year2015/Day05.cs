namespace AdventOfCode.Year2015;

public class Day05 : BaseSolver
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
                {
                    containsVowels += 1;
                }
            }
        }

        return containsVowels;
    }

    bool ContainsRepeat(string str)
    {
        for (int i = 0; i < str.Length - 1; i++)
        {
            if (str[i] == str[i + 1])
            {
                return true;
            }
        }

        return false;
    }

    bool ContainsForbiden(string str, string[] shouldNotContain)
    {
        foreach (var subStr in shouldNotContain)
        {
            if (str.Contains(subStr))
            {
                return true;
            }
        }

        return false;
    }

    public override string SolvePart1(string[] input)
    {
        char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
        string[] shouldNotContain = new[] { "ab", "cd", "pq", "xy" };

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

    bool ContainsPairNonOverlapping(string str)
    {
        for (int i = 0; i < str.Length - 2; i++)
        {
            char charN = str[i];
            char charN1 = str[i + 1];
            //don't care about n+1 that is overlap
            for (int j = i + 2; j < str.Length - 1; j++)
            {
                char otherN = str[j];
                char otherN1 = str[j + 1];
                if (charN == otherN && charN1 == otherN1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool ContainsLetterRepeatingWithLetterBetween(string str)
    {
        for (int i = 0; i < str.Length - 2; i++)
        {
            if (str[i] == str[i + 2]) return true;
        }

        return false;
    }

    public override string SolvePart2(string[] input)
    {
        int niceStrings = 0;
        foreach (var str in input)
        {
            if (!ContainsPairNonOverlapping(str)) continue;
            if (!ContainsLetterRepeatingWithLetterBetween(str)) continue;

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