Digimon player = TestData.DigimonList[0];
Digimon opponent = TestData.DigimonList[1];

Battle battle = new Battle(player, opponent);

while (!battle.IsFinished)
{
    BattleView.DrawBattleScreen(
        player,
        opponent,
        battle.Round
    );

   Move? playerMove = ChooseMove(
    TestData.MoveList,
    player
);

if (playerMove == null)
{
    battle.IsFinished = true;
    battle.Winner = opponent;
    break;
}

Move? opponentMove = ChooseOpponentMove(
    TestData.MoveList,
    opponent
);

if (opponentMove == null)
{
    battle.IsFinished = true;
    battle.Winner = player;
    break;
}

battle.NextRound(
    playerMove,
    opponentMove
);
}

Console.Clear();

if (battle.Winner != null)
{
    Console.WriteLine(
        $"{battle.Winner.Name} wins!"
    );
}

Console.ReadKey();


static Move? ChooseMove(
    List<Move> moves,
    Digimon digimon)
{
    if (!moves.Any(move => move.SpCost <= digimon.CurrentSp))
    {
    return null;
    }

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Choose a move:");

        for (int i = 0; i < moves.Count; i++)
        {
            Move move = moves[i];

            if (move.SpCost <= digimon.CurrentSp)
            {
                Console.WriteLine(
                    $"{i + 1}. {move.Name} - {move.SpCost} SP"
                );
            }
            else
            {
                Console.WriteLine(
                    $"{i + 1}. {move.Name} - Not enough SP"
                );
            }
        }

        Console.Write("Choice: ");

        if (int.TryParse(Console.ReadLine(), out int choice) &&
            choice >= 1 &&
            choice <= moves.Count)
        {
            Move selectedMove = moves[choice - 1];

            if (selectedMove.SpCost <= digimon.CurrentSp)
            {
                return selectedMove;
            }

            Console.WriteLine("Not enough SP.");
        }
        else
        {
            Console.WriteLine("Invalid choice. Try again.");
        }
    }
}
    static Move? ChooseOpponentMove(
    List<Move> moves,
    Digimon opponent)
    {
    List<Move> availableMoves = moves
        .Where(move => move.SpCost <= opponent.CurrentSp)
        .ToList();

    if (availableMoves.Count == 0)
    {
        return null;
    }

    Random random = new Random();

    return availableMoves[
        random.Next(availableMoves.Count)
    ];
    }
