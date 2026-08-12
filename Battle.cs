public class Battle
{
    public Digimon Player { get; set; }
    public Digimon Opponent { get; set; }

    public Battle(Digimon player, Digimon opponent)
    {
        Player = player;
        Opponent = opponent;
    }

    public void Attack(Digimon attacker, Digimon defender, Move move)
    {
        // Battle logic
    }
}