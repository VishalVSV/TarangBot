using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TarangBot.DiscordBot.Commands
{
    public class Headpat : ICommand
    {
        public EmbedBuilder DescriptiveHelpText()
        {
            return Help.ConstructHelp("Headpat", "I don't know why this is here and why you spent time running this command", "headpat", "That's it lol");
        }

        public async Task HandleCommand(SocketMessage msg, CommandHandler commandHandler)
        {
            await msg.Channel.SendMessageAsync("Here you go, uwu\nhttps://tenor.com/view/patpat-pat-comfort-pat-on-the-head-there-there-gif-10534102");
        }

        public string HelpText()
        {
            return "Gives you emotional support with a 0.01% chance of crashing the bot";
        }
    }
}
