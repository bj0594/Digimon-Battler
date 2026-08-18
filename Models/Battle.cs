public class Battle
{
    // Digimon participating in the battle.
    public Digimon Player { get; set; }
    public Digimon Opponent { get; set; }

    // Current state of the battle.
    public int Round { get; set; }
    public bool IsFinished { get; set; }
    public Digimon? Winner { get; set; }


    // Creates a new battle between two Digimon.
    public Battle(
        Digimon player,
        Digimon opponent)
    {
        Player = player;
        Opponent = opponent;
        Round = 1;
        IsFinished = false;
        Winner = null;
    }


    // Performs an attack and returns the damage dealt.
    public int Attack(
        Digimon attacker,
        Digimon defender,
        Move move)
    {
        // A Move cannot be used without enough SP.
        if (attacker.CurrentSp < move.SpCost)
        {
            return 0;
        }

        // Pay the Move's SP cost.
        attacker.CurrentSp -= move.SpCost;

        // Physical Moves use Attack; Magic Moves use Intelligence.
        int attackPower =
            move.Type == "Physical"
                ? attacker.Attack
                : attacker.Intelligence;

        // Calculate damage, with a minimum of 1.
        int damage = Math.Max(
            1,
            attackPower + move.Power - defender.Defense
        );

        // Apply damage without allowing HP to drop below zero.
        defender.CurrentHp = Math.Max(
            0,
            defender.CurrentHp - damage
        );

        // End the battle if the defender was defeated.
        if (IsDefeated(defender))
        {
            IsFinished = true;
            Winner = attacker;
        }

        return damage;
    }


    // Returns true when a Digimon has no HP remaining.
    private bool IsDefeated(Digimon digimon)
    {
        return digimon.CurrentHp <= 0;
    }
}