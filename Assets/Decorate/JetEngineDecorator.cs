public class JetEngineDecorator : AirplaneDecorator
{
    private float _increaseSpeed = 5;
    
    public JetEngineDecorator(Airplane airplane)
    {
        WrapperAirplane = airplane;
        Name = WrapperAirplane.Name;
        Damage = WrapperAirplane.Damage;
        Speed = WrapperAirplane.Speed + _increaseSpeed;
    }

    public override string GetDescription()
    {
        return WrapperAirplane.GetDescription() + JetEngineDescription();
    }

    private string JetEngineDescription() => $", add JetEngine to increase speed to {Speed}";
}