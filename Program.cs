Digimon player = TestData.DigimonList[0];
Digimon opponent = TestData.DigimonList[1];

Battle battle = new Battle(player, opponent);

string battleLog = "";

bool firstTurn = true;

while (!battle.IsFinished)
{
    // Let the player choose a move through the battle UI
    Move? playerMove = BattleView.ChooseMove(
        TestData.MoveList,
        player,
        player,
        opponent,
        battle.Round,
        battleLog,
        !firstTurn
    );

    // Player has no available moves
    if (playerMove == null)
    {
        battle.IsFinished = true;
        battle.Winner = opponent;
        break;
    }

    // Opponent chooses a random available move
    Move? opponentMove = ChooseOpponentMove(
        TestData.MoveList,
        opponent
    );

    // Opponent has no available moves
    if (opponentMove == null)
    {
        battle.IsFinished = true;
        battle.Winner = player;
        break;
    }

    // Execute the round
    battleLog = battle.NextRound(
        playerMove,
        opponentMove
    );
    firstTurn = false;

    // Check if the opponent has any moves left
    if (!TestData.MoveList.Any(
        move => move.SpCost <= opponent.CurrentSp))
    {
        battle.IsFinished = true;
        battle.Winner = player;
    }
}

Console.Clear();

if (battle.Winner != null)
{
    Console.WriteLine(
        $"{battle.Winner.Name} wins!"
    );
}

Console.ReadKey();


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