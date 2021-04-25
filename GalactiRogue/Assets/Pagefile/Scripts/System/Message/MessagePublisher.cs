using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.System.Messaging;


namespace Pagefile.System
{
    // MessagePublisher
    // Used to publish messages to any number of subscribers to send data to other
    // classes with loose coupling. Uses coroutines and yield statements to process
    // messages rather than a monobehaviour so it doesn't need a game object to process
    public class MessagePublisher
    {
        static private MessagePublisher _instance = null;
        private MessagePublisher(){}

        #region Public Members
        public delegate void PublishedMessageHandler(Message msg);
        #endregion

        #region Private Members
        private Dictionary<global::System.Type, PublishedMessageHandler> _messageHandlers = new Dictionary<global::System.Type, PublishedMessageHandler>();
        // These messages are handled at the end of the frame
        private List<Message> _messageQueue = new List<Message>();
        #endregion

        // TODO: Maybe switch to use .NET's Lazy<> class? Do I need to worry about thread
        // safety? Ooooooorrr...does it even need an instance? Can I make this a static class?
        static public MessagePublisher Instance
        {
            get 
            {
                if(_instance == null)
                {
                    _instance = new MessagePublisher();
                    GameObject obj = new GameObject();
                    Object.DontDestroyOnLoad(obj);
                    MessageProcessor msgProc = obj.AddComponent<MessageProcessor>();
                    msgProc.messageDelegate = _instance.ProcessMessages;
                }
                return _instance;
            }
        }

        #region Public Methods
        public void Subscribe(global::System.Type messageType, PublishedMessageHandler messageHandler)
        {
            PublishedMessageHandler handler = null;
            if(!_messageHandlers.TryGetValue(messageType, out handler))
            {
                handler += messageHandler;
                _messageHandlers.Add(messageType, handler);
            }
            else
            {
                handler += messageHandler;
            }
        }

        public void Unsubscribe(global::System.Type messageType, PublishedMessageHandler messageHandler)
        {
            // TODO: This will probably throw an exception if no one has subscribed
            // to the message before
            PublishedMessageHandler handler = _messageHandlers[messageType];
            handler -= messageHandler;
            if(handler == null)
            {
                _messageHandlers.Remove(messageType);
            }
        }

        // Adds a message to be processed in LateUpdate()
        public void PublishMessage(Message msg)
        {
            _messageQueue.Add(msg);
        }

        // Processes a message immediately
        public void PublishImmediate(Message msg)
        {
            PublishedMessageHandler handler;
            _messageHandlers.TryGetValue(msg.MessageType, out handler);
            handler?.Invoke(msg);
        }
        #endregion

        #region Private Methods
        private void ProcessMessages()
        {
            foreach(Message msg in _messageQueue)
            {
                PublishedMessageHandler handler;
                _messageHandlers.TryGetValue(msg.MessageType, out handler);
                handler?.Invoke(msg);
            }
            _messageQueue.Clear();
        }
        #endregion

        private class MessageProcessor : MonoBehaviour
        {
            public delegate void QueueDelegate();
            public QueueDelegate messageDelegate = empty;

            private void LateUpdate()
            {
                messageDelegate();
            }

            private static void empty() { }
        }
    }
}
