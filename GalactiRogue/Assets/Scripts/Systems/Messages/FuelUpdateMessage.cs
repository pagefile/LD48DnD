using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Systems.Messaging;

public class FuelUpdateMessage : Message
{
    protected float _totalFuel;
    public float TotalFuel => _totalFuel;

    public FuelUpdateMessage(Object sender, float fuel)
    {
        _messageType = typeof(FuelUpdateMessage);
        _sender = sender;
        _totalFuel = fuel;
    }
}
