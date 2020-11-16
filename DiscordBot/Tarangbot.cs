using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using TarangBot.GeneralUtils;

namespace TarangBot.DiscordBot
{
    public class Tarangbot : IDestructible
    {
        [JsonIgnore]
        public DiscordSocketClient _client;

        [JsonIgnore]
        public CommandHandler commandHandler = new CommandHandler("TarangBot.DiscordBot.Commands");

        public async Task Start()
        {
            _client = new DiscordSocketClient();

            _client.Log += _client_Log;
            _client.UserJoined += _client_UserJoined;
            _client.UserLeft += _client_UserLeft;
            _client.MessageReceived += _client_MessageReceived;
            _client.Ready += _client_Ready;

            await _client.LoginAsync(Discord.TokenType.Bot, Tarang.Data.DiscordBotToken);

            await _client.StartAsync();


        }

        public Embed ConstructDashboard()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Tarang Bot Dashboard");

            builder.AddField("Registrations:", Tarang.Data.sheetAdapter.ProcessedRecords);

            return builder.Build();
        }

        public async Task UpdateDashboard()
        {
            try
            {
                await ((await _client.GetGuild(Tarang.Data.GuildId).GetTextChannel(Tarang.Data.DashboardChannel).GetMessageAsync(Tarang.Data.DashboardMessageId)) as SocketUserMessage).ModifyAsync((msg) =>
                {
                    msg.Embed = ConstructDashboard();
                });
            }
            catch (Exception)
            {

            }
        }

        private async Task _client_Ready()
        {
            Game game = new Game("Tarang 2020", ActivityType.Playing, ActivityProperties.Instance);

            await _client.SetActivityAsync(game);

            Tarang.Data.MessageQueue.On("NewRegistration", async (o) =>
              {
                  await UpdateDashboard();
              });

            await UpdateDashboard();
        }

        private Task _client_UserLeft(SocketGuildUser arg)
        {
            Tarang.Data.MessageQueue.Dispatch("UserLeft", arg);

            return Task.CompletedTask;
        }

        private Task _client_MessageReceived(SocketMessage arg)
        {
            if (arg.Content.StartsWith(Tarang.Data.DiscordBotPrefix))
            {
                commandHandler.Handle(arg);
            }
            return Task.CompletedTask;
        }

        private Task _client_Log(Discord.LogMessage arg)
        {
            Tarang.Data.Logger.Log(arg.Message);

            return Task.CompletedTask;
        }

        private Task _client_UserJoined(SocketGuildUser arg)//Move to a separate class to organize state the bot should only dispatch events
        {
            Tarang.Data.MessageQueue.Dispatch("OnUserJoin", arg);

            return Task.CompletedTask;
        }

        public void OnDestroy()
        {
            _client.LogoutAsync().Wait();
        }
    }
}
