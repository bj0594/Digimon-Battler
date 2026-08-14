public static partial class BattleView
{
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
}