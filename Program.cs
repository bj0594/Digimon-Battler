while (true)
{
    // Get the Digimon used in the battle
    Digimon player = TestData.DigimonList[0];
    Digimon opponent = TestData.DigimonList[1];

    // Reset HP and SP before starting a new battle
    player.CurrentHp = player.MaxHp;
    player.CurrentSp = player.MaxSp;

    opponent.CurrentHp = opponent.MaxHp;
    opponent.CurrentSp = opponent.MaxSp;

    // Create a new battle
    Battle battle = new Battle(
        player,
        opponent
    );

    string battleLog = "";
    bool firstTurn = true;


    // Main battle loop
    while (!battle.IsFinished)
    {
        // Player chooses a Move or action
        var playerChoice = BattleView.ChooseMove(
            TestData.MoveList,
            player,
            player,
            opponent,
            battle.Round,
            battleLog,
            !firstTurn
        );

        Move? playerMove = playerChoice.Move;


        // Player chooses to flee
        if (playerChoice.Fled)
        {
            battle.IsFinished = true;
            battle.Winner = opponent;
            break;
        }


        // Player has no available Moves
        if (playerMove == null)
        {
            battle.IsFinished = true;
            battle.Winner = opponent;
            break;
        }


        // Player attacks first
        int playerDamage = battle.Attack(
            player,
            opponent,
            playerMove
        );

        battleLog =
            $"{player.Name} used {playerMove.Name}! " +
            $"{opponent.Name} took {playerDamage} damage!";


        // Show the player's attack result
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


        // Opponent chooses a random available Move
        Move? opponentMove = ChooseOpponentMove(
            TestData.MoveList,
            opponent
        );


        // Opponent has no available Moves
        if (opponentMove == null)
        {
            battle.IsFinished = true;
            battle.Winner = player;
            break;
        }


        // Opponent attacks after the player continues
        int opponentDamage = battle.Attack(
            opponent,
            player,
            opponentMove
        );

        battleLog =
            $"{opponent.Name} used {opponentMove.Name}! " +
            $"{player.Name} took {opponentDamage} damage!";


        // Show the opponent's attack result
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


    // Show the appropriate end-of-battle screen
    if (battle.Winner == player)
    {
        bool playAgain = BattleView.ShowVictoryScreen();

        if (!playAgain)
        {
            Console.Clear();
            break;
        }
    }
    else
    {
        bool playAgain = BattleView.ShowDefeatScreen();

        if (!playAgain)
        {
            Console.Clear();
            break;
        }
    }
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