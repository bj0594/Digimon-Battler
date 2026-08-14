public static partial class BattleView
{
    private const int InnerWidth = 72;
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
        int round,
        string battleLog,
        int selectedAction = 0)
    {
        Console.Clear();

        DrawHeader();
        DrawCombatants(player, opponent);

        DrawSeparator();
        WriteRow($"ROUND {round:00}", true);
        DrawSeparator();

        WriteRow("");

        for (int i = 0; i < Actions.Length; i++)
        {
            string marker =
                i == selectedAction
                    ? ">"
                    : " ";

            WriteRow(
                $"   {marker} {Actions[i]}"
            );
        }

        WriteRow("");
        DrawBottomBorder();
    }


    // Draws the common battle screen header
    private static void DrawHeader()
    {
        DrawTopBorder();
        WriteRow("DIGIMON BATTLER", true);
        DrawSeparator();
    }


    // Draws the result of an attack
    public static void ShowBattleResult(
        Digimon player,
        Digimon opponent,
        int round,
        string battleLog)
    {
        Console.Clear();

        DrawHeader();
        DrawCombatants(player, opponent);

        DrawSeparator();
        WriteRow($"ROUND {round:00}", true);
        DrawSeparator();

        WriteRow("");
        WriteRow(battleLog);
        WriteRow("");

        DrawSeparator();
        WriteRow(
            "Press ENTER to continue.",
            true
        );

        DrawBottomBorder();

        Console.ReadKey(true);
    }


    // Draws the Move selection screen
    private static void DrawMoveScreen(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
        string battleLog,
        int selected)
    {
        Console.Clear();

        DrawHeader();
        DrawCombatants(player, opponent);

        DrawSeparator();
        WriteRow($"ROUND {round:00}", true);
        DrawSeparator();

        WriteRow("   CHOOSE MOVE");

        // Display Moves in two columns
        for (int row = 0; row < 3; row++)
        {
            string left =
                GetMoveText(
                    moves,
                    digimon,
                    row,
                    selected
                );

            string right =
                GetMoveText(
                    moves,
                    digimon,
                    row + 3,
                    selected
                );

            WriteRow(
                $"   {left,-31}{right}"
            );
        }

        WriteRow("   Press ESC to return");

        DrawBottomBorder();
    }


    // Creates the display text for one Move
    private static string GetMoveText(
        List<Move> moves,
        Digimon digimon,
        int index,
        int selected)
    {
        if (index >= moves.Count)
        {
            return "";
        }

        Move move = moves[index];

        string marker =
            selected == index
                ? ">"
                : " ";

        string cost =
            move.SpCost <= digimon.CurrentSp
                ? $"{move.SpCost} SP"
                : "NO SP";

        return $"{marker} {move.Name,-20} {cost}";
    }


    // Draws the two Digimon and their battle information
    private static void DrawCombatants(
        Digimon player,
        Digimon opponent,
        Digimon? damagedDigimon = null,
        bool showHit = false)
    {
        string[] playerSprite =
            damagedDigimon == player && showHit
                ? GetHitSprite(PlayerSprite)
                : PlayerSprite;

        string[] opponentSprite =
            damagedDigimon == opponent && showHit
                ? GetHitSprite(OpponentSprite)
                : OpponentSprite;

        WriteRow(
            $"             {player.Name,-15}                 {opponent.Name,-15}"
        );

        WriteRow(
            $"             {player.Attribute,-15}                 {opponent.Attribute,-15}"
        );

        // Draw the opponent one row higher than the player
        for (int i = 0; i < opponentSprite.Length; i++)
        {
            string playerLine =
                i > 0
                    ? playerSprite[i - 1]
                    : "";

            WriteRow(
                $"             {playerLine,-15}                 {opponentSprite[i],-15}"
            );
        }

        // Draw the final row of the player sprite
        WriteRow(
            $"             {playerSprite[5],-15}"
        );

        WriteRow("");

        WriteRow(
            CreateStatRow(
                "HP",
                player.CurrentHp,
                player.MaxHp,
                opponent.CurrentHp,
                opponent.MaxHp
            )
        );

        WriteRow(
            CreateStatRow(
                "SP",
                player.CurrentSp,
                player.MaxSp,
                opponent.CurrentSp,
                opponent.MaxSp
            )
        );
    }

    // Displays a short hit animation on the damaged Digimon
public static void ShowDamageAnimation(
    Digimon player,
    Digimon opponent,
    Digimon damagedDigimon,
    int round)
{
    const int frames = 4;
    const int delay = 100;

    for (int i = 0; i < frames; i++)
    {
        Console.Clear();

        DrawHeader();

        DrawCombatants(
            player,
            opponent,
            damagedDigimon,
            i % 2 == 0
        );

        DrawSeparator();
        WriteRow($"ROUND {round:00}", true);
        DrawSeparator();

        WriteRow("");

        DrawBottomBorder();

        Thread.Sleep(delay);
    }
}

    // Creates a damaged version of a sprite
    private static string[] GetHitSprite(
        string[] sprite)
    {
        return sprite
            .Select(line =>
                line
                    .Replace("█", "▓")
                    .Replace("▄", "▒")
                    .Replace("▀", "▒")
                    .Replace("●", "×")
            )
            .ToArray();
    }

    // Creates one HP or SP display row
    private static string CreateStatRow(
        string label,
        int playerCurrent,
        int playerMaximum,
        int opponentCurrent,
        int opponentMaximum)
    {
        string playerStat =
            $"{label} {CreateBar(playerCurrent, playerMaximum)} " +
            $"{playerCurrent,4}/{playerMaximum,-4}";

        string opponentStat =
            $"{label} {CreateBar(opponentCurrent, opponentMaximum)} " +
            $"{opponentCurrent,4}/{opponentMaximum,-4}";

        return $"{playerStat}       {opponentStat}";
    }


    // Writes a row with exactly the same width as the battle box
    private static void WriteRow(
        string text,
        bool centered = false)
    {
        if (text.Length > InnerWidth)
        {
            text = text[..InnerWidth];
        }

        if (centered)
        {
            int leftPadding =
                (InnerWidth - text.Length) / 2;

            text =
                new string(' ', leftPadding) +
                text;
        }

        Console.WriteLine(
            $"║{text.PadRight(InnerWidth)}║"
        );
    }


    // Draws the top border
    private static void DrawTopBorder()
    {
        Console.WriteLine(
            $"╔{new string('═', InnerWidth)}╗"
        );
    }


    // Draws a horizontal separator
    private static void DrawSeparator()
    {
        Console.WriteLine(
            $"╠{new string('═', InnerWidth)}╣"
        );
    }


    // Draws the bottom border
    private static void DrawBottomBorder()
    {
        Console.WriteLine(
            $"╚{new string('═', InnerWidth)}╝"
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
            Math.Clamp(
                current * BarLength / maximum,
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