using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    public class Help : ICommand
    {
        public string DescriptiveHelpText()
        {
            throw new NotImplementedException();
        }

        public Task HandleCommand(SocketMessage msg,CommandHandler cmd)
        {


            return Task.CompletedTask;
        }

        public string HelpText()
        {
            return "This command lol";
        }
    }
}
