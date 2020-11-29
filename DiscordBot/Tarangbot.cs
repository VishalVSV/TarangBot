using System;
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


            await _client.LoginAsync(TokenType.Bot, Tarang.Data.DiscordBotToken);


            await _client.StartAsync();


        }

        public Embed ConstructDashboard()
        {

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Tarang Bot Dashboard");

            string err = Tarang.Data.LastError;
            if (string.IsNullOrEmpty(err)) err = "No errors so far!";

            builder.AddField("Registrations:", Tarang.Data.sheetAdapter.ProcessedRecords);
            builder.AddField("Last Error:", err);
            builder.AddField("Total number of participants:", Tarang.Data.participants.Count);

            builder.AddField("Status", Tarang.Stop ? "Offline" : "Online");

            if (Tarang.Stop)
                builder.Color = Color.Red;
            else builder.Color = Color.Green;

            if (Tarang.Data.LastError != "" && !Tarang.Stop)
                builder.Color = new Color(64, 224, 208);



            return builder.Build();
        }

        public async Task UpdateDashboard()
        {
            try
            {

                var a = _client.GetGuild(Tarang.Data.GuildId).GetTextChannel(Tarang.Data.DashboardChannel);
                var m = (await a.GetMessageAsync(Tarang.Data.DashboardMessageId)) as RestUserMessage;
                await (m).ModifyAsync((msg) =>
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
            await CycleStatus();

            Tarang.Data.MessageQueue.On("NewRegistration", async (o) =>
              {
                  await UpdateDashboard();
              });


            await UpdateDashboard();
        }

        private int status = 0;
        public async Task CycleStatus()
        {
            if (status == 0)
            {
                Game game = new Game("Tarang 2020", ActivityType.Playing, ActivityProperties.Instance);

                await _client.SetActivityAsync(game);
            }
            else if (status == 1)
            {
                Game game = new Game("Created by Vertex and Shan10", ActivityType.Playing, ActivityProperties.Instance);

                await _client.SetActivityAsync(game);
            }

            status += 1;
            if (status >= 2)
            {
                status = 0;
            }
        }

        private Task _client_UserLeft(SocketGuildUser arg)
        {

            Tarang.Data.MessageQueue.Dispatch("UserLeft", arg);


            return Task.CompletedTask;
        }

        private Task _client_MessageReceived(SocketMessage arg)
        {
            commandHandler.Handle(arg);


            string content = arg.Content.Trim();
            if (content.StartsWith($"<@{_client.CurrentUser.Id}>"))
            {
                string msg = content.Substring($"<@{_client.CurrentUser.Id}>".Length).ToLower();
                if (msg == "hey")
                {
                    arg.Channel.SendMessageAsync($"Hey <@{arg.Author.Id}>");
                }
            }

            return Task.CompletedTask;
        }

        private Task _client_Log(LogMessage arg)
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
