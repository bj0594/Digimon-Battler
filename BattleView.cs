public static class BattleView
{
    // Method to draw the battle screen with the player and opponent Digimon
    public static void DrawBattleScreen(Digimon player, Digimon opponent)
    {
        Console.Clear();

        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                         DIGIMON BATTLER                             ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════════════════════╣");

        Console.WriteLine("║                                                                      ║");
        Console.WriteLine($"║  {player.Name,-20}                     {opponent.Name,-20} ║");
        Console.WriteLine($"║  {player.Attribute,-20}                     {opponent.Attribute,-20} ║");
        Console.WriteLine("║                                                                      ║");

        Console.WriteLine("║                                                                      ║");
        Console.WriteLine("║                                                                      ║");

        Console.WriteLine($"║  HP  {player.CurrentHp}/{player.MaxHp,-10}                 {opponent.CurrentHp}/{opponent.MaxHp,-10} ║");
        Console.WriteLine($"║  SP  {player.CurrentSp}/{player.MaxSp,-10}                 {opponent.CurrentSp}/{opponent.MaxSp,-10} ║");

        Console.WriteLine("║                                                                      ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║                         ROUND 01 — AGUMON                            ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║                                                                      ║");
        Console.WriteLine("║  > MOVES                                                             ║");
        Console.WriteLine("║    INFO                                                              ║");
        Console.WriteLine("║    FLEE                                                              ║");
        Console.WriteLine("║                                                                      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
    }
}