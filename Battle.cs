public class Battle
{
    // Digimon participating in the battle
    public Digimon Player { get; set; }
    public Digimon Opponent { get; set; }

    // Current battle state
    public int Round { get; set; }
    public bool IsFinished { get; set; }
    public Digimon? Winner { get; set; }


    // Creates a new battle between two Digimon
    public Battle(Digimon player, Digimon opponent)
    {
        Player = player;
        Opponent = opponent;
        Round = 1;
        IsFinished = false;
        Winner = null;
    }


    // Makes one Digimon attack another Digimon
    public int Attack(
        Digimon attacker,
        Digimon defender,
        Move move)
    {
        // The attacker cannot use a move without enough SP
        if (attacker.CurrentSp < move.SpCost)
        {
            return 0;
        }

        // Spend SP for the move
        attacker.CurrentSp -= move.SpCost;

        // Physical moves use Attack, other moves use Intelligence
        int attackPower =
            move.Type == "Physical"
                ? attacker.Attack
                : attacker.Intelligence;

        // Calculate and apply damage
        int damage = Math.Max(
            1,
            attackPower + move.Power - defender.Defense
        );

        defender.CurrentHp = Math.Max(
            0,
            defender.CurrentHp - damage
        );

        // Check whether the defender was defeated
        if (IsDefeated(defender))
        {
            IsFinished = true;
            Winner = attacker;
        }

        return damage;
    }


    // Checks whether a Digimon has been defeated
    public bool IsDefeated(Digimon digimon)
    {
        return digimon.CurrentHp <= 0;
    }
}