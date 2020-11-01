using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace TarangBot.DiscordBot
{
    public class CommandHandler
    {
        public Dictionary<string, Type> commands = new Dictionary<string, Type>();

        public CommandHandler(string Command_Namespace)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where((t) => t.Namespace == Command_Namespace).ToArray();

            Type ICommandType = typeof(ICommand);

            for (int i = 0; i < types.Length; i++)
            {
                if(ICommandType.IsAssignableFrom(types[i]))
                {
                    commands.Add(types[i].Name.ToLower(), types[i]);
                }
            }
        }

        public void Handle(SocketMessage msg)
        {

        }
    }

    public interface ICommand
    {
        Task HandleCommand(SocketMessage msg,CommandHandler commandHandler);

        string HelpText();

        string DescriptiveHelpText();
    }
}
