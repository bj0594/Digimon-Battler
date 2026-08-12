public static class BattleView
{
    // Battle screen dimensions
    private const int InnerWidth = 72;

    // Horizontal center positions for each combatant
    private const int PlayerCenterX = 21;
    private const int OpponentCenterX = 53;

    // Length of HP/SP bars
    private const int BarLength = 18;

    // Player sprite
    private static readonly string[] PlayerSprite =
    {
        "  ▄██▄",
        "▄██████▄",
        "███████●",
        " ██████▀",
        "  ████",
        " ██  ██"
    };

    // Opponent sprite
    private static readonly string[] OpponentSprite =
    {
        "  ▄█ █▄",
        " ▄█████▄ ⌜/",
        "█●█●████╱",
        "███████▀",
        " ██ ██"
    };


    // Draws the entire battle screen
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

        DrawBottomBorder();

        Console.SetCursorPosition(0, 0);
    }


    // Draws the header
    private static void DrawHeader()
    {
        DrawTopBorder();

        WriteInside(
            1,
            CenterText("DIGIMON BATTLER", InnerWidth)
        );

        DrawSeparator(2);
    }


    // Draws the player and opponent section
    private static void DrawCombatants(
        Digimon player,
        Digimon opponent)
    {
        // Clear the combat area while keeping the side walls.
        for (int y = 3; y <= 16; y++)
        {
            WriteInside(y, "");
        }


        // -------------------------
        // PLAYER
        // -------------------------

        // Player name and attribute
        WriteAtCenter(
            PlayerCenterX,
            4,
            player.Name
        );

        WriteAtCenter(
            PlayerCenterX,
            5,
            player.Attribute
        );


        // -------------------------
        // OPPONENT
        // -------------------------

        // Opponent sprite is one row higher.
        DrawSprite(
            OpponentSprite,
            OpponentCenterX,
            4
        );


        // -------------------------
        // PLAYER SPRITE
        // -------------------------

        DrawSprite(
            PlayerSprite,
            PlayerCenterX,
            7
        );


        // Opponent name and attribute
        WriteAtCenter(
            OpponentCenterX,
            9,
            opponent.Name
        );

        WriteAtCenter(
            OpponentCenterX,
            10,
            opponent.Attribute
        );


        // -------------------------
        // HP / SP
        // -------------------------

        DrawStatBars(
            player,
            opponent
        );


        // Bottom of combatant area
        DrawSeparator(17);
    }


    // Draws a sprite at a specific position
    private static void DrawSprite(
        string[] sprite,
        int centerX,
        int startY)
    {
        for (int i = 0; i < sprite.Length; i++)
        {
            WriteAtCenter(
                centerX,
                startY + i,
                sprite[i]
            );
        }
    }


    // Draws HP and SP for both Digimon
    private static void DrawStatBars(
        Digimon player,
        Digimon opponent)
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


        string playerHp =
            $"HP {playerHpBar} {player.CurrentHp}/{player.MaxHp}";

        string opponentHp =
            $"HP {opponentHpBar} {opponent.CurrentHp}/{opponent.MaxHp}";

        string playerSp =
            $"SP {playerSpBar} {player.CurrentSp}/{player.MaxSp}";

        string opponentSp =
            $"SP {opponentSpBar} {opponent.CurrentSp}/{opponent.MaxSp}";


        // HP
        WriteAtCenter(
            PlayerCenterX,
            14,
            playerHp
        );

        WriteAtCenter(
            OpponentCenterX,
            14,
            opponentHp
        );


        // SP
        WriteAtCenter(
            PlayerCenterX,
            15,
            playerSp
        );

        WriteAtCenter(
            OpponentCenterX,
            15,
            opponentSp
        );
    }


    // Creates a visual stat bar
    private static string CreateBar(
        int current,
        int maximum)
    {
        if (maximum <= 0)
        {
            return new string(
                '░',
                BarLength
            );
        }

        double percentage =
            (double)current / maximum;

        int filled =
            (int)(percentage * BarLength);

        filled = Math.Clamp(
            filled,
            0,
            BarLength
        );

        return new string(
            '█',
            filled
        )
        +
        new string(
            '░',
            BarLength - filled
        );
    }


    // Draws the current round
    private static void DrawRound(
        int roundNumber,
        Digimon player)
    {
        WriteInside(
            18,
            CenterText(
                $"ROUND {roundNumber:00} — {player.Name}",
                InnerWidth
            )
        );

        DrawSeparator(19);
    }


    // Draws the battle menu
    private static void DrawBattleMenu()
    {
        WriteInside(20, "");
        WriteInside(21, "   > ATTACK");
        WriteInside(22, "     INFO");
        WriteInside(23, "     FLEE");
        WriteInside(24, "");

        DrawSeparator(25);
    }


    // Draws the battle log
    private static void DrawBattleLog(
        Digimon player)
    {
        WriteInside(
            26,
            " BATTLE LOG"
        );

        WriteInside(
            27,
            $" {player.Name} is ready for battle."
        );
    }


    // Draws the top border
    private static void DrawTopBorder()
    {
        Console.SetCursorPosition(0, 0);

        Console.Write(
            $"╔{new string('═', InnerWidth)}╗"
        );
    }


    // Draws a horizontal separator
    private static void DrawSeparator(int y)
    {
        Console.SetCursorPosition(0, y);

        Console.Write(
            $"╠{new string('═', InnerWidth)}╣"
        );
    }


    // Draws the bottom border
    private static void DrawBottomBorder()
    {
        Console.SetCursorPosition(0, 28);

        Console.Write(
            $"╚{new string('═', InnerWidth)}╝"
        );
    }


    // Writes content inside the battle box
    private static void WriteInside(
        int y,
        string content)
    {
        if (content.Length > InnerWidth)
        {
            content = content[..InnerWidth];
        }

        Console.SetCursorPosition(0, y);

        Console.Write(
            $"║{content.PadRight(InnerWidth)}║"
        );
    }


    // Writes text centered around a specific X position
    private static void WriteAtCenter(
        int centerX,
        int y,
        string text)
    {
        int x =
            centerX - (text.Length / 2);

        // Keep content inside the side walls.
        x = Math.Max(1, x);

        if (x + text.Length > InnerWidth + 1)
        {
            text = text[..Math.Max(
                0,
                InnerWidth + 1 - x
            )];
        }

        Console.SetCursorPosition(
            x,
            y
        );

        Console.Write(text);
    }


    // Centers text inside a specified width
    private static string CenterText(
        string text,
        int width)
    {
        if (text.Length >= width)
        {
            return text[..width];
        }

        int leftPadding =
            (width - text.Length) / 2;

        int rightPadding =
            width - text.Length - leftPadding;

        return new string(' ', leftPadding)
             + text
             + new string(' ', rightPadding);
    }
}