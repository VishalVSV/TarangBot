using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

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
                if (ICommandType.IsAssignableFrom(types[i]))
                {
                    commands.Add(types[i].Name.ToLower(), types[i]);
                }
            }
        }

        public void Handle(SocketMessage msg)
        {
            if (msg.Author.IsBot)
                return;

            string cmd_name = msg.Content.Split('\n', ' ')[0].Substring(Tarang.Data.DiscordBotPrefix.Length);

            if (commands.ContainsKey(cmd_name))
            {
                ICommand cmd = (ICommand)Activator.CreateInstance(commands[cmd_name]);

                cmd.HandleCommand(msg, this);
            }
            else
            {
                msg.Channel.SendMessageAsync("Command not found");
            }
        }


        public string GetHelp(Type t)
        {
            return ((ICommand)Activator.CreateInstance(t)).HelpText();
        }

        public EmbedBuilder GetDescHelp(Type t)
        {
            return ((ICommand)Activator.CreateInstance(t)).DescriptiveHelpText();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class NoHelp : Attribute
    {

    }

    public interface ICommand
    {
        Task HandleCommand(SocketMessage msg, CommandHandler commandHandler);

        string HelpText();

        EmbedBuilder DescriptiveHelpText();
    }
}
