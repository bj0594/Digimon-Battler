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
        return damage;
    }

    public bool IsDefeated(Digimon digimon)
    {
    return digimon.CurrentHp <= 0;
    }

   public string NextRound(
    Move playerMove,
    Move opponentMove)
{
    Attack(
        Player,
        Opponent,
        playerMove
    );

    if (IsDefeated(Opponent))
    {
        return "";
    }

    int damage = Attack(
        Opponent,
        Player,
        opponentMove
    );

    if (IsDefeated(Player))
    {
        return $"{Opponent.Name} used {opponentMove.Name}! " +
               $"{Player.Name} took {damage} damage!";
    }

    Round++;

    return $"{Opponent.Name} used {opponentMove.Name}! " +
           $"{Player.Name} took {damage} damage!";
}
}