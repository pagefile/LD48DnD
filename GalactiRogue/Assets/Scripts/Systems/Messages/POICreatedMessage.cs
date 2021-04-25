using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.System.Messaging;

public class POICreatedMessage : Message
{
    protected GameObject _poi;
    public GameObject POI => _poi;

    public POICreatedMessage(Object sender, GameObject poi)
    {
        _messageType = typeof(POICreatedMessage);
        _sender = sender;
        _poi = poi;
    }
}
