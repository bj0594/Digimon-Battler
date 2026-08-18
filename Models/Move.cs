public class Move
{
    // Basic Move information.
    public string Name { get; set; }
    public string Attribute { get; set; }
    public string Type { get; set; }

    // Resource cost and damage power.
    public int SpCost { get; set; }
    public int Power { get; set; }

    // Indicates whether the Move can be inherited
    public bool Inheritable { get; set; }
}