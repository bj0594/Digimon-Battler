public static partial class BattleView
{
    // Remembers the last selected Move between rounds.
    private static int lastSelectedMove;

    // Available actions in the main battle menu.
    private static readonly string[] Actions =
    {
        "ATTACK",
        "INFO",
        "FLEE"
    };


    // Lets the player choose their own Digimon.
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

            // Display every available Digimon.
            for (int i = 0; i < digimonList.Count; i++)
            {
                string marker =
                    i == selected ? ">" : " ";

                WriteRow(
                    $"   {marker} {digimonList[i].Name,-20}" +
                    $" {digimonList[i].Attribute}"
                );
            }

            WriteRow("");
            WriteRow("Use ARROW KEYS and press ENTER");
            DrawBottomBorder();

            switch (Console.ReadKey(true).Key)
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


    // Lets the player choose an opponent.
    public static Digimon ChooseOpponent(
        List<Digimon> digimonList,
        Digimon player)
    {
        // Only allow opponents from the same Stage as the player's Digimon.
        // The player cannot choose their own Digimon.
        List<Digimon> opponents = digimonList
            .Where(digimon =>
                digimon.Stage == player.Stage &&
                digimon != player)
            .ToList();

        int selected = 0;

        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("CHOOSE YOUR OPPONENT", true);
            WriteRow("");

            // Display every valid opponent.
            for (int i = 0; i < opponents.Count; i++)
            {
                string marker =
                    i == selected ? ">" : " ";

                WriteRow(
                    $"   {marker} {opponents[i].Name,-20}" +
                    $" {opponents[i].Attribute}"
                );
            }

            WriteRow("");
            WriteRow("Use ARROW KEYS and press ENTER");
            DrawBottomBorder();

            switch (Console.ReadKey(true).Key)
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


    // Handles navigation through the main battle menu.
    public static (Move? Move, bool Fled) ChooseMove(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
        bool startInAttack = false)
    {
        // The battle ends if no Move can be afforded.
        if (!HasAvailableMove(moves, digimon))
        {
            return (null, true);
        }


        // After the first round, open directly in the Move menu.
        if (startInAttack)
        {
            Move? move = ChooseAttack(
                moves,
                digimon,
                player,
                opponent,
                round,
                lastSelectedMove
            );

            // ESC returns to the main battle menu.
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
                selected
            );

            switch (Console.ReadKey(true).Key)
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

                    // ATTACK
                    if (selected == 0)
                    {
                        Move? move = ChooseAttack(
                            moves,
                            digimon,
                            player,
                            opponent,
                            round,
                            lastSelectedMove
                        );

                        if (move != null)
                        {
                            return (move, false);
                        }
                    }

                    // INFO
                    else if (selected == 1)
                    {
                        ShowInfoScreen(
                            player,
                            opponent
                        );
                    }

                    // FLEE
                    else if (selected == 2)
                    {
                        return (null, true);
                    }

                    break;
            }
        }
    }


    // Checks whether at least one Move can be afforded.
    private static bool HasAvailableMove(
        List<Move> moves,
        Digimon digimon)
    {
        return moves.Any(
            move => move.SpCost <= digimon.CurrentSp
        );
    }


    // Handles navigation through the available Moves.
    private static Move? ChooseAttack(
        List<Move> moves,
        Digimon digimon,
        Digimon player,
        Digimon opponent,
        int round,
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
                selected
            );

            switch (Console.ReadKey(true).Key)
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

                // Return to the main battle menu without selecting a Move.
                case ConsoleKey.Escape:
                    return null;

                case ConsoleKey.Enter:

                    Move move = moves[selected];

                    // Do not allow a Move that costs more SP than available.
                    if (move.SpCost <= digimon.CurrentSp)
                    {
                        lastSelectedMove = selected;
                        return move;
                    }

                    break;
            }
        }
    }
}