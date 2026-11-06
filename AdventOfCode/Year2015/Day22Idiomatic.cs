namespace AdventOfCode.Year2015;

public class Day22Idiomatic : BaseSolver
{
    public override int Year => 2015;
    public override int Day => 22;

    public readonly record struct BattleState(
        int PlayerHp,
        int PlayerMana,
        int BossHp,
        int BossDamage,
        int ShieldTimer = 0,
        int PoisonTimer = 0,
        int RechargeTimer = 0,
        int ManaSpent = 0)
    {
        public bool IsBossDead => BossHp <= 0;

        // Pure transition: ticks all active effects and decrements timers
        public BattleState ApplyEffects() => this with
        {
            BossHp        = PoisonTimer > 0 ? BossHp - 3 : BossHp,
            PlayerMana    = RechargeTimer > 0 ? PlayerMana + 101 : PlayerMana,
            ShieldTimer   = Math.Max(0, ShieldTimer - 1),
            PoisonTimer   = Math.Max(0, PoisonTimer - 1),
            RechargeTimer = Math.Max(0, RechargeTimer - 1)
        };

        // Generates valid state transitions for all castable spells
        public IEnumerable<BattleState> GetNextStatesAfterCast()
        {
            // Magic Missile (53 mana, 4 damage)
            if (PlayerMana >= 53)
                yield return this with
                {
                    PlayerMana = PlayerMana - 53,
                    ManaSpent  = ManaSpent + 53,
                    BossHp     = BossHp - 4
                };

            // Drain (73 mana, 2 damage, +2 heal)
            if (PlayerMana >= 73)
                yield return this with
                {
                    PlayerMana = PlayerMana - 73,
                    ManaSpent  = ManaSpent + 73,
                    BossHp     = BossHp - 2,
                    PlayerHp   = PlayerHp + 2
                };

            // Shield (113 mana, 6 turns)
            if (PlayerMana >= 113 && ShieldTimer == 0)
                yield return this with
                {
                    PlayerMana  = PlayerMana - 113,
                    ManaSpent   = ManaSpent + 113,
                    ShieldTimer = 6
                };

            // Poison (173 mana, 6 turns)
            if (PlayerMana >= 173 && PoisonTimer == 0)
                yield return this with
                {
                    PlayerMana  = PlayerMana - 173,
                    ManaSpent   = ManaSpent + 173,
                    PoisonTimer = 6
                };

            // Recharge (229 mana, 5 turns)
            if (PlayerMana >= 229 && RechargeTimer == 0)
                yield return this with
                {
                    PlayerMana    = PlayerMana - 229,
                    ManaSpent     = ManaSpent + 229,
                    RechargeTimer = 5
                };
        }
    }

    public override string SolvePart1(string[] input) =>
        FindMinManaToWin(Parse(input), hardMode: false).ToString();

    public override string SolvePart2(string[] input) =>
        FindMinManaToWin(Parse(input), hardMode: true).ToString();

    private static int FindMinManaToWin(BattleState initialState, bool hardMode)
    {
        int bestMana = int.MaxValue;
        Search(initialState, isPlayerTurn: true);
        return bestMana;

        void Search(BattleState state, bool isPlayerTurn)
        {
            // 1. Branch-and-bound pruning: discard branch if already worse than best win
            if (state.ManaSpent >= bestMana)
                return;

            // 2. Part 2 Hard Mode penalty
            if (hardMode && isPlayerTurn)
            {
                state = state with { PlayerHp = state.PlayerHp - 1 };
                if (state.PlayerHp <= 0) return;
            }

            // 3. Shield armor check BEFORE timers tick down (1 -> 0 turn still gets armor)
            int armor = state.ShieldTimer > 0 ? 7 : 0;

            // 4. Tick start-of-turn effects
            state = state.ApplyEffects();
            if (state.IsBossDead)
            {
                bestMana = state.ManaSpent;
                return;
            }

            // 5. Execute action
            if (isPlayerTurn)
            {
                foreach (var nextState in state.GetNextStatesAfterCast())
                {
                    if (nextState.IsBossDead && nextState.ManaSpent < bestMana)
                        bestMana = nextState.ManaSpent;
                    else
                        Search(nextState, isPlayerTurn: false);
                }
            }
            else
            {
                // Boss attack
                int damage = Math.Max(1, state.BossDamage - armor);
                BattleState afterBossHit = state with { PlayerHp = state.PlayerHp - damage };

                if (afterBossHit.PlayerHp > 0)
                    Search(afterBossHit, isPlayerTurn: true);
            }
        }
    }

    private static BattleState Parse(string[] input)
    {
        int bossHp = int.Parse(input[^2].Split(' ')[^1]);
        int bossDmg = int.Parse(input[^1].Split(' ')[^1]);
        return new BattleState(PlayerHp: 50, PlayerMana: 500, BossHp: bossHp, BossDamage: bossDmg);
    }
}
/* IDIOMATIC
zmeneni z list<spell> na to ze spelly jsou definovany v battle state jako jeden readonly record struct
pridani if early return pruning to znacne zmensi runtime
               if (state.ManaSpent >= bestMana)
                return;
nevyhoda je ze vsechno je v jednom structu takze to neni moc rozsiritelny
*/
