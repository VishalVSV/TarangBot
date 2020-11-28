using Discord.WebSocket;
using System.Collections.Generic;

namespace TarangBot.DiscordBot
{
    public class PollSystem
    {
        public ulong RecvChannel, PollChannel;
        public string id;
        public List<Poll> polls = new List<Poll>();

        public void Init()
        {
            Tarang.Data.MessageQueue.On("MessageReceived", (o) =>
             {
                 SocketMessage msg = (SocketMessage)o[0];
                 if(msg.Content.StartsWith(Tarang.Data.DiscordBotPrefix + "pollsetup recv"))
                 {
                     string poll_id = msg.Content.Substring((Tarang.Data.DiscordBotPrefix + "pollsetup recv").Length).Trim();
                     if(poll_id == id)
                     {
                         RecvChannel = msg.Channel.Id;
                     }
                 }
                 else if (msg.Content.StartsWith(Tarang.Data.DiscordBotPrefix + "pollsetup poll"))
                 {
                     string poll_id = msg.Content.Substring((Tarang.Data.DiscordBotPrefix + "pollsetup poll").Length).Trim();
                     if (poll_id == id)
                     {
                         PollChannel = msg.Channel.Id;
                     }
                 }
             });
        }
    }

    public class Poll
    {
        public ulong MessageId;
        public long left, right;
    }
}
