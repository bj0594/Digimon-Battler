public static partial class BattleView
{
    private const int InfoColumnWidth = 36;


    // Displays information about both Digimon.
    public static void ShowInfoScreen(
        Digimon player,
        Digimon opponent)
    {
        Console.Clear();

        DrawHeader();

        WriteRow("");
        WriteRow("DIGIMON INFO", true);
        WriteRow("");

        WriteInfoRow(
            player.Name,
            opponent.Name
        );

        WriteInfoRow(
            player.Attribute,
            opponent.Attribute
        );

        WriteRow("");

        WriteInfoStatRow(
            "HP",
            $"{player.CurrentHp}/{player.MaxHp}",
            $"{opponent.CurrentHp}/{opponent.MaxHp}"
        );

        WriteInfoStatRow(
            "SP",
            $"{player.CurrentSp}/{player.MaxSp}",
            $"{opponent.CurrentSp}/{opponent.MaxSp}"
        );

        WriteRow("");

        WriteInfoStatRow(
            "Attack",
            player.Attack.ToString(),
            opponent.Attack.ToString()
        );

        WriteInfoStatRow(
            "Defense",
            player.Defense.ToString(),
            opponent.Defense.ToString()
        );

        WriteInfoStatRow(
            "Intelligence",
            player.Intelligence.ToString(),
            opponent.Intelligence.ToString()
        );

        WriteInfoStatRow(
            "Speed",
            player.Speed.ToString(),
            opponent.Speed.ToString()
        );

        WriteRow("");

        DrawSeparator();
        WriteRow("Press ESC to return.", true);
        DrawBottomBorder();

        // Ignore all keys except ESC.
        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
        }
    }


    // Writes text for both Digimon in aligned columns.
    private static void WriteInfoRow(
        string playerText,
        string opponentText)
    {
        WriteRow(
            $"    {playerText,-InfoColumnWidth}{opponentText}"
        );
    }


    // Writes a labelled stat for both Digimon.
    private static void WriteInfoStatRow(
        string label,
        string playerValue,
        string opponentValue)
    {
        WriteInfoRow(
            $"{label,-14}{playerValue}",
            $"{label,-14}{opponentValue}"
        );
    }
}