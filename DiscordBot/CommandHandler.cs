using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Newtonsoft.Json;

namespace TarangBot.DiscordBot
{
    public class CommandHandler
    {
        public Dictionary<string, Type> commands = new Dictionary<string, Type>();

        [JsonIgnore]
        public Dictionary<ulong, ICommand> current_command = new Dictionary<ulong, ICommand>();

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

            if (!current_command.ContainsKey(msg.Author.Id))
                current_command.Add(msg.Author.Id, null);

            if (current_command[msg.Author.Id] != null)
            {
                current_command[msg.Author.Id].HandleCommand(msg, this);
            }
            else
            {
                if (msg.Content.Split('\n', ' ')[0].Length >= Tarang.Data.DiscordBotPrefix.Length)
                {
                    string cmd_name = msg.Content.Split('\n', ' ')[0].Substring(Tarang.Data.DiscordBotPrefix.Length);

                    if (commands.ContainsKey(cmd_name))
                    {
                        ICommand cmd = (ICommand)Activator.CreateInstance(commands[cmd_name]);

                        cmd.HandleCommand(msg, this);
                    }
                    else
                    {
                        if (msg.Content.StartsWith(Tarang.Data.DiscordBotPrefix))
                            msg.Channel.SendMessageAsync("Command not found");
                    }
                }
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
