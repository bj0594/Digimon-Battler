# Digimon Battler

A terminal-based Digimon battle application built with C#.

## About

Digimon Battler is an interactive console application where the user
can select Digimon and battle opponents using moves from the DigiDB
CSV datasets.

The project explores CSV handling, C# object modelling and LINQ
through an interactive application rather than a traditional data
viewer.

## Goal

The goal is to:

- Read and process CSV data
- Map CSV rows to C# objects
- Use LINQ to query and manipulate the data
- Build an interactive terminal application
- Use the dataset as the foundation for a simple battle system

## Data

The project uses the DigiDB datasets:

- `digimonlist.csv` — Digimon attributes and stats
- `movelist.csv` — Move attributes, power and SP cost

The `Attribute` field will be used to connect Digimon with relevant
moves in the battle system.

## Planned Features

- Browse and search Digimon
- Select a player Digimon
- Select an opponent
- Select available moves
- Use HP and SP during battles
- Calculate damage using Digimon stats and Move Power
- Determine the winner
- Use LINQ for filtering, sorting and statistics

## LINQ

LINQ will be used to:

- Find and filter Digimon
- Find moves matching a Digimon's Attribute
- Sort Digimon and moves by their stats
- Find strongest/weakest values
- Calculate statistics and comparisons

## Project Plan

1. Set up project and datasets
2. Create C# models
3. Read and map CSV data
4. Implement LINQ queries
5. Build battle system
6. Build interactive terminal interface
7. Test and refine

## Technologies

- C#
- .NET
- LINQ
- CSV
- Console Application