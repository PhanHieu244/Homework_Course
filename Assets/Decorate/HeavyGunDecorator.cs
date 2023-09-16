public class HeavyGunDecorator : AirplaneDecorator
{
    private float _increaseDamage = 10;
    
    public HeavyGunDecorator(Airplane airplane)
    {
        WrapperAirplane = airplane;
        Name = WrapperAirplane.Name;
        Damage = WrapperAirplane.Damage + _increaseDamage;
        Speed = WrapperAirplane.Speed;
    }

    public override string GetDescription()
    {
        return WrapperAirplane.GetDescription() + HeavyGunDescription();
    }
    
    private string HeavyGunDescription() => $", add HeavyGun to increase damage to {Damage}";
}