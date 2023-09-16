public class Thunderbolt : Airplane
{

    public Thunderbolt()
    {
        Name = "Thunderbolt";
        Damage = 5;
        Speed = 5;
    }

    public override string GetDescription() => $"{Name} has {Damage} damages and {Speed} speed";
}