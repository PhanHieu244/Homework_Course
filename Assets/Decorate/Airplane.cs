using UnityEngine;

public abstract class Airplane
{
    public string Name { get; protected set;}
    public float Damage { get; protected set;}
    public float Speed { get; protected set;}
    
    public abstract string GetDescription();
}
