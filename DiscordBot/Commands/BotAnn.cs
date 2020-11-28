using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    [NoHelp]
    public class BotAnn : ICommand
    {
        public static WebClient client = new WebClient();

        public EmbedBuilder DescriptiveHelpText()
        {
            throw new NotImplementedException();
        }

        private Embed MakeEmbed(string s, string title = "Announcement")
        {
            return (new EmbedBuilder().WithTitle(title).WithDescription(s).WithColor(new Color(64, 224, 208))).Build();
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            if (!(msg.Author as SocketGuildUser).GuildPermissions.Administrator)
                return;


            SocketTextChannel channel = Tarang.Data.TarangBot._client.GetChannel(Tarang.Data.AnnouncementChannel) as SocketTextChannel;

            if (channel == null)
            {
                await msg.Channel.SendMessageAsync("Announcement channel not found or set");
                return;
            }

            if (msg.Content.IndexOf('\n') > 0)
            {
                string msg_content = msg.Content.Substring(msg.Content.IndexOf('\n') + 1);
                string title = "Announcement";

                if (msg.Content.Substring(0, msg.Content.IndexOf('\n') + 1).Split(' ').Length > 1)
                {
                    title = msg.Content.Substring(0, msg.Content.IndexOf('\n') + 1).Split(' ')[1].Trim();
                }

                if (msg.Attachments.Count == 0)
                {
                    await channel.SendMessageAsync(null, false, MakeEmbed(msg_content, title));
                }
                else
                {
                    Attachment attachment = msg.Attachments.ElementAt(0);

                    byte[] b = client.DownloadData(attachment.Url);

                    Stream res = new MemoryStream(b);

                    await channel.SendFileAsync(res, attachment.Filename, "", false, MakeEmbed(msg_content, title));
                }
            }
        }

        public string HelpText()
        {
            throw new NotImplementedException();
        }
    }
}
