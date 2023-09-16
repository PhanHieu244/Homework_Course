using System;
using UnityEngine;

public class AirplaneGenerator : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("First Airplane");
        Airplane thunderbolt = new Thunderbolt();
        thunderbolt = new RocketDecorator(thunderbolt);
        thunderbolt = new HeavyGunDecorator(thunderbolt);
        thunderbolt = new JetEngineDecorator(thunderbolt);
        Debug.Log(thunderbolt.GetDescription());

        Debug.Log("Second Airplane");
        Airplane lightning = new Lightning();
        lightning = new RocketDecorator(lightning);
        lightning = new JetEngineDecorator(lightning);
        Debug.Log(lightning.GetDescription());
    }
}