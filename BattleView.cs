public static class BattleView
{
    private const int InnerWidth = 72;
    private const int BarLength = 18;
    private const int InfoColumnWidth = 36;

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

        DrawHeader();
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


    // Draws the common battle screen header
    private static void DrawHeader()
    {
        DrawTopBorder();
        WriteRow("DIGIMON BATTLER", true);
        DrawSeparator();
    }


    // Handles navigation through the main battle menu
    public static (Move? Move, bool Fled) ChooseMove(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
        string battleLog,
        bool startInAttack = false)
    {
        // End the battle if no Move can be afforded
        if (!HasAvailableMove(moves, digimon))
        {
            return (null, true);
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

            // If ESC was pressed, return to the main menu
            if (move != null)
            {
                return (move, false);
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
                            return (move, false);
                        }
                    }
                    else if (selected == 1)
                    {
                        // INFO does not advance the battle
                        ShowInfoScreen(
                            player,
                            opponent
                        );
                    }
                    else if (selected == 2)
                    {
                        // Player chooses to flee
                        return (null, true);
                    }

                    break;
            }
        }
    }


    // Checks whether a Digimon can afford at least one Move
    private static bool HasAvailableMove(
        List<Move> moves,
        Digimon digimon)
    {
        return moves.Any(
            move => move.SpCost <= digimon.CurrentSp
        );
    }


    // Displays the result of an attack and waits for the player
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


    // Displays information about both Digimon
    public static void ShowInfoScreen(
        Digimon player,
        Digimon opponent)
    {
        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("DIGIMON INFO", true);
            WriteRow("");

            // Display names and attributes
            WriteInfoRow(
                player.Name,
                opponent.Name
            );

            WriteInfoRow(
                player.Attribute,
                opponent.Attribute
            );

            WriteRow("");

            // Display HP and SP
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

            // Display combat stats
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

            // INFO only responds to ESC
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                return;
            }
        }
    }


    // Writes two pieces of information in aligned columns
    private static void WriteInfoRow(
        string playerText,
        string opponentText)
    {
        string line =
            $"    {playerText,-InfoColumnWidth}{opponentText}";

        WriteRow(line);
    }


    // Writes a labelled stat for both Digimon
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


    // Displays the victory screen
    public static bool ShowVictoryScreen()
    {
        return ShowEndScreen(
            new[]
            {
                "██╗   ██╗██╗ ██████╗████████╗ ██████╗ ██████╗ ██╗   ██╗",
                "██║   ██║██║██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗╚██╗ ██╔╝",
                "██║   ██║██║██║        ██║   ██║   ██║██████╔╝ ╚████╔╝",
                "╚██╗ ██╔╝██║██║        ██║   ██║   ██║██╔══██╗  ╚██╔╝",
                " ╚████╔╝ ██║╚██████╗   ██║   ╚██████╔╝██║  ██║   ██║",
                "  ╚═══╝  ╚═╝ ╚═════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝   ╚═╝"
            }
        );
    }


    // Displays the defeat screen
    public static bool ShowDefeatScreen()
    {
        return ShowEndScreen(
            new[]
            {
                "██████╗ ███████╗███████╗███████╗ █████╗ ████████╗",
                "██╔══██╗██╔════╝██╔════╝██╔════╝██╔══██╗╚══██╔══╝",
                "██║  ██║█████╗  █████╗  █████╗  ███████║   ██║",
                "██║  ██║██╔══╝  ██╔══╝  ██╔══╝  ██╔══██║   ██║",
                "██████╔╝███████╗██║     ███████╗██║  ██║   ██║",
                "╚═════╝ ╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝"
            }
        );
    }


    // Displays a victory or defeat screen
    private static bool ShowEndScreen(
        string[] title)
    {
        Console.Clear();

        Console.WriteLine();
        Console.WriteLine();

        foreach (string line in title)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(
            "                 Press ENTER to try again"
        );
        Console.WriteLine(
            "                 Press ESC to quit"
        );

        while (true)
        {
            ConsoleKey key =
                Console.ReadKey(true).Key;

            if (key == ConsoleKey.Enter)
            {
                return true;
            }

            if (key == ConsoleKey.Escape)
            {
                Console.Clear();
                return false;
            }
        }
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
            int selected =
                row + column * 3;

            DrawMoveScreen(
                moves,
                digimon,
                player,
                opponent,
                round,
                battleLog,
                selected
            );

            ConsoleKey key =
                Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    column = 0;
                    break;

                case ConsoleKey.RightArrow:
                    if (
                        column == 0 &&
                        row + 3 < moves.Count
                    )
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
                    if (
                        row < 2 &&
                        row + 1 < moves.Count
                    )
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
        Digimon opponent)
    {
        WriteRow(
            $"             {player.Name,-15}                 {opponent.Name,-15}"
        );

        WriteRow(
            $"             {player.Attribute,-15}                 {opponent.Attribute,-15}"
        );

        // Draw the opponent one row higher than the player
        for (
            int i = 0;
            i < OpponentSprite.Length;
            i++
        )
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