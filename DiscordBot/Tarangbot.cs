using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;
using TarangBot.GeneralUtils;

namespace TarangBot.DiscordBot
{
    public class Tarangbot : IDestructible
    {
        private DiscordSocketClient _client;

        public List<string> usernames = new List<string>();

        public async Task Start()
        {
            _client = new DiscordSocketClient();

            _client.Log += _client_Log;
            _client.UserJoined += _client_UserJoined;
            _client.UserLeft += _client_UserLeft;
            _client.MessageReceived += _client_MessageReceived;

            Tarang.Data.MessageQueue.On("NewRegistration", (o) =>
             {
                 if (((string[])o).Length == 2)
                     if (((string[])o)[1].Contains("#"))
                         usernames.Add(((string[])o)[1]);
             });

            await _client.LoginAsync(Discord.TokenType.Bot, Tarang.Data.DiscordBotToken);

            await _client.StartAsync();


        }

        private Task _client_UserLeft(SocketGuildUser arg)
        {
            Tarang.Data.Logger.Log(arg.Username + " left");

            return Task.CompletedTask;
        }

        private Task _client_MessageReceived(SocketMessage arg)
        {
            Tarang.Data.Logger.Log(arg.Content);

            return Task.CompletedTask;
        }

        private Task _client_Log(Discord.LogMessage arg)
        {
            Tarang.Data.Logger.Log(arg.Message);

            return Task.CompletedTask;
        }

        private async Task _client_UserJoined(SocketGuildUser arg)//Move to a separate class to organize state the bot should only dispatch events
        {
            try
            {
                if (usernames.Contains(arg.Username + "#" + arg.Discriminator))
                {
                    Tarang.Data.Logger.Log($"{arg.Username} has joined the server {arg.Guild.Name}");

                    foreach (SocketRole item in arg.Guild.Roles)
                    {
                        if (item.Name == "Vertex")
                        {
                            await arg.AddRoleAsync(item);
                            usernames.Remove(arg.Username + "#" + arg.Discriminator);
                            return;
                        }
                    }

                    RestRole vertex = await arg.Guild.CreateRoleAsync("Vertex", Discord.GuildPermissions.None.Modify(administrator: true), null, false, null);

                    await arg.AddRoleAsync(vertex);
                    usernames.Remove(arg.Username + "#" + arg.Discriminator);
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        public void OnDestroy()
        {
            _client.LogoutAsync().Wait();
        }
    }
}
