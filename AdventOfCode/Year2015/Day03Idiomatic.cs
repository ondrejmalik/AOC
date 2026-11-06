namespace AdventOfCode.Year2015;

public class Day03Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 3;

    public override string SolvePart1(string[] input)
    {
        int x = 0;
        int y = 0;
        List<(int, int)> visitedLocations = [(x, y)];
        foreach (char c in input[0])
        {
            switch (c)
            {
                case '^':
                    y += 1;
                    break;
                case 'v':
                    y -= 1;
                    break;
                case '>':
                    x += 1;
                    break;
                case '<':
                    x -= 1;
                    break;
            }

            if (!visitedLocations.Contains((x, y)))
            {
                visitedLocations.Add((x, y));
            }
        }

        return visitedLocations.Count().ToString();
    }

    public override string SolvePart2(string[] input)
    {
        (int x, int y)[] pos = [(0, 0), (0, 0)];

        HashSet<(int, int)> visitedLocations = [(0, 0)];
        for (int i = 0; i < input[0].Length; i++)
        {
            ref var p = ref pos[i % 2];
            p = input[0][i] switch
            {
                '^' => (p.x, p.y + 1),
                'v' => (p.x, p.y - 1),
                '>' => (p.x + 1, p.y),
                '<' => (p.x - 1, p.y),
                _ => p
            };

            visitedLocations.Add(p);
        }

        return visitedLocations.Count().ToString();
    }
}
/*notes
 *tady v prvni casti jsem nejdriv chtel udelat 2d pole boolu ale to pak neni nekonecny a pametove neefektivni
 * nakonec jsem se rozhodl ukladat navstiveny pozice do listu<int,int> a kontrolovat jestli jsem tam uz byl
 * v druhy casti jsem si nebyl jistej jestli ten if
 *   if (!visitedLocations.Contains((xRobo, yRobo)))
                {
                    visitedLocations.Add((xRobo, yRobo));
                }
 * dat pro santu i robo santu zvlast nebo nejak nakonec pro oba stejny
 * ale nevedel jsem jak elegantne udelat aby se ulozily ty spravny promeny jednou santa a pak robo
 */

/*IDIOMATIC
 v druhe casti z int santaX, santaY a roboX, roboY jsem udelal (int x,int y)[]
 pres
            ref var p = ref pos[i % 2];
            jsem ziskal pozici santy nebo pozici robo podle zbytku po deleni a pres swtich match statement jsem ulozil
            zaroven jsem mohl odstranit if contains protoze jsem predelal List na HashSet takze nema duplicity
*/