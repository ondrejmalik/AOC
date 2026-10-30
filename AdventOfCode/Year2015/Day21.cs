using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015;

public class Day21 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 21;

    class ShopItem
    {
        public int Cost { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
    }

    class Player
    {
        public int HP { get; set; }
        public int Defence { get; set; }
        public int Attack { get; set; }
    }

    class Boss : Player;

    void AddToShopt(List<ShopItem> shop, string line, Regex regex, int skips)
    {
        ShopItem item = new ShopItem();
        var matchEnumerator = regex.EnumerateMatches(line);

        for (int i = 0; i < skips; i++)
            matchEnumerator.MoveNext();

        matchEnumerator.MoveNext();
        ValueMatch valueMatch = matchEnumerator.Current;

        item.Cost = Int32.Parse(line[valueMatch.Index..(valueMatch.Index + valueMatch.Length)]);

        matchEnumerator.MoveNext();
        valueMatch = matchEnumerator.Current;

        item.Attack = Int32.Parse(line[valueMatch.Index..(valueMatch.Index + valueMatch.Length)]);
        matchEnumerator.MoveNext();

        valueMatch = matchEnumerator.Current;
        item.Defence = Int32.Parse(line[valueMatch.Index..(valueMatch.Index + valueMatch.Length)]);
        shop.Add(item);
    }

    int FindHighestCost(Player player, Boss boss, List<ShopItem> weaponShop, List<ShopItem> armorShop,
        List<ShopItem> ringShop)
    {
        int maxCost = 0;
        for (int weaponIndex = 0; weaponIndex < weaponShop.Count; weaponIndex++)
        {
            ShopItem weapon = weaponShop[weaponIndex];
            for (int armorIndex = 0; armorIndex < armorShop.Count; armorIndex++)
            {
                ShopItem armor = armorShop[armorIndex];

                ShopItem ring1 = null!, ring2 = null!;

                for (int ring1Index = 0; ring1Index < ringShop.Count; ring1Index++)
                {
                    ring1 = ringShop[ring1Index];


                    for (int ring2Index = 0; ring2Index < ringShop.Count; ring2Index++)
                    {
                        //case when both rings are the same and also they are not empty ring
                        if (ring1Index == ring2Index && ring1Index != 0)
                        {
                            continue;
                        }

                        ring2 = ringShop[ring2Index];

                        int totalBuildCost = weapon.Cost + armor.Cost + ring1.Cost + ring2.Cost;

                        if (!SimulateFight(player, boss, weapon, armor, ring1, ring2))
                        {
                            maxCost = Math.Max(maxCost, totalBuildCost);
                        }
                    }
                }
            }
        }


        return maxCost;
    }

    int FindLowestCostWin(Player player, Boss boss, List<ShopItem> weaponShop, List<ShopItem> armorShop,
        List<ShopItem> ringShop)
    {
        int minCost = Int32.MaxValue;
        for (int weaponIndex = 0; weaponIndex < weaponShop.Count; weaponIndex++)
        {
            ShopItem weapon = weaponShop[weaponIndex];
            for (int armorIndex = 0; armorIndex < armorShop.Count; armorIndex++)
            {
                ShopItem armor = armorShop[armorIndex];

                ShopItem ring1 = null!, ring2 = null!;

                for (int ring1Index = 0; ring1Index < ringShop.Count; ring1Index++)
                {
                    ring1 = ringShop[ring1Index];


                    for (int ring2Index = 0; ring2Index < ringShop.Count; ring2Index++)
                    {
                        //case when both rings are the same and also they are not empty ring
                        if (ring1Index == ring2Index && ring1Index != 0)
                        {
                            continue;
                        }

                        ring2 = ringShop[ring2Index];

                        int totalBuildCost = weapon.Cost + armor.Cost + ring1.Cost + ring2.Cost;

                        if (SimulateFight(player, boss, weapon, armor, ring1, ring2))
                        {
                            minCost = Math.Min(minCost, totalBuildCost);
                            if (minCost == 121)
                            {
                                SimulateFight(player, boss, weapon, armor, ring1, ring2);
                            }
                        }
                    }
                }
            }
        }


        return minCost;
    }

    Player SetPlayerStats(Player player, ShopItem weapon, ShopItem armor, ShopItem ring1, ShopItem ring2)
    {
        Player newPlayer = new Player();
        newPlayer.HP = player.HP;
        newPlayer.Attack += weapon.Attack;
        newPlayer.Defence += armor.Defence;

        newPlayer.Attack += ring1.Attack;
        newPlayer.Defence += ring1.Defence;

        newPlayer.Attack += ring2.Attack;
        newPlayer.Defence += ring2.Defence;
        return newPlayer;
    }

    bool SimulateFight(Player player, Boss boss, ShopItem weapon, ShopItem armor, ShopItem ring1, ShopItem ring2)
    {
        Player newPlayer = SetPlayerStats(player, weapon, armor, ring1, ring2);

        Boss newBoss = new Boss()
        {
            HP = boss.HP,
            Attack = boss.Attack,
            Defence = boss.Defence
        };
        int tick = 0;
        while (true)
        {
            //players turn
            if (tick % 2 == 0)
            {
                newBoss.HP -= Math.Max(1, newPlayer.Attack - newBoss.Defence);

                if (newBoss.HP <= 0)
                    return true;
            }
            //Bosses turn
            else
            {
                newPlayer.HP -= Math.Max(1, newBoss.Attack - newPlayer.Defence);

                if (newPlayer.HP <= 0)
                    return false;
            }

            tick += 1;
            //Console.WriteLine($"player {newPlayer.HP}");
            //Console.WriteLine($"Boss {newBoss.HP}");
        }

        return false;
    }

    public override string SolvePart1(string[] input)
    {
        Player player = new Player()
        {
            HP = 100,
            Attack = 0,
            Defence = 0,
        };

        Boss boss = new Boss()
        {
            HP = Int32.Parse(input[^3].Split(' ')[2]),
            Attack = Int32.Parse(input[^2].Split(' ')[1]),
            Defence = Int32.Parse(input[^1].Split(' ')[1]),
        };

        List<ShopItem> weaponShop = new List<ShopItem>();
        List<ShopItem> armorShop = new List<ShopItem>();
        List<ShopItem> ringShop = new List<ShopItem>();

        armorShop.Add(new ShopItem
        {
            Attack = 0,
            Cost = 0,
            Defence = 0
        });

        ringShop.Add(new ShopItem
        {
            Attack = 0,
            Cost = 0,
            Defence = 0
        });

        Regex regex = new Regex("\\d+");
        int shop = 0;
        for (int i = 1; i < input.Length - 4; i++)
        {
            string line = input[i];
            if (line == "")
            {
                shop++;
                i++;
                continue;
            }

            switch (shop)
            {
                case 0:
                    AddToShopt(weaponShop, line, regex, 0);
                    break;
                case 1:
                    AddToShopt(armorShop, line, regex, 0);
                    break;
                case 2:
                    AddToShopt(ringShop, line, regex, 1);
                    break;
            }
        }

        int lowestCost = FindLowestCostWin(player, boss, weaponShop, armorShop, ringShop);
        return lowestCost.ToString();
    }


    public override string SolvePart2(string[] input)
    {
        Player player = new Player()
        {
            HP = 100,
            Attack = 0,
            Defence = 0,
        };

        Boss boss = new Boss()
        {
            HP = Int32.Parse(input[^3].Split(' ')[2]),
            Attack = Int32.Parse(input[^2].Split(' ')[1]),
            Defence = Int32.Parse(input[^1].Split(' ')[1]),
        };

        List<ShopItem> weaponShop = new List<ShopItem>();
        List<ShopItem> armorShop = new List<ShopItem>();
        List<ShopItem> ringShop = new List<ShopItem>();

        armorShop.Add(new ShopItem
        {
            Attack = 0,
            Cost = 0,
            Defence = 0
        });

        ringShop.Add(new ShopItem
        {
            Attack = 0,
            Cost = 0,
            Defence = 0
        });

        Regex regex = new Regex("\\d+");
        int shop = 0;
        for (int i = 1; i < input.Length - 4; i++)
        {
            string line = input[i];
            if (line == "")
            {
                shop++;
                i++;
                continue;
            }

            switch (shop)
            {
                case 0:
                    AddToShopt(weaponShop, line, regex, 0);
                    break;
                case 1:
                    AddToShopt(armorShop, line, regex, 0);
                    break;
                case 2:
                    AddToShopt(ringShop, line, regex, 1);
                    break;
            }
        }

        int highestCost = FindHighestCost(player, boss, weaponShop, armorShop, ringShop);
        return highestCost.ToString();
    }
}
// v parsovani me zmatly prazdne radky a nadpisy tak ze jsem musel preskocit 2 radky a pak zmenit list do ktereho vkladam. takze misto foreach jsem musel pouzit .MoveNext()
// potom u rings me zmatlo +1+2.. atd  kde jsem musel preskocit prvni match
// potom se do Listu vkladaly divny inty i tak bylo to protoze jsem musel za \d dat + aby se cely ten substring matchnul jako jeden match
// Simulace souboje je jenom stridani se pres i%2 ==0 zacina hrac a kdyz bossovi nebo hraci dojdou zivoty vraci true kdyz vyhraje hrac kdyz nevyhraje vraci false
// V simulaci byl problem s jednou veci a to je ze se musely vytvorit kopie player a boss aby se nepropsala hodnota do dalsi simulace s jinymi predmety takzy bylo potreba udelat kopii. Říkám si jestli by to kopírování nešlo nějak snáž než vypisovat všechny prvky. treba u stringu je to pres ToString()
// hledani jsem se rozhodl bruteforcenout pres vyzkouseni vsech kombinaci i kdyz bych nemusel. stacilo by je seradit a dokud by hrac vyhraval tak snizovat cost.
// u prstenu protoze muzou byt 0 1 nebo 2 tak me napadlo vlozit do listu dummy prseny a hledat variace. Na ty ale neni built in funkce a tak jsem se zeptal gemini jestli je to podvadeni rikal ze ne ale ze variace vubec nepotrebuju protoze staci 2 loopy.