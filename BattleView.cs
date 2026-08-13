public static class BattleView
{
    private const int InnerWidth = 72;
    private const int BarLength = 18;

    // Remembers the last selected Move between rounds
    private static int lastSelectedMove;

    private static readonly string[] Actions =
    {
        "ATTACK",
        "INFO",
        "FLEE"
    };

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

        DrawTopBorder();
        WriteRow("DIGIMON BATTLER", true);
        DrawSeparator();

        DrawCombatants(player, opponent);

        DrawSeparator();
        WriteRow($"ROUND {round:00}", true);
        DrawSeparator();

        WriteRow("");

        for (int i = 0; i < Actions.Length; i++)
        {
            string marker = i == selectedAction ? ">" : " ";
            WriteRow($"   {marker} {Actions[i]}");
        }

        WriteRow("");
        DrawBottomBorder();
    }


    // Handles navigation through the main battle menu
    public static Move? ChooseMove(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
        string battleLog,
        bool startInAttack = false)
    {
        // End the battle if the Digimon cannot afford any Move
        if (!moves.Any(move => move.SpCost <= digimon.CurrentSp))
        {
            return null;
        }

        // Start directly in the Move menu after the first round
        if (startInAttack)
        {
            Move? move = ChooseAttack(
                moves,
                digimon,
                player,
                opponent,
                round,
                battleLog,
                lastSelectedMove
            );

            // If ESC was pressed, return to the main battle menu
            if (move != null)
            {
                return move;
            }
        }

        int selected = 0;

        while (true)
        {
            DrawBattleScreen(
                player,
                opponent,
                round,
                battleLog,
                selected
            );

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected--;

                    if (selected < 0)
                    {
                        selected = Actions.Length - 1;
                    }

                    break;

                case ConsoleKey.DownArrow:
                    selected++;

                    if (selected >= Actions.Length)
                    {
                        selected = 0;
                    }

                    break;

                case ConsoleKey.Enter:
                if (selected == 0)
                {
                    Move? move = ChooseAttack(
                        moves,
                        digimon,
                        player,
                        opponent,
                        round,
                        battleLog,
                        lastSelectedMove
                    );

                    if (move != null)
                    {
                        return move;
                    }
                }
                
    else if (selected == 2)
    {
        // Player chooses to flee the battle
        return null;
    }

    // INFO is not implemented yet.
    break;
            }
        }
    }

    // Displays the result of an attack and waits for the player
    public static void ShowBattleResult(
        Digimon player,
        Digimon opponent,
        int round,
        string battleLog)
    {
        Console.Clear();

        DrawTopBorder();
        WriteRow("DIGIMON BATTLER", true);
        DrawSeparator();

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

    // Handles navigation through the available Moves
    private static Move? ChooseAttack(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
        string battleLog,
        int selectedMove)
    {
        int row = selectedMove % 3;
        int column = selectedMove / 3;

        while (true)
        {
            int selected = row + column * 3;

            DrawMoveScreen(
                moves,
                digimon,
                player,
                opponent,
                round,
                battleLog,
                selected
            );

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    column = 0;
                    break;

                case ConsoleKey.RightArrow:
                    if (column == 0 && row + 3 < moves.Count)
                    {
                        column = 1;
                    }

                    break;

                case ConsoleKey.UpArrow:
                    if (row > 0)
                    {
                        row--;
                    }

                    break;

                case ConsoleKey.DownArrow:
                    if (row < 2 && row + 1 < moves.Count)
                    {
                        row++;
                    }

                    break;

                case ConsoleKey.Escape:
                    return null;

                case ConsoleKey.Enter:
                    Move move = moves[selected];

                    if (move.SpCost <= digimon.CurrentSp)
                    {
                        // Remember this position for the next round
                        lastSelectedMove = selected;

                        return move;
                    }

                    break;
            }
        }
    }


    // Draws the battle screen with the Move selection menu
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

        DrawTopBorder();
        WriteRow("DIGIMON BATTLER", true);
        DrawSeparator();

        DrawCombatants(player, opponent);

        DrawSeparator();
        WriteRow($"ROUND {round:00}", true);
        DrawSeparator();

        WriteRow("   CHOOSE MOVE");

        // Display six Moves in two columns
        for (int row = 0; row < 3; row++)
        {
            int leftIndex = row;
            int rightIndex = row + 3;

            string left = GetMoveText(
                moves,
                digimon,
                leftIndex,
                selected
            );

            string right = GetMoveText(
                moves,
                digimon,
                rightIndex,
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

        string marker = selected == index ? ">" : " ";

        string cost =
            move.SpCost <= digimon.CurrentSp
                ? $"{move.SpCost} SP"
                : "NO SP";

        return $"{marker} {move.Name,-20} {cost}";
    }


    // Draws the two Digimon and their battle information
    private static void DrawCombatants(
        Digimon player,
        Digimon opponent)
    {
        WriteRow(
            $"             {player.Name,-15}                 {opponent.Name,-15}"
        );

        WriteRow(
            $"             {player.Attribute,-15}                 {opponent.Attribute,-15}"
        );

        // Draw the opponent one row higher than the player
        for (int i = 0; i < OpponentSprite.Length; i++)
        {
            string playerSprite =
                i > 0
                    ? PlayerSprite[i - 1]
                    : "";

            WriteRow(
                $"             {playerSprite,-15}                 {OpponentSprite[i],-15}"
            );
        }

        // Draw the final row of the player sprite
        WriteRow(
            $"             {PlayerSprite[5],-15}"
        );

        WriteRow("");

        string playerHp =
            $"HP {CreateBar(player.CurrentHp, player.MaxHp)} {player.CurrentHp,4}/{player.MaxHp,-4}";

        string opponentHp =
            $"HP {CreateBar(opponent.CurrentHp, opponent.MaxHp)} {opponent.CurrentHp,4}/{opponent.MaxHp,-4}";

        string playerSp =
            $"SP {CreateBar(player.CurrentSp, player.MaxSp)} {player.CurrentSp,4}/{player.MaxSp,-4}";

        string opponentSp =
            $"SP {CreateBar(opponent.CurrentSp, opponent.MaxSp)} {opponent.CurrentSp,4}/{opponent.MaxSp,-4}";

        WriteRow(
            $"{playerHp}       {opponentHp}"
        );

        WriteRow(
            $"{playerSp}       {opponentSp}"
        );
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
                new string(' ', leftPadding) + text;
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
             + new string('░', BarLength - filled);
    }
}