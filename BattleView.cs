public static class BattleView
{
    private const int BarLength = 18;

    private static readonly string[] PlayerSprite =
    {
        "  ▄██▄",
        "▄██████▄",
        "███████●",
        " ██████▀",
        "  ████",
        "██    ██"
    };

    private static readonly string[] OpponentSprite =
    {
        "  ▄█ █▄",
        " ▄█████▄ ⌜/",
        "█●█●████╱",
        "███████▀",
        " ██ ██"
    };


    // Draws the complete battle screen
    public static void DrawBattleScreen(
        Digimon player,
        Digimon opponent,
        int round = 1)
    {
        Console.Clear();

        Console.WriteLine(
            "╔════════════════════════════════════════════════════════════════════════╗"
        );

        Console.WriteLine(
            "║                         DIGIMON BATTLER                                ║"
        );

        Console.WriteLine(
            "╠════════════════════════════════════════════════════════════════════════╣"
        );

        DrawCombatants(player, opponent);

        Console.WriteLine(
            "╠════════════════════════════════════════════════════════════════════════╣"
        );

        Console.WriteLine(
            $"║                         ROUND {round:00}                                       ║"
        );

        Console.WriteLine(
            "╠════════════════════════════════════════════════════════════════════════╣"
        );

        Console.WriteLine(
            "║                                                                        ║"
        );

        Console.WriteLine(
            "║   > ATTACK                                                             ║"
        );

        Console.WriteLine(
            "║     INFO                                                               ║"
        );

        Console.WriteLine(
            "║     FLEE                                                               ║"
        );

        Console.WriteLine(
            "║                                                                        ║"
        );

        Console.WriteLine(
            "╠════════════════════════════════════════════════════════════════════════╣"
        );

        Console.WriteLine(
            "║ BATTLE LOG                                                             ║"
        );

        Console.WriteLine(
            $"║ {player.Name} is ready for battle.                                            ║"
        );

        Console.WriteLine(
            "╚════════════════════════════════════════════════════════════════════════╝"
        );
    }


    // Draws the two Digimon and their battle information
    private static void DrawCombatants(
        Digimon player,
        Digimon opponent)
    {
        Console.WriteLine(
            $"║             {player.Name,-15}                 {opponent.Name,-15}            ║"
        );

        Console.WriteLine(
            $"║             {player.Attribute,-15}                 {opponent.Attribute,-15}            ║"
        );

        // Opponent starts one row higher than the player
        for (int i = 0; i < OpponentSprite.Length; i++)
        {
            string playerSprite = "";

            if (i > 0)
            {
                playerSprite = PlayerSprite[i - 1];
            }

            Console.WriteLine(
                $"║             {playerSprite,-15}                 {OpponentSprite[i],-15}            ║"
            );
        }

        // Last row of the player sprite
        Console.WriteLine(
            $"║             {PlayerSprite[5],-15}                                            ║"
        );

        Console.WriteLine(
            "║                                                                        ║"
        );

        string playerHp =
            $"HP {CreateBar(player.CurrentHp, player.MaxHp)} {player.CurrentHp,4}/{player.MaxHp,-4}";

        string opponentHp =
            $"HP {CreateBar(opponent.CurrentHp, opponent.MaxHp)} {opponent.CurrentHp,4}/{opponent.MaxHp,-4}";

        string playerSp =
            $"SP {CreateBar(player.CurrentSp, player.MaxSp)} {player.CurrentSp,4}/{player.MaxSp,-4}";

        string opponentSp =
            $"SP {CreateBar(opponent.CurrentSp, opponent.MaxSp)} {opponent.CurrentSp,4}/{opponent.MaxSp,-4}";

        Console.WriteLine(
            $"║ {playerHp}       {opponentHp}  ║"
        );

        Console.WriteLine(
            $"║ {playerSp}       {opponentSp}  ║"
        );
    }


    // Creates a visual representation of a stat bar
    private static string CreateBar(
        int current,
        int maximum)
    {
        if (maximum <= 0)
        {
            return new string('░', BarLength);
        }

        int filled =
            current * BarLength / maximum;

        filled = Math.Clamp(
            filled,
            0,
            BarLength
        );

        return new string('█', filled)
             + new string(
                 '░',
                 BarLength - filled
             );
    }
}