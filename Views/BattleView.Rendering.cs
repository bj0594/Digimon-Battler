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


    // Draws the main battle menu.
    public static void DrawBattleScreen(
        Digimon player,
        Digimon opponent,
        int round,
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
            string marker = i == selectedAction ? ">" : " ";

            WriteRow(
                $"   {marker} {Actions[i]}"
            );
        }

        WriteRow("");
        DrawBottomBorder();
    }


    // Draws the common header used by battle screens.
    private static void DrawHeader()
    {
        DrawTopBorder();
        WriteRow("DIGIMON BATTLER", true);
        DrawSeparator();
    }


    // Displays the result of the most recent attack.
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
        WriteRow("Press ENTER to continue.", true);
        DrawBottomBorder();

        Console.ReadKey(true);
    }


    // Draws the Move selection menu.
    private static void DrawMoveScreen(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
        int selected)
    {
        Console.Clear();

        DrawHeader();
        DrawCombatants(player, opponent);

        DrawSeparator();
        WriteRow($"ROUND {round:00}", true);
        DrawSeparator();

        WriteRow("   CHOOSE MOVE");

        // Display the available Moves in two columns.
        for (int row = 0; row < 3; row++)
        {
            string left = GetMoveText(
                moves,
                digimon,
                row,
                selected
            );

            string right = GetMoveText(
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


    // Creates the display text for one Move.
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
            selected == index ? ">" : " ";

        string cost =
            move.SpCost <= digimon.CurrentSp
                ? $"{move.SpCost} SP"
                : "NO SP";

        return $"{marker} {move.Name,-20} {cost}";
    }


    // Draws both Digimon, their sprites, and their HP/SP.
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

        // The opponent's sprite starts one row above the player's sprite.
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

        // Draw the final row of the player's sprite.
        WriteRow(
            $"             {playerSprite[^1],-15}"
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


    // Displays a short blinking animation on the damaged Digimon.
    public static void ShowDamageAnimation(
        Digimon player,
        Digimon opponent,
        Digimon damagedDigimon,
        int round)
    {
        const int frames = 4;
        const int frameDelay = 100;

        for (int frame = 0; frame < frames; frame++)
        {
            Console.Clear();

            DrawHeader();

            DrawCombatants(
                player,
                opponent,
                damagedDigimon,
                frame % 2 == 0
            );

            DrawSeparator();
            WriteRow($"ROUND {round:00}", true);
            DrawSeparator();

            WriteRow("");
            DrawBottomBorder();

            Thread.Sleep(frameDelay);
        }
    }


    // Creates the damaged version of a sprite used by the hit animation.
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


    // Creates one aligned HP or SP row for both Digimon.
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


    // Writes text inside the fixed-width battle box.
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
            int padding =
                (InnerWidth - text.Length) / 2;

            text =
                new string(' ', padding) + text;
        }

        Console.WriteLine(
            $"║{text.PadRight(InnerWidth)}║"
        );
    }


    // Draws the top border of the battle box.
    private static void DrawTopBorder()
    {
        Console.WriteLine(
            $"╔{new string('═', InnerWidth)}╗"
        );
    }


    // Draws a horizontal separator.
    private static void DrawSeparator()
    {
        Console.WriteLine(
            $"╠{new string('═', InnerWidth)}╣"
        );
    }


    // Draws the bottom border of the battle box.
    private static void DrawBottomBorder()
    {
        Console.WriteLine(
            $"╚{new string('═', InnerWidth)}╝"
        );
    }


    // Creates a fixed-width visual bar for HP or SP.
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