public static class TestData
{
    // Static lists to hold sample Digimon and Move data for testing
    public static List<Digimon> DigimonList = new()
    {
        new Digimon
        {
            Name = "Agumon",
            Attribute = "Fire",
            MaxHp = 1030,
            CurrentHp = 1030,
            MaxSp = 59,
            CurrentSp = 59,
            Attack = 131,
            Defense = 103,
            Intelligence = 54,
            Speed = 86
        },

        new Digimon
        {
            Name = "Gomamon",
            Attribute = "Water",
            MaxHp = 1160,
            CurrentHp = 1160,
            MaxSp = 69,
            CurrentSp = 69,
            Attack = 93,
            Defense = 93,
            Intelligence = 81,
            Speed = 79
        }
    };

    // Static list to hold sample Move data for testing
    public static List<Move> MoveList = new()
    {
        new Move
        {
            Name = "Wolkenapalm I",
            Attribute = "Fire",
            Type = "Physical",
            SpCost = 3,
            Power = 65
        },

        new Move
        {
            Name = "Wolkenapalm III",
            Attribute = "Fire",
            Type = "Physical",
            SpCost = 9,
            Power = 105
        },

        new Move
        {
            Name = "Burst Flame I",
            Attribute = "Fire",
            Type = "Magic",
            SpCost = 3,
            Power = 55
        },

        new Move
        {
            Name = "Ice Archery I",
            Attribute = "Water",
            Type = "Physical",
            SpCost = 3,
            Power = 65
        },

        new Move
        {
            Name = "Ice Archery III",
            Attribute = "Water",
            Type = "Physical",
            SpCost = 9,
            Power = 105
        },

        new Move
        {
            Name = "Hydro Water I",
            Attribute = "Water",
            Type = "Magic",
            SpCost = 3,
            Power = 55
        }
    };
}