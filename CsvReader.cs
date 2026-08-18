public static class CsvReader
{
    // Reads Digimon data from the CSV file.
    public static List<Digimon> ReadDigimon(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath); 

        List<Digimon> digimonList = new(); 

        // Skip the first line because it contains the column names.
        foreach (string line in lines.Skip(1))
        {
            string[] values = line.Split(',');

            Digimon digimon = new Digimon
            {
                Name = values[1],
                Stage = values[2],
                Attribute =
                    values[4] == "Thunder"
                        ? "Electric"
                        : values[4],

                MaxHp = int.Parse(values[7]),
                MaxSp = int.Parse(values[8]),
                Attack = int.Parse(values[9]),
                Defense = int.Parse(values[10]),
                Intelligence = int.Parse(values[11]),
                Speed = int.Parse(values[12])
            };

            digimon.CurrentHp = digimon.MaxHp;
            digimon.CurrentSp = digimon.MaxSp;

            digimonList.Add(digimon);
        }

        return digimonList;
    }


    // Reads Move data from the CSV file.
    public static List<Move> ReadMoves(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        List<Move> moveList = new();

        // Skip the first line because it contains the column names.
        foreach (string line in lines.Skip(1))
        {
            string[] values = line.Split(',');

            Move move = new Move
            {
                Name = values[0],
                SpCost = int.Parse(values[1]),
                Type = values[2],
                Power = int.Parse(values[3]),
                Attribute = values[4],
                Inheritable = values[5] == "Yes"
            };

            moveList.Add(move);
        }

        return moveList;
    }
}