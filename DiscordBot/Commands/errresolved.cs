using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    [NoHelp]
    public class errresolved : ICommand
    {
        public EmbedBuilder DescriptiveHelpText()
        {
            throw new NotImplementedException();
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            if (!(msg.Author as SocketGuildUser).GuildPermissions.Administrator)
                return;

            Tarang.Data.LastError = "";
            await msg.Channel.SendMessageAsync("Last error marked as resolved");
            await Tarang.Data.TarangBot.UpdateDashboard();
        }

        public string HelpText()
        {
            throw new NotImplementedException();
        }
    }
}
