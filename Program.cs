// Program reads both CSV files before running
List<Digimon> digimonList =
    CsvReader.ReadDigimon(
        "Data/DigiDB_digimonlist.csv"
    );

List<Move> moveList =
    CsvReader.ReadMoves(
        "Data/DigiDB_movelist.csv"
    );

while (true)
{
    // Let the player choose an Attribute first.
    string selectedAttribute = BattleView.ChooseAttribute(
        digimonList
    );

    // Let the player choose a Digimon with that Attribute.
    Digimon player = BattleView.ChooseDigimon(
        digimonList,
        selectedAttribute
    );

    Digimon opponent = BattleView.ChooseOpponent(
        digimonList,
        player
    );

    // Confirm the selected Digimon before starting the battle.
    if (!BattleView.ConfirmBattle(player, opponent))
    {
        continue;
    }

    // Get Moves matching the player's Attribute or Neutral Moves.
    List<Move> playerMoves = moveList
        .Where(move =>
            move.Attribute == player.Attribute ||
            move.Attribute == "Neutral")
        .ToList();

    // Get Moves matching the opponent's Attribute or Neutral Moves.
    List<Move> opponentMoves = moveList
        .Where(move =>
            move.Attribute == opponent.Attribute ||
            move.Attribute == "Neutral")
        .ToList();

    // Reset both Digimon for a new battle.
    player.CurrentHp = player.MaxHp;
    player.CurrentSp = player.MaxSp;

    opponent.CurrentHp = opponent.MaxHp;
    opponent.CurrentSp = opponent.MaxSp;


    // Create a new battle using the selected Digimon.
    Battle battle = new Battle(
        player,
        opponent
    );

    string battleLog = "";
    bool firstTurn = true;


    // Run the battle until one side wins, flees, or runs out of moves.
    while (!battle.IsFinished)
    {
        // Let the player choose a Move or battle action.
        var playerChoice = BattleView.ChooseMove(
            playerMoves,
            player,
            player,
            opponent,
            battle.Round,
            !firstTurn
        );

        Move? playerMove = playerChoice.Move;


        // Fleeing gives the victory to the opponent.
        if (playerChoice.Fled)
        {
            battle.IsFinished = true;
            battle.Winner = opponent;
            break;
        }


        // No Move means the player cannot continue.
        if (playerMove == null)
        {
            battle.IsFinished = true;
            battle.Winner = opponent;
            break;
        }


        // The player always attacks first.
        int playerDamage = battle.Attack(
            player,
            opponent,
            playerMove
        );

        BattleView.ShowDamageAnimation(
            player,
            opponent,
            opponent,
            battle.Round
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


        // The opponent cannot attack if they were defeated.
        if (battle.IsFinished)
        {
            break;
        }


        // Let the opponent choose a random affordable Move.
        Move? opponentMove = ChooseOpponentMove(
            opponentMoves,
            opponent
        );

        if (opponentMove == null)
        {
            battle.IsFinished = true;
            battle.Winner = player;
            break;
        }


        // The opponent attacks after the player continues.
        int opponentDamage = battle.Attack(
            opponent,
            player,
            opponentMove
        );

        BattleView.ShowDamageAnimation(
            player,
            opponent,
            player,
            battle.Round
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


        // The battle ends immediately if the player was defeated.
        if (battle.IsFinished)
        {
            break;
        }


        // Prepare for the next round.
        firstTurn = false;
        battle.Round++;
    }


    // Show the appropriate end screen.
    bool playAgain = battle.Winner == player
        ? BattleView.ShowVictoryScreen()
        : BattleView.ShowDefeatScreen();

    if (!playAgain)
    {
        Console.Clear();
        break;
    }
}


// Chooses a random Move the opponent can currently afford.
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