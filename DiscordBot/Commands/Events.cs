using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TarangBot.TarangEvent;

namespace TarangBot.DiscordBot.Commands
{
    public class Events : ICommand
    {

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            Participant participant = Tarang.Data.GetParticipant(msg.Author.Username + "#" + msg.Author.Discriminator);

            if(participant != null)
            {
                EmbedBuilder builder = new EmbedBuilder().WithTitle($"{msg.Author.Username} - Registered Events");

                StringBuilder events = new StringBuilder();

                for (int i = 0; i < participant.Registered_Events.Count; i++)
                {
                    Event @event = Tarang.Data.GetEventById(participant.Registered_Events[i]);
                    events.AppendLine($"{@event.Names[0]}");
                }

                builder.WithDescription(events.ToString());

                await msg.Channel.SendMessageAsync(null, false, builder.Build());
            }
            else
            {
                await msg.Channel.SendMessageAsync("Either you aren't a participant or something went wrong...");
            }

        }

        public string HelpText()
        {
            return "Find the events you are registered for";
        }

        public EmbedBuilder DescriptiveHelpText()
        {
            return Help.ConstructHelp("Events", "Find the events you are registered for", "events", "Displays the list of events that you are registered for");
        }
    }
}
