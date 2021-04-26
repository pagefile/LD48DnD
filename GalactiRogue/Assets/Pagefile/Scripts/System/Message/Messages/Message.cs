using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Pagefile.Systems.Messaging
{
    public abstract class Message
    {
        // TODO: Probably don't need this variable. GetType() can be used instead
        protected global::System.Type _messageType;
        // Not sure I really need this variable....The whole point of these messages is for
        // loose coupling
        protected Object _sender;
        public Object Sender => _sender;
        public global::System.Type MessageType => GetType();
    }
}
