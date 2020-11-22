using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TarangBot.TarangEvent;

namespace TarangBot.DiscordBot.Commands
{
    public class Rules : ICommand
    {
        private string last_event_name;

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            string event_name = "";

            if (msg.Content.Length >= Tarang.Data.DiscordBotPrefix.Length + 5) event_name = msg.Content.Substring(Tarang.Data.DiscordBotPrefix.Length + 5).Trim();

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

            Event @event = Tarang.Data.GetEvent(event_name, out int edits);

            if (edits < 3)
            {
                await msg.Channel.SendMessageAsync(null, false, @event);
            }
            else if (edits < 5)
            {
                await msg.Channel.SendMessageAsync($"Could not find event {event_name}. Did you mean {@event.Names[0]}?");
                commandHandler.current_command[msg.Author.Id] = this;
                last_event_name = @event.Names[0];
                return;
            }
            else
            {
                await msg.Channel.SendMessageAsync($"Did not find any event called {event_name}!");
            }

            commandHandler.current_command[msg.Author.Id] = null;

        }

        public string HelpText()
        {
            return "Explains the rules and other details of an event";
        }

        public EmbedBuilder DescriptiveHelpText()
        {
            return Help.ConstructHelp("Rules", HelpText(), "rules <event_name>", "Shows the rules and details of the particular event");
        }
    }
}
