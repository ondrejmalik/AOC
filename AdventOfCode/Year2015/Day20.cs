using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Year2015;

public class Day20 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 20;


    int BinarySearchHouseNumber(int targetPresents)
    {
        int low = 0, high = 16777215, mid = 0;
        while (low <= high)
        {
            mid = low + (high - low) / 2;
            long presents = PresentsHouse(mid);
            Console.WriteLine($"{mid} {presents}");
            if (presents == targetPresents)
            {
                return mid;
            }

            else if (presents <= targetPresents)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return mid;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int PresentsHouse(int houseNumber)
    {
        int totalPresents = 0;
        for (int elfNumber = 1; elfNumber <= houseNumber; elfNumber++)
        {
            if (houseNumber % elfNumber == 0)
            {
                totalPresents += elfNumber;
            }
        }

        return totalPresents;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int PresentsHousePart2(int houseNumber)
    {
        int totalPresents = 0;

        for (int elfNumber = 1; elfNumber <= houseNumber; elfNumber++)
        {
            if (elfNumber * 50 >= houseNumber && houseNumber % elfNumber == 0)
            {
                totalPresents += elfNumber * 11;
            }
        }

        return totalPresents;
    }

    public override string SolvePart1(string[] input)
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        int targetPresents = Int32.Parse(input[0]) / 10;
        int currentPresents = 0;
        int a = 0;
        /*
        //a = BinarySearchHouseNumber(targetPresents);
        Parallel.For(12000000 / 16, 12000000 / 15, houseNumber =>
        {
            int presentsHouse = PresentsHouse(houseNumber);

            if (presentsHouse >= targetPresents)
            {
                Console.WriteLine(presentsHouse);
                Console.WriteLine("!!!!!!!!!!!");
                Console.WriteLine(houseNumber);
            }

            houseNumber--;
        });
*/
        return a.ToString();
    }

    public override string SolvePart2(string[] input)
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        int targetPresents = Int32.Parse(input[0]);
        int currentPresents = 0;
        //a = BinarySearchHouseNumber(targetPresents);
        var a = Parallel.For(600_000, 786241, houseNumber =>
        {
            int presentsHouse = PresentsHousePart2(houseNumber);

            //Console.WriteLine(houseNumber);
            if (presentsHouse >= targetPresents)
            {
                Console.WriteLine("!!!!!!!!!!!");
                Console.WriteLine(presentsHouse);
                Console.WriteLine(houseNumber);
                Console.WriteLine("!!!!!!!!!!!");
            }
        });
        if (a.IsCompleted)
        {
            return a.ToString();
        }
        else
        {
            return "ni";
        }
    }
}
// u tohole me nenapadlo "chytre reseni mozna meoizace ale nevim
// rozhodne to neslo  ruteforcenout uz od nuly protoze to by trvalo hrozne dlouho takze jsem si uvedomil ze trend ty funkce je rostouci ale ne vzdycky takze binary search nefungoval na 100% ale priblizil me k tomu
// pak jsem jenom zmencoval rozsah dokud jsem prestaval dostavat rozumne hodnoty a udelal to pres Parallel.For aby to bylo rychlejsi
// v budoucnu bych se k tomu chtel vratit abych si mohl internalizovat jak se tohle melo resit jestli pres memoizaci nebo pres nejake pravidlo delitelnosti"