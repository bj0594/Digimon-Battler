public static partial class BattleView
{
    private static readonly string[] VictoryTitle =
    {
        "██╗   ██╗██╗ ██████╗████████╗ ██████╗ ██████╗ ██╗   ██╗",
        "██║   ██║██║██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗╚██╗ ██╔╝",
        "██║   ██║██║██║        ██║   ██║   ██║██████╔╝ ╚████╔╝",
        "╚██╗ ██╔╝██║██║        ██║   ██║   ██║██╔══██╗  ╚██╔╝",
        " ╚████╔╝ ██║╚██████╗   ██║   ╚██████╔╝██║  ██║   ██║",
        "  ╚═══╝  ╚═╝ ╚═════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝   ╚═╝"
    };

    private static readonly string[] DefeatTitle =
    {
        "██████╗ ███████╗███████╗███████╗ █████╗ ████████╗",
        "██╔══██╗██╔════╝██╔════╝██╔════╝██╔══██╗╚══██╔══╝",
        "██║  ██║█████╗  █████╗  █████╗  ███████║   ██║",
        "██║  ██║██╔══╝  ██╔══╝  ██╔══╝  ██╔══██║   ██║",
        "██████╔╝███████╗██║     ███████╗██║  ██║   ██║",
        "╚═════╝ ╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝"
    };


    // Displays the victory screen.
    public static bool ShowVictoryScreen()
    {
        return ShowEndScreen(VictoryTitle);
    }


    // Displays the defeat screen.
    public static bool ShowDefeatScreen()
    {
        return ShowEndScreen(DefeatTitle);
    }


    // Displays an end-of-battle screen and waits for the player's choice.
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

        // Ignore all keys except ENTER and ESC.
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
}