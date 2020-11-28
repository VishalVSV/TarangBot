using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TarangBot.TarangEvent;

namespace TarangBot.DiscordBot.Commands
{
    public class Spectate : ICommand
    {

        private string last_event_name = null;

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            if (commandHandler.current_command[msg.Author.Id] != null)
                commandHandler.current_command[msg.Author.Id] = null;

            if ((msg.Author as SocketGuildUser).VoiceChannel == null)
            {
                await msg.Channel.SendMessageAsync($"Join a VC first then run the spectate command");
                return;
            }

            string event_name = "";

            if (msg.Content.Length > Tarang.Data.DiscordBotPrefix.Length + "spectate".Length)
            {
                event_name = msg.Content.Substring(Tarang.Data.DiscordBotPrefix.Length + "spectate".Length).Trim();
            }

            var msg0 = msg.Content.ToLower().Trim();
            if (last_event_name != null && (msg0 == "yes" || msg0 == "y" || msg0 == "s"))
            {
                commandHandler.current_command[msg.Author.Id] = null;
                event_name = last_event_name;
            }

            if (last_event_name != null && (msg0 == "no" || msg0 == "n"))
            {
                await msg.Channel.SendMessageAsync("ok (;-;)");
                commandHandler.current_command[msg.Author.Id] = null;
                return;
            }

            int edits;
            Event event_ = Tarang.Data.GetEvent(event_name, out edits);

            if (edits < 3)
            {
                SocketVoiceChannel WaitingRoomVC = event_.WaitingVC;
                if (WaitingRoomVC != null)
                {
                    await msg.Channel.SendMessageAsync($"Moving {(string.IsNullOrEmpty((msg.Author as SocketGuildUser).Nickname) ? msg.Author.Username : (msg.Author as SocketGuildUser).Nickname)} to {event_.Names[0]}");
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
            else if (edits < 5)
            {
                await msg.Channel.SendMessageAsync($"Could not find event {event_name}. Did you mean {event_.Names[0]}?");
                commandHandler.current_command[msg.Author.Id] = this;
                last_event_name = event_.Names[0];
            }
            else
            {
                await msg.Channel.SendMessageAsync($"Did not find any event called {event_name}!");
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
