using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015;

public class Day06Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 6;

    int FindWhiteSpaceIndexLeft(string str, int index)
    {
        for (int i = index; i > index - 4; i--)
        {
            if (str[i] == ' ')
            {
                return i + 1;
            }
        }

        return -1;
    }

    int FindWhiteSpaceIndexRight(string str, int index)
    {
        for (int i = index; i < index + 4; i++)
        {
            if (str[i] == ' ')
            {
                return i - 1;
            }

            if (i == str.Length - 1)
            {
                return i;
            }
        }

        return -1;
    }

    int CountLit(bool[,] grid)
    {
        int count = 0;
        foreach (var light in grid)
        {
            if (light)
                count += 1;
        }

        return count;
    }

    int CountLitBrightness(int[,] grid)
    {
        int count = 0;
        foreach (var light in grid)
        {
            count += light;
        }

        return count;
    }

    public override string SolvePart1(string[] input)
    {
        bool[,] grid = new bool[1000, 1000];
        foreach (var line in input)
        {
            var m = Regex.Match(line, @"(\d+),(\d+) through (\d+),(\d+)");

            (int x, int y) start = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
            (int x, int y) end = (int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));

            if (m.Groups[0].Value.StartsWith("turn on"))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int x = start.x; x <= end.x; x++)
                    {
                        grid[x, y] = true;
                    }
                }
            }
            else if (line.StartsWith("toggle"))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int x = start.x; x <= end.x; x++)
                    {
                        grid[x, y] = !grid[x, y];
                    }
                }
            }
            else if (line.StartsWith("turn off"))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int x = start.x; x <= end.x; x++)
                    {
                        grid[x, y] = false;
                    }
                }
            }
        }


        return CountLit(grid).ToString();
    }

    public override string SolvePart2(string[] input)
    {
        int[,] bools = new int[1000, 1000];
        int[,] grid = bools;
        foreach (var line in input)
        {
            var m = Regex.Match(line, @"(\d+),(\d+) through (\d+),(\d+)");

            (int x, int y) start = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
            (int x, int y) end = (int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));


            if (line.StartsWith("turn on"))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int x = start.x; x <= end.x; x++)
                    {
                        grid[x, y] += 1;
                    }
                }
            }
            else if (line.StartsWith("toggle"))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int x = start.x; x <= end.x; x++)
                    {
                        grid[x, y] += 2;
                    }
                }
            }
            else if (line.StartsWith("turn off"))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int x = start.x; x <= end.x; x++)
                    {
                        grid[x, y] = Math.Max(grid[x, y] - 1, 0);
                    }
                }
            }
        }


        return CountLitBrightness(grid).ToString();
    }
}
/*notes
  tohle trvalo asi hodinu.
  problem byl hlavne v parsovani cisel a pak v indexaci prepinani kde muselo byt <= misto <
  nejdriv jsem to chtel udelat pres substring radku bez turn on/ off /toggle a ten pak jeste rozdelit podle through
  ale to mi prislo neefektivni misto toho jsem si uvedomil ze cislo bude u ',' a musel jsem teda hledat zacatek a konec cisla pres IndexOf coz zabralo hodne casu
  nevim jak bych to udelal rychlejc mozna pres regex a hledat cisla? nebo nejak jinak?
  druha cast tam stacilo jenom prepsat bool na int.
  Taky me napadlo jestli by to neslo bez toho dvourozmernyho pole a rovnou to nejak vydedukovat z tech lokaci bez toho abych pouzival pamet
*/
/* IDIOMATIC
 tady byl bug bug ktery jsem si nevsiml ma tam byt num2+1 uz jsem ho opravil
            {
                int substringStart = num2 + 1;
                int substringLenght = FindWhiteSpaceIndexRight(line, num2) - substringStart + 1;
                end.y = Int32.Parse(line.Substring(substringStart, substringLenght));
            }
    v idiomatickem reseni tohle vubec neresim a delam to pres regex coz je mnohem prehlednejsi
    u regexu je indexace tak ze 0 je cely match 1 [1] == startX [2] startY atd 
*/