public static class BattleView
{
    // Constants for the battle screen layout
    private const int InnerWidth = 72;
    private const int BarLength = 18;

    // Method to draw the entire battle screen
    public static void DrawBattleScreen(
        Digimon player,
        Digimon opponent,
        int roundNumber = 1)
    {
        Console.Clear();

        DrawHeader();
        DrawCombatants(player, opponent);
        DrawRound(roundNumber, player);
        DrawBattleMenu();
        DrawBattleLog(player);

        Console.WriteLine($"╚{new string('═', InnerWidth)}╝");
    }

    // Method to draw the header of the battle screen
    private static void DrawHeader()
    {
        Console.WriteLine($"╔{new string('═', InnerWidth)}╗");
        WriteCentered("DIGIMON BATTLER");
        Console.WriteLine($"╠{new string('═', InnerWidth)}╣");
    }

    // Method to draw the combatants (player and opponent) on the battle screen
    private static void DrawCombatants(Digimon player, Digimon opponent)
    {
        WriteEmptyLine();

        WriteRow(
            $"   {player.Name,-30}{opponent.Name,-30}"
        );

        WriteRow(
            $"   {player.Attribute,-30}{opponent.Attribute,-30}"
        );

        WriteEmptyLine();

        // Temporary placeholders for Digimon illustrations
        WriteRow(
            "          [ PLAYER ]                          [ OPPONENT ]"
        );

        WriteEmptyLine();

        DrawStatBars(player, opponent);

        WriteEmptyLine();

        Console.WriteLine($"╠{new string('═', InnerWidth)}╣");
    }

    // Method to draw the health and special points bars for both Digimon
    private static void DrawStatBars(Digimon player, Digimon opponent)
    {
        string playerHpBar = CreateBar(
            player.CurrentHp,
            player.MaxHp
        );

        string opponentHpBar = CreateBar(
            opponent.CurrentHp,
            opponent.MaxHp
        );

        string playerSpBar = CreateBar(
            player.CurrentSp,
            player.MaxSp
        );

        string opponentSpBar = CreateBar(
            opponent.CurrentSp,
            opponent.MaxSp
        );

        WriteRow(
            $"   HP {playerHpBar} {player.CurrentHp,4}/{player.MaxHp,-4}" +
            $"     HP {opponentHpBar} {opponent.CurrentHp,4}/{opponent.MaxHp,-4}"
        );

        WriteRow(
            $"   SP {playerSpBar} {player.CurrentSp,4}/{player.MaxSp,-4}" +
            $"     SP {opponentSpBar} {opponent.CurrentSp,4}/{opponent.MaxSp,-4}"
        );
    }

    // Method to create a visual representation of a stat bar based on current and maximum values
    private static string CreateBar(int current, int maximum)
    {
        if (maximum <= 0)
        {
            return new string('░', BarLength);
        }

        double percentage = (double)current / maximum;

        int filled = (int)(percentage * BarLength);

        filled = Math.Clamp(filled, 0, BarLength);

        return new string('█', filled)
             + new string('░', BarLength - filled);
    }


    private static void DrawRound(int roundNumber, Digimon player)
    {
        WriteCentered($"ROUND {roundNumber:00} — {player.Name}");

        Console.WriteLine($"╠{new string('═', InnerWidth)}╣");
    }

    // Method to draw the battle menu options
    private static void DrawBattleMenu()
    {
        WriteEmptyLine();

        WriteRow("   > ATTACK");
        WriteRow("     INFO");
        WriteRow("     FLEE");

        WriteEmptyLine();

        Console.WriteLine($"╠{new string('═', InnerWidth)}╣");
    }

    // Method to draw the battle log section of the screen
    private static void DrawBattleLog(Digimon player)
    {
        WriteRow(" BATTLE LOG");
        WriteRow($" {player.Name} is ready for battle.");
    }

    // Helper method to write text centered within the battle screen
    private static void WriteCentered(string text)
    {
        int padding = (InnerWidth - text.Length) / 2;

        WriteRow(
            new string(' ', padding) + text
        );
    }

    // Helper method to write an empty line within the battle screen
    private static void WriteEmptyLine()
    {
        WriteRow("");
    }

    // Helper method to write a row of text within the battle screen, ensuring it fits within the defined width
    private static void WriteRow(string content)
    {
        if (content.Length > InnerWidth)
        {
            content = content[..InnerWidth];
        }

        Console.WriteLine(
            $"║{content.PadRight(InnerWidth)}║"
        );
    }
}