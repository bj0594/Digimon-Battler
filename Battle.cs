public class Battle
{
    // Digimon participating in the battle
    public Digimon Player { get; set; }
    public Digimon Opponent { get; set; }

    // Current battle state
    public int Round { get; set; }
    public bool IsFinished { get; set; }
    public Digimon? Winner { get; set; }


    // Creates a new battle
    public Battle(Digimon player, Digimon opponent)
    {
        Player = player;
        Opponent = opponent;
        Round = 1;
        IsFinished = false;
        Winner = null;
    }


    // Makes one Digimon attack another Digimon
    public void Attack(
        Digimon attacker,
        Digimon defender,
        Move move)
    {
        // The attacker cannot use a move without enough SP
        if (attacker.CurrentSp < move.SpCost)
        {
            return;
        }

        // Spend SP
        attacker.CurrentSp -= move.SpCost;

        // Calculate damage
        int attackPower;

        if (move.Type == "Physical")
        {
            attackPower = attacker.Attack;
        }
        else
        {
            attackPower = attacker.Intelligence;
        }

        int damage = attackPower + move.Power - defender.Defense;

        // Always deal at least 1 damage
        damage = Math.Max(1, damage);

        // Apply damage
        defender.CurrentHp -= damage;

        // Prevent HP from going below zero
        defender.CurrentHp = Math.Max(0, defender.CurrentHp);

        // Check if the defender has been defeated
        if (defender.CurrentHp <= 0)
        {
            IsFinished = true;
            Winner = attacker;
        }
    }

    public bool IsDefeated(Digimon digimon)
    {
    return digimon.CurrentHp <= 0;
    }

    public void NextRound(Move playerMove, Move opponentMove)
{
    // Player attacks first
    Attack(Player, Opponent, playerMove);

    // Stop if the opponent was defeated
    if (IsDefeated(Opponent))
    {
        return;
    }

    // Opponent attacks
    Attack(Opponent, Player, opponentMove);

    // Stop if the player was defeated
    if (IsDefeated(Player))
    {
        return;
    }

    Round++;
}
}