public class Battle
{
    // Properties to hold the Digimon participants in the battle
    public Digimon Player { get; set; }
    public Digimon Opponent { get; set; }

    // Properties to hold the battle state
    public int Round { get; set; }
    public bool IsFinished { get; set; }
    public Digimon Winner { get; set; }

    // Constructor to initialize the battle with the player and opponent Digimon
    public Battle(Digimon player, Digimon opponent)
    {
        Player = player;
        Opponent = opponent;
        Round = 1;
        IsFinished = false;
    }

    public void Attack(Digimon attacker, Digimon defender, Move move)
    {
        // Battle logic
    }
}