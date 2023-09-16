public class Lightning : Airplane
{
    public Lightning()
    {
        Name = "Lightning";
        Damage = 10;
        Speed = 10;
    }

    public override string GetDescription() =>  $"{Name} has {Damage} damages and {Speed} speed";
}