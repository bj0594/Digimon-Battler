public class Digimon
{
    // Basic Digimon information.
    public string Name { get; set; }
    public string Attribute { get; set; }

    // Health and resource values.
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }

    public int MaxSp { get; set; }
    public int CurrentSp { get; set; }

    // Combat stats.
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Intelligence { get; set; }
    public int Speed { get; set; }
}