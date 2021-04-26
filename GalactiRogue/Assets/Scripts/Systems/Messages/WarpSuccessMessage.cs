using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Systems.Messaging;

public class WarpSuccessMessage : Message
{
    private float _warpFuelUsed = 0f;
    public float WarpFuelUsed => _warpFuelUsed;

    public WarpSuccessMessage(Object sender, float fuelUsed)
    {
        _messageType = typeof(WarpSuccessMessage);
        _sender = sender;
        _warpFuelUsed = fuelUsed;
    }
}
