using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Systems.Messaging;


namespace Pagefile.Systems
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
        private Queue<Message> _messageQueue = new Queue<Message>();
        // _messageQueue is passed to _handlerQueue to keep infinite messages from piling up during message handling
        private Queue<Message> _handlerQueue = default;
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
            _messageQueue.Enqueue(msg);
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
            Message msg = default;
            _handlerQueue = _messageQueue;
            _messageQueue = new Queue<Message>();   // If performance becomes an issue we can keep three queues around to swap between
            while(_handlerQueue.Count > 0)  // I can't do clever things like while(_messageQueue.TryDequeue(msg)) because Unity doesn't have that method >:(
            {
                msg = _handlerQueue.Dequeue();
                PublishedMessageHandler handler;
                _messageHandlers.TryGetValue(msg.MessageType, out handler);
                try
                {
                    handler?.Invoke(msg);
                }
                catch(System.Exception e)
                {
                    Debug.LogError($"Exception during message handling in {e.TargetSite}!\n{e.Message}\n{e.StackTrace}");
                }
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
