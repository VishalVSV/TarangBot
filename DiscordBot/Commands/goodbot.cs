using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    [NoHelp]
    public class goodbot : ICommand
    {
        public EmbedBuilder DescriptiveHelpText()
        {
            throw new NotImplementedException();
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            await msg.Channel.SendMessageAsync($"Thank you for the praise uWu\n\n`I have been appreciated {++Tarang.Data.GoodBotScore} times!`");
        }

        public string HelpText()
        {
            throw new NotImplementedException();
        }
    }
}
