public class RocketDecorator : AirplaneDecorator
{
    public RocketDecorator(Airplane airplane)
    {
        WrapperAirplane = airplane;
        Name = WrapperAirplane.Name;
        Damage = WrapperAirplane.Damage;
        Speed = WrapperAirplane.Speed;
    }
    
    public override string GetDescription()
    {
        return WrapperAirplane.GetDescription() + RocketDescription();
    }
    
    private string RocketDescription() => ", add new module rocket";
}