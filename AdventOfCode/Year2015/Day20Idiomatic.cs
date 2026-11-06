using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Year2015;

public class Day20Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 20;


    public override string SolvePart1(string[] input) =>
        Solve(int.Parse(input[0]), multiplier: 10, maxDeliveries: int.MaxValue);

    public override string SolvePart2(string[] input) =>
        Solve(int.Parse(input[0]), multiplier: 11, maxDeliveries: 50);

    //ptame se kterym domum donese elf darek?
    //vychazi to z tohohle ale nedavam false u cisel ktere jsou nasobky ale 
    //https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
    private static string Solve(int target, int multiplier, int maxDeliveries)
    {
        int limit = target / multiplier;
        int[] houses = new int[limit];
        for (int elf = 1; elf < limit; elf++)
        {
            // Deliver presents
            for (int h = elf, count = 0; h < limit && count < maxDeliveries; h += elf, count++)
            {
                houses[h] += elf * multiplier;
            }

            // House 'elf' has received all presents it will EVER get!
            if (houses[elf] >= target)
                return elf.ToString();
        }

        return "Not found";
    }
}
// u tohole me nenapadlo "chytre reseni mozna meoizace ale nevim
// rozhodne to neslo  ruteforcenout uz od nuly protoze to by trvalo hrozne dlouho takze jsem si uvedomil ze trend ty funkce je rostouci ale ne vzdycky takze binary search nefungoval na 100% ale priblizil me k tomu
// pak jsem jenom zmencoval rozsah dokud jsem prestaval dostavat rozumne hodnoty a udelal to pres Parallel.For aby to bylo rychlejsi
// v budoucnu bych se k tomu chtel vratit abych si mohl internalizovat jak se tohle melo resit jestli pres memoizaci nebo pres nejake pravidlo delitelnosti"
/* IDIOMATIC
tohle jsem mel resit pres ne pres otazku kolik eflu navstivi tento dum ale KTERE DOMY NAVSTIVI ELF k temto domum pak pricitat dokud dum 'elf' nebude mit chtenou hodnotu
vratit z toho loopu muzu rovnou u te iterace pres elfy kdyz je dum[elf] vetsi nez target je to vlastne early return protoze je to prvni hodnota ktera bude vetsi a nikdy uz se nezmeni - nebude vetsi
*/