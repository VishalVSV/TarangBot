using System;
using System.Collections.Generic;

namespace TarangBot.MessagingUtils
{
    public class MessageQueueHandler
    {
        private Queue<Message> msg_queue = new Queue<Message>();
        private Dictionary<string, List<MessageHandler>> events = new Dictionary<string, List<MessageHandler>>();

        public Action<string> Log;

        /// <summary>
        /// Delegate that defines the handler template for the on listen paradigm
        /// </summary>
        /// <param name="parameters">The parameters that the dispatcher provided</param>
        public delegate void MessageHandler(object[] parameters);

        public Action<Message> OnDispatch;

        public void On(string EventName,MessageHandler callback)
        {
            if (events.ContainsKey(EventName))
                events[EventName].Add(callback);
            else events.Add(EventName, new List<MessageHandler>() { callback });
        }

        public void Dispatch(string EventName,params object[] parameters)
        {
            var msg = Message.ConstructMessage(EventName, parameters);
            OnDispatch?.Invoke(msg);
            msg_queue.Enqueue(msg);
        }

        public void HandleEvents()
        {
            while (msg_queue.Count > 0)
            {
                Message msg = msg_queue.Dequeue();

                if (events.ContainsKey(msg.EventName))
                {
                    int c = events[msg.EventName].Count;
                    for (int i = 0; i < c; i++)
                    {
                        events[msg.EventName][i](msg.Parameters);
                    }
                }
                else
                {
                    Log?.Invoke($"Event {msg.EventName} is not being listened for!");
                }
            }
        }
    }
}
