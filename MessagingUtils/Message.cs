using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.MessagingUtils
{
    public struct Message
    {
        public string EventName;
        public object[] Parameters;

        public static Message ConstructMessage(string EventName,object[] parameters)
        {
            Message message = new Message();
            message.EventName = EventName;message.Parameters = parameters;
            return message;
        }
    }
}
