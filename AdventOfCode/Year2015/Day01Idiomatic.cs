namespace AdventOfCode.Year2015;

public class Day01Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 1;

    public override string SolvePart1(string[] input)
    {
        int floor = 0;
        foreach (var (i, ch) in input[0].Index())
        {
            floor += ch == '(' ? 1 : -1;
        }

        return floor.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        int floor = 0;
        foreach (var (i, ch) in input[0].Index())
        {
            floor += ch == '(' ? 1 : -1;
            if (floor == -1) return (i + 1).ToString();
        }

        return "0";
    }
    /*notes
     *prvni ukol byl jednoduchy nejvic casu zabralo vypinani inline pomoci a github copilota
     *jediny co me napadlo je jak bych mel v c# resit iterace jestli spis pres foreach ale tam nemam indexy a nebo vzdycky pres for?
     */
    /*IDIOMATIC
     misto for loopu jde udelat .Index()
     a misto if else otaznik
     */
}