using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015;

public class Day25Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 25;

    public override string SolvePart1(string[] input)
    {
        var matches = Regex.Matches(input[0], "\\d+");
        int row = int.Parse(matches[0].Value);
        int column = int.Parse(matches[1].Value);
        //trojuhelnihove cislo ((r+c-2)(r+c-1)/2)+c
        var n = ((row + column - 2) * (row + column - 1) / 2) + column -
                1; //n je placement kolikate cislo vybereme -1 protoze 1. kod uz mame
        long factor = (long)BigInteger.ModPow(252533, n, 33554393);
        long result = (20151125 * factor) % 33554393;
        return result.ToString();
    }


    public override string SolvePart2(string[] input)
    {
        //tady bylo jen insert 50 stars :) 
        return "Not implemented yet";
    }
}
// nejdriv jsem to delal uplne naopak chctel jsem pracovat s tou tabulkou tak jsem si udelal funkci GetCode at a pres modulo jsem nejak chctel zjistovat podle indexu kodu jeho row a column to se mi ale nepovedlo protoze jsem nevedel cim ho mam v modulu delit ted uz vim ze to je index % column(asi nezkousel jsem)
// tady si s tacilo uvedomit co vlastne potrebuju ta tabulka v prikladu byla jen na to abych si zkontroloval jestli to na tech par prvnich prvku funguje jinak stacilo zadat prvni kod a vysledek byl druhy kod coz byl argument pro 3. kod atd
// fixnul jsem 2 budy prvni co me napadlo bylo printovat row column code a overit jestli se opravdu v prikladu nachazi ale cisla zacinaly od row 2 column 2 tak jsem si uvedomil ze ten for loop musi ommitnout prvni kod na row 1 column 1 protoze ten uz mam v zadani
// druhy bug ktery nastal byl ze jsem nezacinal s column na 1 ale na tom naximalnim nakze jsem udelal columnCurrent a row jsem nechal na column jako start loop se pak breakne kdyz souradnice sedi na prikladu to pak fungovalo a vysledek taky
// 

/* IDIOMATIC
 thle jde resit pres trojuhelnikova cisla  pres tenhle vzorec ((r+c-2)(r+c-1)/2)+c to je poradi cisla ktere hledame a iterace nasobeni a modula jde udelat primo pres funkci BigInteger.ModPow();
 */