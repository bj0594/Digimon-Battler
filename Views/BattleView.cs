public static partial class BattleView
{
    private static int lastSelectedMove;

    private static readonly string[] Actions =
    {
        "ATTACK",
        "INFO",
        "FLEE"
    };


    // Lets the player choose their Digimon
    public static Digimon ChooseDigimon(
        List<Digimon> digimonList)
    {
        int selected = 0;

        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("CHOOSE YOUR DIGIMON", true);
            WriteRow("");

            for (int i = 0; i < digimonList.Count; i++)
            {
                string marker =
                    i == selected
                        ? ">"
                        : " ";

                WriteRow(
                    $"   {marker} {digimonList[i].Name,-20} " +
                    $"{digimonList[i].Attribute}"
                );
            }

            WriteRow("");
            WriteRow("Use ARROW KEYS and press ENTER");
            DrawBottomBorder();

            ConsoleKey key =
                Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected--;

                    if (selected < 0)
                    {
                        selected = digimonList.Count - 1;
                    }

                    break;

                case ConsoleKey.DownArrow:
                    selected++;

                    if (selected >= digimonList.Count)
                    {
                        selected = 0;
                    }

                    break;

                case ConsoleKey.Enter:
                    return digimonList[selected];
            }
        }
    }


    // Lets the player choose their opponent
    public static Digimon ChooseOpponent(
        List<Digimon> digimonList,
        Digimon player)
    {
        List<Digimon> opponents = digimonList
            .Where(digimon => digimon != player)
            .ToList();

        int selected = 0;

        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("CHOOSE YOUR OPPONENT", true);
            WriteRow("");

            for (int i = 0; i < opponents.Count; i++)
            {
                string marker =
                    i == selected
                        ? ">"
                        : " ";

                WriteRow(
                    $"   {marker} {opponents[i].Name,-20} " +
                    $"{opponents[i].Attribute}"
                );
            }

            WriteRow("");
            WriteRow("Use ARROW KEYS and press ENTER");
            DrawBottomBorder();

            ConsoleKey key =
                Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected--;

                    if (selected < 0)
                    {
                        selected = opponents.Count - 1;
                    }

                    break;

                case ConsoleKey.DownArrow:
                    selected++;

                    if (selected >= opponents.Count)
                    {
                        selected = 0;
                    }

                    break;

                case ConsoleKey.Enter:
                    return opponents[selected];
            }
        }
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

            // Return to the main menu if ESC was pressed
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

            ConsoleKey key =
                Console.ReadKey(true).Key;

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
}