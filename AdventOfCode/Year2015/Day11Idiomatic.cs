namespace AdventOfCode.Year2015;

public class Day11Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 11;


    bool Rule1(char[] password)
    {
        bool ruleValid = false;


        for (int i = 0; i < password.Length - 2; i++)
        {
            char c = password[i];
            char cNPlus1 = password[i + 1];
            char cNPlus2 = password[i + 2];

            if (c + 1 == cNPlus1 && c + 2 == cNPlus2)
            {
                ruleValid = true;
                break;
            }
        }

        return ruleValid;
    }

    bool Rule2(char[] password)
    {
        for (int i = 0; i < password.Length; i++)
        {
            char c = password[i];

            if (c == 'i' || c == 'o' || c == 'l')
                return false;
        }

        return true;
    }

    bool Rule3(char[] password)
    {
        int numberOfPairs = 0;
        for (int i = 0; i < password.Length - 1;)
        {
            char c = password[i];
            char cNPlus1 = password[i + 1];

            //rule 3
            if (c == cNPlus1)
            {
                numberOfPairs++;
                i += 2;
            }
            else
                i++;
        }


        if (numberOfPairs >= 2)
            return true;

        return false;
    }

    void IncorementPassword(char[] password)
    {
        //zjednoduseni odometru
        // jde od konce a kdyz uvudi z tak ho zmeni na a kdyz ne jen posune pismenko a zkonci loop
        for (int i = password.Length - 1; i >= 0; i++)
        {
            if (password[i] == 'z') password[i] = 'a';
            else
            {
                password[i]++;
                break;
            }
        }
    }

    void CarryIncrement(char[] password, ref int i)
    {
        password[i] = 'a';

        if (password[i - 1] == 'z')
        {
            i -= 1;
            CarryIncrement(password, ref i);
        }
        else
        {
            password[i - 1] = (char)(password[i - 1] + 1);
            i = password.Length - 1;
        }
    }

    public override string SolvePart1(string[] input)
    {
        var password = input[0].ToArray();

        // 97 - 122
        while (true)
        {
            if (Rule1(password) && Rule2(password) && Rule3(password))
                break;

            IncorementPassword(password);
        }

        return new string(password.ToArray());
    }

    public override string SolvePart2(string[] input)
    {
        //result of part 1
        var password = SolvePart1(input).ToArray();

        IncorementPassword(password);
        // 97 - 122
        while (true)
        {
            if (Rule1(password) && Rule2(password) && Rule3(password))
                break;

            IncorementPassword(password);
        }

        return new string(password.ToArray());
    }
}
// Vpodstate jsem se rozhodoval jestli dat vzechny rules do 1 loopu ale nakonec jsem se rozhodl to rozdelit a resit jeden rule za druhym
// Dlouho mi nefungoval Iterator a az pres gemini jsem pochopil ze to funguje jako tachometr v aute - aby se zvysilo cislo vysiho radu musi se vratit na zacatek ty v tom soucastnem ja to pochopil tak, ze uz zustanou jako a
// Part 2 byla jen o optimalizaci aby to tolik nealokovalo
// v puvodnim reseni jsem delal string alokaci na kazdy novy heslo coz znamenalo 40 gbps alokaci vyresil jsem to pres string.ToList() kde muzu menit obsah bez nove alokace to znamena 1 alokace na heslo slo to taky udelat pres String builder
/* IDIOMATIC
zmena List<char> na char[] mohlo by byt i Span<char>  */
