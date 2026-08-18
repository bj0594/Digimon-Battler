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


    // Lets the player choose an Attribute.
    public static string ChooseAttribute(
        List<Digimon> digimonList)
    {
        // Get all unique Attributes from the dataset.
        List<string> attributes = digimonList
            .Select(digimon => digimon.Attribute)
            .Distinct()
            .OrderBy(attribute => attribute)
            .ToList();

        int selected = 0;

        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("CHOOSE ATTRIBUTE", true);
            WriteRow("");

            // Display every available Attribute.
            for (int i = 0; i < attributes.Count; i++)
            {
                string marker =
                    i == selected ? ">" : " ";

                WriteRow(
                    $"   {marker} {attributes[i]}"
                );
            }

            WriteRow("");
            WriteRow("");
            WriteRow("Use ARROW KEYS and press ENTER", true);
            DrawBottomBorder();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:

                    selected--;

                    if (selected < 0)
                    {
                        selected = attributes.Count - 1;
                    }

                    break;


                case ConsoleKey.DownArrow:

                    selected++;

                    if (selected >= attributes.Count)
                    {
                        selected = 0;
                    }

                    break;


                case ConsoleKey.Enter:

                    return attributes[selected];
            }
        }
    }


    // Lets the player choose their own Digimon.
    public static Digimon ChooseDigimon(
        List<Digimon> digimonList,
        string selectedAttribute)
    {
        // Only show Digimon with the selected Attribute.
        // that have at least one valid opponent
        List<Digimon> availableDigimon = digimonList
            .Where(digimon =>
                digimon.Attribute == selectedAttribute &&
                digimonList.Any(opponent =>
                    opponent.Stage == digimon.Stage &&
                    opponent.Attribute != digimon.Attribute &&
                    opponent != digimon))
            .OrderBy(digimon => digimon.Name)
            .ToList();

        int selected = 0;
        int scrollOffset = 0;

        // Keep the menu at a fixed height.
        const int visibleCount = 6;

        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("CHOOSE YOUR DIGIMON", true);
            WriteRow("");

            // Show an up arrow when there are Digimon above the visible list.
            WriteRow(
                scrollOffset > 0
                    ? "                         ▲"
                    : ""
            );

            // Display only the visible Digimon.
            for (int i = 0; i < visibleCount; i++)
            {
                int index = scrollOffset + i;

                if (index >= availableDigimon.Count)
                {
                    WriteRow("");
                    continue;
                }

                Digimon digimon = availableDigimon[index];

                string marker =
                    index == selected ? ">" : " ";

                WriteRow(
                    $"   {marker} {digimon.Name,-20}" +
                    $" {digimon.Stage,-12}" +
                    $" HP {digimon.MaxHp,5}" +
                    $" SP {digimon.MaxSp,4}"
                );
            }

            // Show a down arrow when there are Digimon below the visible list.
            WriteRow(
                scrollOffset + visibleCount < availableDigimon.Count
                    ? "                         ▼"
                    : ""
            );

            WriteRow("");
            WriteRow("");
            WriteRow("Use ARROW KEYS and press ENTER", true);
            DrawBottomBorder();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:

                    if (selected > 0)
                    {
                        selected--;

                        // Scroll up when the selected Digimon leaves the window.
                        if (selected < scrollOffset)
                        {
                            scrollOffset--;
                        }
                    }

                    break;


                case ConsoleKey.DownArrow:

                    if (selected < availableDigimon.Count - 1)
                    {
                        selected++;

                        // Scroll down when the selected Digimon leaves the window.
                        if (selected >= scrollOffset + visibleCount)
                        {
                            scrollOffset++;
                        }
                    }

                    break;


                case ConsoleKey.Enter:

                    return availableDigimon[selected];
            }
        }
    }


    // Lets the player choose an opponent.
    public static Digimon ChooseOpponent(
        List<Digimon> digimonList,
        Digimon player)
    {
        // Only allow opponents from the same Stage and a different Attribute.
        // The player cannot choose their own Digimon.
        List<Digimon> opponents = digimonList
            .Where(digimon =>
                digimon.Stage == player.Stage &&
                digimon.Attribute != player.Attribute &&
                digimon != player)
            .OrderBy(digimon => digimon.Name)
            .ToList();

        int selected = 0;
        int scrollOffset = 0;

        // Keep the menu at a fixed height.
        const int visibleCount = 6;

        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("CHOOSE YOUR OPPONENT", true);
            WriteRow("");

            // Show an up arrow when there are opponents above the visible list.
            WriteRow(
                scrollOffset > 0
                    ? "                         ▲"
                    : ""
            );

            // Display only the visible opponents.
            for (int i = 0; i < visibleCount; i++)
            {
                int index = scrollOffset + i;

                if (index >= opponents.Count)
                {
                    WriteRow("");
                    continue;
                }

                Digimon digimon = opponents[index];

                string marker =
                    index == selected ? ">" : " ";

                WriteRow(
                    $"   {marker} {digimon.Name,-20}" +
                    $" {digimon.Stage,-12}" +
                    $" {digimon.Attribute,-10}" +
                    $" HP {digimon.MaxHp,5}" +
                    $" SP {digimon.MaxSp,4}"
                );
            }

            // Show a down arrow when there are opponents below the visible list.
            WriteRow(
                scrollOffset + visibleCount < opponents.Count
                    ? "                         ▼"
                    : ""
            );

            WriteRow("");
            WriteRow("");
            WriteRow("Use ARROW KEYS and press ENTER", true);
            DrawBottomBorder();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:

                    if (selected > 0)
                    {
                        selected--;

                        // Scroll up when the selected opponent leaves the window.
                        if (selected < scrollOffset)
                        {
                            scrollOffset--;
                        }
                    }

                    break;


                case ConsoleKey.DownArrow:

                    if (selected < opponents.Count - 1)
                    {
                        selected++;

                        // Scroll down when the selected opponent leaves the window.
                        if (selected >= scrollOffset + visibleCount)
                        {
                            scrollOffset++;
                        }
                    }

                    break;


                case ConsoleKey.Enter:

                    return opponents[selected];
            }
        }
    }


    // Lets the player confirm the selected Digimon and opponent.
    public static bool ConfirmBattle(
        Digimon player,
        Digimon opponent)
    {
        while (true)
        {
            Console.Clear();

            DrawHeader();

            WriteRow("");
            WriteRow("BATTLE READY", true);
            WriteRow("");

            WriteRow(
                $"   {player.Name,-20}" +
                $" {player.Stage,-12}" +
                $" {player.Attribute,-10}" +
                $" HP {player.MaxHp,5}" +
                $" SP {player.MaxSp,4}"
            );

            WriteRow(
                $"   {opponent.Name,-20}" +
                $" {opponent.Stage,-12}" +
                $" {opponent.Attribute,-10}" +
                $" HP {opponent.MaxHp,5}" +
                $" SP {opponent.MaxSp,4}"
            );

            WriteRow("");
            WriteRow("");
            WriteRow("Press ENTER to fight.", true);
            WriteRow("Press ESC to cancel.", true);

            DrawBottomBorder();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Enter:

                    return true;


                case ConsoleKey.Escape:

                    return false;
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
        const int movesPerPage = 6;
        const int columnSize = 3;

        int selected = selectedMove;

        while (true)
        {
            int page = selected / movesPerPage;

            DrawMoveScreen(
                moves,
                digimon,
                player,
                opponent,
                round,
                selected,
                page
            );

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:

                    int upPosition =
                        selected % movesPerPage;

                    int upRow =
                        upPosition % columnSize;

                    if (upRow > 0)
                    {
                        // Move one row up in the same column.
                        selected--;
                    }
                    else if (selected >= movesPerPage)
                    {
                        // Move to the bottom row of the previous page,
                        // keeping the same column.
                        selected -= movesPerPage - 2;
                    }

                    break;


                case ConsoleKey.DownArrow:

                    int downPosition =
                        selected % movesPerPage;

                    int downRow =
                        downPosition % columnSize;

                    if (downRow < columnSize - 1)
                    {
                        // Move one row down in the same column.
                        int next = selected + 1;

                        if (next < moves.Count)
                        {
                            selected = next;
                        }
                    }
                    else
                    {
                        // Move to the top row of the next page,
                        // keeping the same column.
                        int next =
                            selected + movesPerPage - 2;

                        if (next < moves.Count)
                        {
                            selected = next;
                        }
                    }

                    break;


                case ConsoleKey.LeftArrow:

                    int leftPosition =
                        selected % movesPerPage;

                    if (leftPosition >= columnSize)
                    {
                        // Move to the same row in the left column.
                        selected -= columnSize;
                    }

                    break;


                case ConsoleKey.RightArrow:

                    int rightPosition =
                        selected % movesPerPage;

                    if (rightPosition < columnSize)
                    {
                        // Move to the same row in the right column.
                        int next =
                            selected + columnSize;

                        if (next < moves.Count)
                        {
                            selected = next;
                        }
                    }

                    break;


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