using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    public class Help : ICommand
    {
        public EmbedBuilder DescriptiveHelpText()
        {
            return ConstructHelp("Help", "Displays a list of commands and usage of each command", "help", "Shows a list of commands", "help <command_name>", "Shows a detailed explaination of the specified command");
        }

        public static EmbedBuilder ConstructHelp(string name, string desc, params string[] usage)
        {
            EmbedBuilder embed = new EmbedBuilder();

            embed.AddField("Description", desc);

            StringBuilder s = new StringBuilder();

            for (int i = 0; i < usage.Length; i++)
            {
                s.AppendLine($"`{Tarang.Data.DiscordBotPrefix}{usage[i]}`\n   {usage[i + 1]}");
                i++;
            }

            embed.AddField(name, s.ToString());

            return embed;
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler cmd)
        {
            if (msg.Content.Split(' ').Length == 1)
            {
                EmbedBuilder builder = new EmbedBuilder();

                builder.WithTitle("Help");

                StringBuilder cmds = new StringBuilder();
                foreach (string cmd_str in cmd.commands.Keys)
                {
                    if (cmd.commands[cmd_str].GetCustomAttribute<NoHelp>() == null)
                        cmds.AppendLine($"`{Tarang.Data.DiscordBotPrefix}{cmd_str}` - { cmd.GetHelp(cmd.commands[cmd_str]) }");
                }

                builder.AddField("Commands", cmds.ToString());

                await msg.Channel.SendMessageAsync(null, false, builder.Build());
            }
            else
            {
                string cmd_name = msg.Content.Split(' ')[1];

                if(cmd_name.ToLower() == "me")
                {
                    await msg.Channel.SendMessageAsync("No one can...");
                }
                if (cmd.commands.ContainsKey(cmd_name))
                {
                    if (cmd.commands[cmd_name].GetCustomAttribute<NoHelp>() == null)
                    {
                        EmbedBuilder help = cmd.GetDescHelp(cmd.commands[cmd_name]);
                        help.WithTitle("Help");

                        await msg.Channel.SendMessageAsync(null, false, help.Build());
                    }
                }
                else
                {
                    await msg.Channel.SendMessageAsync($"Command {cmd_name} not found!");
                }
            }
        }

        public string HelpText()
        {
            return "This command, shows a list of the commands";
        }
    }
}
