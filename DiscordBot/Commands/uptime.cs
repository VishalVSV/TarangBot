using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    [NoHelp]
    public class uptime : ICommand
    {
        public EmbedBuilder DescriptiveHelpText()
        {
            throw new NotImplementedException();
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            if (!(msg.Author as SocketGuildUser).GuildPermissions.Administrator)
                return;

            string time = "";

            var t = (DateTime.Now - Tarang.StartTime);

            if (t.Days > 0)
                time += $"{t.Days} Days and ";

            if (t.Hours > 0)
                time += $"{t.Hours} hours and ";

            if (t.Minutes > 0)
                time += $"{t.Minutes} minutes and ";

            time += $"{t.Seconds} seconds";

            await msg.Channel.SendMessageAsync($"TarangBot has been live for {time}");
        }

        public string HelpText()
        {
            throw new NotImplementedException();
        }
    }
}
