using AdventOfCode;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015;

public partial class Day21Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 21;

    public readonly record struct ShopItem(int Cost, int Attack, int Armor)
    {
        public static ShopItem operator +(ShopItem a, ShopItem b) =>
            new(a.Cost + b.Cost, a.Attack + b.Attack, a.Armor + b.Armor);
    }

    public readonly record struct Fighter(int Hp, int Attack, int Armor)
    {
        public Fighter Equip(ShopItem gear) =>
            this with { Attack = Attack + gear.Attack, Armor = Armor + gear.Armor };
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberRegex();

    public override string SolvePart1(string[] input) =>
        EvaluateLoadouts(input)
            .Where(x => x.PlayerWins)
            .Min(x => x.Cost)
            .ToString();

    public override string SolvePart2(string[] input) =>
        EvaluateLoadouts(input)
            .Where(x => !x.PlayerWins)
            .Max(x => x.Cost)
            .ToString();

    private static IEnumerable<(int Cost, bool PlayerWins)> EvaluateLoadouts(string[] input)
    {
        var (weapons, armors, rings, boss) = Parse(input);
        var basePlayer = new Fighter(Hp: 100, Attack: 0, Armor: 0);
        foreach (var w in weapons)
        foreach (var a in armors)
            for (int r1 = 0; r1 < rings.Count; r1++)
            for (int r2 = r1; r2 < rings.Count; r2++) // Start at r1 to avoid duplicate ring permutations
            {
                if (r1 == r2 && r1 != 0) continue; // Only index 0 ("no ring") can be picked twice
                ShopItem totalGear = w + a + rings[r1] + rings[r2];
                Fighter equippedPlayer = basePlayer.Equip(totalGear);
                yield return (totalGear.Cost, PlayerWins(equippedPlayer, boss));
            }
    }

    private static bool PlayerWins(Fighter player, Fighter boss)
    {
        int playerDmg = Math.Max(1, player.Attack - boss.Armor);
        int bossDmg = Math.Max(1, boss.Attack - player.Armor);
        int turnsToKillBoss = (boss.Hp + playerDmg - 1) / playerDmg;
        int turnsToKillPlayer = (player.Hp + bossDmg - 1) / bossDmg;
        return turnsToKillBoss <= turnsToKillPlayer;
    }

    private static (List<ShopItem> Weapons, List<ShopItem> Armors, List<ShopItem> Rings, Fighter Boss) Parse(
        string[] input)
    {
        List<ShopItem> weapons = [];
        List<ShopItem> armors = [new(0, 0, 0)]; // 0 armor option (dummy)
        List<ShopItem> rings = [new(0, 0, 0)]; // 0 ring option (dummy)
        int section = 0;
        for (int i = 1; i < input.Length - 4; i++)
        {
            if (string.IsNullOrWhiteSpace(input[i]))
            {
                section++;
                i++; // Skip the next section header
                continue;
            }

            var nums = NumberRegex().Matches(input[i]).Select(m => int.Parse(m.ValueSpan)).ToArray();
            // Rings have "+1", "+2" in their names; Cost, Damage, and Armor are always the last 3 numbers:
            ShopItem item = new(nums[^3], nums[^2], nums[^1]);
            switch (section)
            {
                case 0: weapons.Add(item); break;
                case 1: armors.Add(item); break;
                case 2: rings.Add(item); break;
            }
        }

        Fighter boss = new(
            Hp: int.Parse(input[^3].Split(": ")[1]),
            Attack: int.Parse(input[^2].Split(": ")[1]),
            Armor: int.Parse(input[^1].Split(": ")[1])
        );
        return (weapons, armors, rings, boss);
    }
}