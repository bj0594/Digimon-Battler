public class Digimon
{
    // Properties to hold the Digimon's attributes
    public string Name { get; set; }
    public string Attribute { get; set; }

    // Properties to hold the Digimon's stats
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }

    public int MaxSp { get; set; }
    public int CurrentSp { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Intelligence { get; set; }
    public int Speed { get; set; }
}