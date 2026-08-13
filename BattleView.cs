public static class BattleView
{
    private const int InnerWidth = 72;
    private const int BarLength = 18;
    private static int lastSelectedMove = 0;

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

        string[] actions =
        {
            "ATTACK",
            "INFO",
            "FLEE"
        };

        for (int i = 0; i < actions.Length; i++)
        {
            string marker =
                i == selectedAction ? ">" : " ";

            WriteRow(
                $"   {marker} {actions[i]}"
            );
        }

        WriteRow("");

        DrawSeparator();

        WriteRow("BATTLE LOG");

        WriteRow(battleLog);

        DrawBottomBorder();
    }


    // Handles the main battle menu
    public static Move? ChooseMove(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
        string battleLog,
        bool startInAttack = false,
        int selectedMove = 0)
    {
        if (!moves.Any(move => move.SpCost <= digimon.CurrentSp))
        {
            return null;
        }
        
        int selected = 0;

        if (startInAttack)
        {
            return ChooseAttack(
                moves,
                digimon,
                player,
                opponent,
                round,
                battleLog,
                lastSelectedMove
            );
        }

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

            if (key == ConsoleKey.UpArrow)
            {
                selected--;

                if (selected < 0)
                {
                    selected = 2;
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selected++;

                if (selected > 2)
                {
                    selected = 0;
                }
            }
            else if (key == ConsoleKey.Enter)
            {
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

                // INFO and FLEE are not implemented yet.
            }
        }
    }


    // Handles Move selection after choosing ATTACK
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

            if (key == ConsoleKey.LeftArrow)
            {
                column = 0;
            }
            else if (key == ConsoleKey.RightArrow)
            {
                if (column == 0 && row + 3 < moves.Count)
                {
                    column = 1;
                }
            }
            else if (key == ConsoleKey.UpArrow)
            {
                if (row > 0)
                {
                    row--;
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                if (row < 2 && row + 1 < moves.Count)
                {
                    row++;
                }
            }
            else if (key == ConsoleKey.Escape)
            {
                return null;
            }
            else if (key == ConsoleKey.Enter)
            {
                Move move = moves[selected];

                if (move.SpCost <= digimon.CurrentSp)
                {
                    lastSelectedMove = selected;
                    return move;
                }
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

        for (int row = 0; row < 3; row++)
        {
            int leftIndex = row;
            int rightIndex = row + 3;

            string left = "";
            string right = "";

            if (leftIndex < moves.Count)
            {
                Move move = moves[leftIndex];

                string marker =
                    selected == leftIndex ? ">" : " ";

                string status =
                    move.SpCost <= digimon.CurrentSp
                        ? $"{move.SpCost} SP"
                        : "NO SP";

                left =
                    $"{marker} {move.Name,-20} {status}";
            }

            if (rightIndex < moves.Count)
            {
                Move move = moves[rightIndex];

                string marker =
                    selected == rightIndex ? ">" : " ";

                string status =
                    move.SpCost <= digimon.CurrentSp
                        ? $"{move.SpCost} SP"
                        : "NO SP";

                right =
                    $"{marker} {move.Name,-20} {status}";
            }

            WriteRow(
                $"   {left,-31}{right}"
            );
        }

        WriteRow("");

        DrawSeparator();

        WriteRow("BATTLE LOG");

        WriteRow(battleLog);

        DrawBottomBorder();
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

        // Opponent starts one row higher than the player
        for (int i = 0; i < OpponentSprite.Length; i++)
        {
            string playerSprite = "";

            if (i > 0)
            {
                playerSprite = PlayerSprite[i - 1];
            }

            WriteRow(
                $"             {playerSprite,-15}                 {OpponentSprite[i],-15}"
            );
        }

        // Last row of the player sprite
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