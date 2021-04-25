using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.System.Messaging;

public class POIClearMessage : Message
{
    public POIClearMessage(Object sender)
    {
        _messageType = typeof(POIClearMessage);
        _sender = sender;
    }
}
