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

    // Let the opponent choose a random available move
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

    // Stop here if the round ended because of HP reaching zero
    if (battle.IsFinished)
    {
        break;
    }

    // Check whether either Digimon can still afford a Move
    bool playerCanAttack = HasAvailableMove(
        TestData.MoveList,
        player
    );

    bool opponentCanAttack = HasAvailableMove(
        TestData.MoveList,
        opponent
    );

    if (!playerCanAttack && !opponentCanAttack)
    {
        battle.IsFinished = true;
    }
    else if (!playerCanAttack)
    {
        battle.IsFinished = true;
        battle.Winner = opponent;
    }
    else if (!opponentCanAttack)
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
else
{
    Console.WriteLine(
        "Both Digimon are out of SP."
    );
}

Console.ReadKey();


// Checks whether a Digimon can afford at least one Move
static bool HasAvailableMove(
    List<Move> moves,
    Digimon digimon)
{
    return moves.Any(
        move => move.SpCost <= digimon.CurrentSp
    );
}


// Chooses a random Move the opponent can afford
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

    return availableMoves[
        Random.Shared.Next(availableMoves.Count)
    ];
}