using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    [NoHelp]
    public class mkdashboard : ICommand
    {
        public EmbedBuilder DescriptiveHelpText()
        {
            throw new NotImplementedException();
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            if (!(msg.Author as SocketGuildUser).GuildPermissions.Administrator)
                return;

            var msg0 = (await msg.Channel.SendMessageAsync(null, false, Tarang.Data.TarangBot.ConstructDashboard()));
            Tarang.Data.DashboardMessageId = msg0.Id;
            Tarang.Data.DashboardChannel = msg.Channel.Id;
        }

        public string HelpText()
        {
            throw new NotImplementedException();
        }
    }
}
