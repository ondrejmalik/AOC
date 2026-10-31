using System.Diagnostics;

namespace AdventOfCode.Year2015;

public class Day22 : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 22;
    private static int _minCountTick = int.MaxValue;
    private static int _minCountMana = int.MaxValue;


    enum SpellType
    {
        Shield,
        Poison,
        Recharge
    }

    struct Spell(SpellType spellType, int duration)
    {
        public SpellType SpellType { get; set; } = spellType;
        public int Duration { get; set; } = duration;
    }


    public class Player
    {
        public int Hp { get; set; }
        public bool HasShield { get; set; }
        public int Mana { get; set; }
    }


    class Boss
    {
        public int Hp { get; set; }
        public int Attack { get; set; }
    }


    bool SimulateTickRecursive(Player player, Boss boss, List<Spell> spells, int tick, int manaSpent)
    {
        //apply
        player.HasShield = false;
        for (int i = 0; i < spells.Count; i++)
        {
            Spell spell = spells[i];
            switch (spell.SpellType)
            {
                case SpellType.Shield:
                    player.HasShield = true;
                    break;
                case SpellType.Poison:
                    boss.Hp -= 3;
                    break;
                case SpellType.Recharge:
                    player.Mana += 101;
                    break;
            }

            if (boss.Hp <= 0)
            {
                if (manaSpent <= _minCountMana)
                {
                    _minCountMana = manaSpent;
                    _minCountTick = tick;
                }

                return true;
            }

            spell.Duration -= 1;
            spells[i] = spell;
        }

        spells.RemoveAll(spell => spell.Duration == 0);


        //players turn
        if (tick % 2 == 0)
        {
            for (int spellIndex = 0; spellIndex < 5; spellIndex++)
            {
                Player newPlayer = new Player()
                {
                    Hp = player.Hp,
                    Mana = player.Mana,
                    HasShield = player.HasShield
                };

                Boss newBoss = new Boss()
                {
                    Hp = boss.Hp,
                    Attack = boss.Attack,
                };

                List<Spell> newSpells = [.. spells];

                int newManaSpent = manaSpent;

                switch (spellIndex)
                {
                    case 0: //shield
                        if (newPlayer.Mana >= 113 &&
                            !newSpells.Where((tuple, _) => tuple.SpellType == SpellType.Shield).Any())
                        {
                            newSpells.Add(new Spell(SpellType.Shield, 6));

                            newManaSpent += 113;
                            newPlayer.Mana -= 113;
                        }
                        else
                            continue;

                        break;
                    case 1: // poison
                        if (newPlayer.Mana >= 173 &&
                            !newSpells.Where((tuple, _) => tuple.SpellType == SpellType.Poison).Any())
                        {
                            newSpells.Add(new Spell(SpellType.Poison, 6));

                            newManaSpent += 173;
                            newPlayer.Mana -= 173;
                        }
                        else
                            continue;

                        break;
                    case 2: // recharge
                        if (newPlayer.Mana >= 229 &&
                            !newSpells.Where((tuple, _) => tuple.SpellType == SpellType.Recharge).Any())
                        {
                            newSpells.Add(new Spell(SpellType.Recharge, 5));

                            newManaSpent += 229;
                            newPlayer.Mana -= 229;
                        }
                        else
                            continue;

                        break;
                    case 3: //magic missile 
                        if (newPlayer.Mana >= 53)
                        {
                            newBoss.Hp -= 4;

                            newManaSpent += 53;
                            newPlayer.Mana -= 53;
                        }
                        else
                            continue;

                        break;
                    case 4: // drain
                        if (newPlayer.Mana >= 73)
                        {
                            newBoss.Hp -= 2;
                            newPlayer.Hp += 2;

                            newManaSpent += 73;
                            newPlayer.Mana -= 73;
                        }
                        else
                            continue;

                        break;
                }

                if (newBoss.Hp <= 0)
                {
                    if (newManaSpent <= _minCountMana)
                    {
                        _minCountMana = newManaSpent;
                        _minCountTick = tick;
                    }

                    return true;
                }

                bool killedBoss = SimulateTickRecursive(newPlayer, newBoss, newSpells, tick + 1, newManaSpent);
            }
        }

        //Bosses turn
        else
        {
            if (player.HasShield)
                player.Hp -= Math.Max(1, boss.Attack - 7);
            else
                player.Hp -= Math.Max(1, boss.Attack);

            if (player.Hp <= 0)
                return false;
            bool killedBoss = SimulateTickRecursive(player, boss, spells, tick + 1, manaSpent);
        }

        return false;
    }


    bool SimulateTickRecursiveHardMode(Player player, Boss boss, List<Spell> spells, int tick, int manaSpent)
    {
        //apply
        player.HasShield = false;
        for (int i = 0; i < spells.Count; i++)
        {
            Spell spell = spells[i];
            switch (spell.SpellType)
            {
                case SpellType.Shield:
                    player.HasShield = true;
                    break;
                case SpellType.Poison:
                    boss.Hp -= 3;
                    break;
                case SpellType.Recharge:
                    player.Mana += 101;
                    break;
            }

            if (boss.Hp <= 0)
            {
                if (manaSpent <= _minCountMana)
                {
                    _minCountMana = manaSpent;
                    _minCountTick = tick;
                }

                return true;
            }

            spell.Duration -= 1;
            spells[i] = spell;
        }

        spells.RemoveAll(spell => spell.Duration == 0);


        //players turn
        if (tick % 2 == 0)
        {
            //start of player turn - lower their HP
            player.Hp -= 1;
            if (player.Hp <= 0)
                return false;
            
            for (int spellIndex = 0; spellIndex < 5; spellIndex++)
            {
                Player newPlayer = new Player()
                {
                    Hp = player.Hp,
                    Mana = player.Mana,
                    HasShield = player.HasShield
                };

                Boss newBoss = new Boss()
                {
                    Hp = boss.Hp,
                    Attack = boss.Attack,
                };

                List<Spell> newSpells = [.. spells];

                int newManaSpent = manaSpent;

                switch (spellIndex)
                {
                    case 0: //shield
                        if (newPlayer.Mana >= 113 &&
                            !newSpells.Where((tuple, _) => tuple.SpellType == SpellType.Shield).Any())
                        {
                            newSpells.Add(new Spell(SpellType.Shield, 6));

                            newManaSpent += 113;
                            newPlayer.Mana -= 113;
                        }
                        else
                            continue;

                        break;
                    case 1: // poison
                        if (newPlayer.Mana >= 173 &&
                            !newSpells.Where((tuple, _) => tuple.SpellType == SpellType.Poison).Any())
                        {
                            newSpells.Add(new Spell(SpellType.Poison, 6));

                            newManaSpent += 173;
                            newPlayer.Mana -= 173;
                        }
                        else
                            continue;

                        break;
                    case 2: // recharge
                        if (newPlayer.Mana >= 229 &&
                            !newSpells.Where((tuple, _) => tuple.SpellType == SpellType.Recharge).Any())
                        {
                            newSpells.Add(new Spell(SpellType.Recharge, 5));

                            newManaSpent += 229;
                            newPlayer.Mana -= 229;
                        }
                        else
                            continue;

                        break;
                    case 3: //magic missile 
                        if (newPlayer.Mana >= 53)
                        {
                            newBoss.Hp -= 4;

                            newManaSpent += 53;
                            newPlayer.Mana -= 53;
                        }
                        else
                            continue;

                        break;
                    case 4: // drain
                        if (newPlayer.Mana >= 73)
                        {
                            newBoss.Hp -= 2;
                            newPlayer.Hp += 2;

                            newManaSpent += 73;
                            newPlayer.Mana -= 73;
                        }
                        else
                            continue;

                        break;
                }

                if (newBoss.Hp <= 0)
                {
                    if (newManaSpent <= _minCountMana)
                    {
                        _minCountMana = newManaSpent;
                        _minCountTick = tick;
                    }

                    return true;
                }

                bool killedBoss = SimulateTickRecursiveHardMode(newPlayer, newBoss, newSpells, tick + 1, newManaSpent);
            }
        }

        //Bosses turn
        else
        {
            if (player.HasShield)
                player.Hp -= Math.Max(1, boss.Attack - 7);
            else
                player.Hp -= Math.Max(1, boss.Attack);

            if (player.Hp <= 0)
                return false;
            bool killedBoss = SimulateTickRecursiveHardMode(player, boss, spells, tick + 1, manaSpent);
        }

        return false;
    }

    public override string SolvePart1(string[] input)
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        Player player = new()
        {
            Hp = 50,
            HasShield = false,
            Mana = 500,
        };

        Boss boss = new()
        {
            Hp = int.Parse(input[^2].Split(' ')[2]),
            Attack = int.Parse(input[^1].Split(' ')[1]),
        };

        //SimulateTickRecursive(player, boss, [], 0, 0);
        Console.WriteLine(_minCountMana);
        Console.WriteLine(_minCountTick);

        Console.WriteLine(s.Elapsed);
        return "".ToString();
    }

    public override string SolvePart2(string[] input)
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        Player player = new()
        {
            Hp = 50,
            HasShield = false,
            Mana = 500,
        };

        Boss boss = new()
        {
            Hp = int.Parse(input[^2].Split(' ')[2]),
            Attack = int.Parse(input[^1].Split(' ')[1]),
        };

        SimulateTickRecursiveHardMode(player, boss, [], 0, 0);
        Console.WriteLine(_minCountMana);
        Console.WriteLine(_minCountTick);

        Console.WriteLine(s.Elapsed);
        return "".ToString();
    }
}
// nejdriv jsem to chtel delat pres interface/abstract class ale to jsem si uvedomil ze je antipattern protoze bych checkoval child class u kazdy iterace a v OOP by melo platit ze caller by nemel znak konkretni implementaci
// pak jsem to zmenil na Tupple (SpellType,int duration) to se ale taky ze zacatku neosvedcilo protoze jsem je nemel defaultne pojmenovany a zaroven me prekvapilo ze nemuzu primo upravit promenou duration pres spells[i].duration -= 1 protoze je to value type a ne referencni
// takze jsem z tupple udelal tridu coz byla hrozna chyba. Naivne jsem si totiz mislel ze kdyz pak dam .ToList() na List<Spell> tak se z referencni tridy Spell stane zazrakem value type
// tohle zpusobilo bug ze i kdyz jsem chtel v kazdy iteraci udelat novy list se stejnou pocatecni hodnotou tak jsem v kazdy iteraci i kdyz v newSpells menil hodnoty spellu se stejnou referenci jako v spells listu takze se to do nej propsalo
// rikam si jak bych vlastne mel pojmenovavat promeny co chci aby byly kopie pro lokalni pouziti ve funkci a neprepisovaly argumenty primo i u referencnich typu. tady je pojmenovavam newNeco ale dela to hrozny bordel protoze intuitivne pouzivam ty normalni jmena
// dalsi blbost co jsem udelal byla ze kdyz vraci rekurze true jako ze zabila bosse tak jsem checknul jestli ho zabila a znova jsem se snazil prepsat minMana a minTick coz je samozdrejme blbost protoze v predchozim ticku byla mana treba nizsi.
// to jsem si neuvedomil z 2 duvovu.
// 1. v tom ifu ktery checkuje jestli byl boss zabit jsem pak vratil false takze se to nepropsalo az uplne nahoru a pak by me trklo ze se uklada 0 coz je uplna blbost.
// 2. v prikladu ze zadani na kterym jsem debugoval bosse zabil poison a v poslednim ticku player nepouzil manu takze v predchozim ticku byla mana stejna jako v tom poslednim