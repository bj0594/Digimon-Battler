Digimon player = TestData.DigimonList[0];
Digimon opponent = TestData.DigimonList[1];

Battle battle = new Battle(player, opponent);

string battleLog = "";
bool firstTurn = true;

while (!battle.IsFinished)
{
    // Player chooses a move
    Move? playerMove = BattleView.ChooseMove(
        TestData.MoveList,
        player,
        player,
        opponent,
        battle.Round,
        battleLog,
        !firstTurn
    );

    if (playerMove == null)
    {
        battle.IsFinished = true;
        battle.Winner = opponent;
        break;
    }


    // Player attacks
    int playerDamage = battle.Attack(
        player,
        opponent,
        playerMove
    );

    battleLog =
        $"{player.Name} used {playerMove.Name}! " +
        $"{opponent.Name} took {playerDamage} damage!";

    BattleView.ShowBattleResult(
        player,
        opponent,
        battle.Round,
        battleLog
    );


    // Stop if the opponent was defeated
    if (battle.IsFinished)
    {
        break;
    }


    // Opponent chooses a move
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


    // Opponent attacks
    int opponentDamage = battle.Attack(
        opponent,
        player,
        opponentMove
    );

    battleLog =
        $"{opponent.Name} used {opponentMove.Name}! " +
        $"{player.Name} took {opponentDamage} damage!";

    BattleView.ShowBattleResult(
        player,
        opponent,
        battle.Round,
        battleLog
    );


    // Stop if the player was defeated
    if (battle.IsFinished)
    {
        break;
    }


    // Start the next round
    firstTurn = false;
    battle.Round++;
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