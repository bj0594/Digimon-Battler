Digimon player = TestData.DigimonList[0];
Digimon opponent = TestData.DigimonList[1];

Move move = TestData.MoveList[0];

Battle battle = new Battle(player, opponent);

BattleView.DrawBattleScreen(
    player,
    opponent,
    battle.Round
);

Console.ReadKey();

battle.Attack(
    player,
    opponent,
    move
);

Console.Clear();

Console.WriteLine(
    $"{player.Name} used {move.Name}!"
);

Console.WriteLine(
    $"{opponent.Name} has {opponent.CurrentHp} HP remaining."
);

Console.WriteLine(
    $"{player.Name} has {player.CurrentSp} SP remaining."
);

Console.ReadKey();