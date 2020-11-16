using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TarangBot.TarangEvent;

namespace TarangBot.DiscordBot.Commands
{
    public class Spectate : ICommand
    {

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            string event_name = msg.Content.Substring(Tarang.Data.DiscordBotPrefix.Length + "spectate".Length).Trim();

            int edits;
            Event event_ = Tarang.Data.GetEvent(event_name, out edits);

            if (edits < 3)
            {
                SocketVoiceChannel WaitingRoomVC = event_.WaitingVC;
                if (WaitingRoomVC != null)
                {
                    await msg.Channel.SendMessageAsync($"Moving {(string.IsNullOrEmpty((msg.Author as SocketGuildUser).Nickname) ? msg.Author.Username : (msg.Author as SocketGuildUser).Nickname)} to {WaitingRoomVC.Name}");
                    await (msg.Author as SocketGuildUser).ModifyAsync((a) =>
                    {
                        a.Channel = WaitingRoomVC;
                    });
                }
                else
                {
                    await msg.Channel.SendMessageAsync($"Couldn't find VC or event cannot be spectated");
                }
            }
            else
            {
                await msg.Channel.SendMessageAsync($"Could not find event {event_name}. Did you mean {event_.Names[0]}?");
            }


        }

        public string HelpText()
        {
            return "Join the waiting VC of an event";
        }
        public EmbedBuilder DescriptiveHelpText()
        {
            return Help.ConstructHelp("Spectate", "Join the waiting VC of an event", "spectate <event_name>", "Moves you to the VC allocated for this event");
        }
    }
}
