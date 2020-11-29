using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    [NoHelp]
    public class Mlg : ICommand
    {
        public EmbedBuilder DescriptiveHelpText()
        {
            throw new NotImplementedException();
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            await msg.Channel.SendMessageAsync("https://tenor.com/view/wow-cat-thug-life-doritos-mountain-dew-gif-14737452");
        }

        public string HelpText()
        {
            throw new NotImplementedException();
        }
    }
}
