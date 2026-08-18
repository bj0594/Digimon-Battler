# Digimon Battler

A small terminal-based Digimon battle game made with C# and .NET.

## About

This project uses the DigiDB Digimon and Move datasets from CSV files.

The data is read into C# objects and queried with LINQ. Instead of making
a traditional data viewer, I used the dataset as the foundation for a
simple playable battle system.

The player chooses an Attribute, picks a Digimon, chooses an opponent and
then fights using available Moves. HP and SP are tracked throughout the
battle.

## How it works

The program starts by reading both CSV files with `CsvReader`.

The CSV rows are mapped to two models:

- `Digimon` — contains the Digimon's name, stage, Attribute and stats.
- `Move` — contains the Move's name, Attribute, type, power, SP cost and
  whether it is inheritable.

LINQ is used to filter and sort the data. For example:

List<string> attributes = digimonList
    .Select(digimon => digimon.Attribute)
    .Distinct()
    .OrderBy(attribute => attribute)
    .ToList();

`Where()` is used to find Digimon with a specific Attribute, valid
opponents, and Moves available to each Digimon.

`OrderBy()`, `Distinct()` and `Any()` are also used throughout the
application.

## Battle system

The battle system is handled by the `Battle` class.

Physical Moves use Attack and Magic Moves use Intelligence. Damage is
calculated from the attacker's stat, the Move's Power and the defender's
Defense.

A Move can only be used when the Digimon has enough SP.

The player can also view both Digimon's stats or flee from battle.

## Project structure

```text
Data/
├── DigiDB_digimonlist.csv
└── DigiDB_movelist.csv

Battle.cs
BattleView.cs
BattleView.EndScreens.cs
BattleView.Info.cs
BattleView.Rendering.cs
CsvReader.cs
Digimon.cs
Move.cs
Program.cs
```

The main responsibilities are:

- `Program.cs` — controls the application and battle loop.
- `CsvReader.cs` — reads and maps the CSV files.
- `Digimon.cs` / `Move.cs` — data models.
- `Battle.cs` — battle and damage logic.
- `BattleView*.cs` — menus, input and terminal presentation.

## Program flow

```text
Read CSV files
      ↓
Choose Attribute
      ↓
Choose Digimon
      ↓
Choose opponent
      ↓
Confirm battle
      ↓
Filter available Moves
      ↓
Battle
      ↓
Victory / Defeat
      ↓
Play again or quit
```

## Data and LINQ

The project demonstrates several ways of working with the CSV dataset:

- `File.ReadAllLines()` reads the CSV files.
- `Split()` is used to separate CSV values.
- `Select()` retrieves properties from the dataset.
- `Where()` filters the data.
- `OrderBy()` sorts results.
- `Distinct()` removes duplicate Attributes.
- `Any()` checks whether matching data exists.

## Running the project

Make sure the CSV files are inside the `Data` folder and run:

```bash
dotnet run
```