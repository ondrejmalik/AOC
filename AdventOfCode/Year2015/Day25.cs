namespace AdventOfCode.Year2015;

public class Day25 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 25;

    public override string SolvePart1(string[] input)
    {
        long currentCode = 20151125;
        for (int column = 2; true; column++)
        {
            int columnCurrent = 1;

            bool breakOuter = false;
            for (int row = column; row >= 1; row--)
            {
                currentCode = NextCode(currentCode);
                if (row == 2981 && columnCurrent == 3075)
                {
                    Console.WriteLine($"{row} {columnCurrent} {currentCode}");
                    breakOuter = true;
                    break;
                }


                columnCurrent++;
            }

            if (breakOuter)
                break;
        }

        return currentCode.ToString();
    }

    long NextCode(long currentCode)
    {
        return (currentCode * 252533) % 33554393;
    }

    int GetCodeAt(List<int> codes, int row, int column)
    {
        var mod = column;
        return 0;
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