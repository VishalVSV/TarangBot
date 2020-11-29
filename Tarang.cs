using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarangBot.GeneralUtils;
using TarangBot.MailIntegration;

namespace TarangBot
{
    public static class Tarang
    {
        public static TarangData Data;

        public static Mutex data_mutex = new Mutex();

        public static bool Stop = false;

        public static TimeSpan AvgLoopTimeUpdateInterval = TimeSpan.FromSeconds(5);
        public static TimeSpan AutoSaveInterval = TimeSpan.FromMinutes(2);

        public static DateTime StartTime;

        public static void ReloadConfig()
        {
            if (!File.Exists("./Data/config.txt"))
            {
                throw new Exception("Config file not found!");
            }

            TarangData data = TarangData.Load("./Data/config.txt");

            Data.AnnouncementChannel = data.AnnouncementChannel;
            Data.BotMessagesChannel = data.BotMessagesChannel;
            Data.DashboardChannel = data.DashboardChannel;
            Data.DashboardMessageId = data.DashboardMessageId;
            Data.DiscordBotPrefix = data.DiscordBotPrefix;
            Data.DiscordBotToken = data.DiscordBotToken;
            Data.DiscordInvite = data.DiscordInvite;
            Data.GuildId = data.GuildId;
            Data.LastError = data.LastError;
            Data.MailPassword = data.MailPassword;
            Data.MailUsername = data.MailUsername;
            Data.participants = data.participants;
            Data.raw_events = data.raw_events;
            Data.SheetPollInterval = data.SheetPollInterval;

            Data.Events.Clear();

            for (int i = 0; i < Data.raw_events.Count; i++)
            {
                Data.raw_events[i].LoadData();
                for (int j = 0; j < Data.raw_events[i].Names.Length; j++)
                {
                    Data.Events.Add(Data.raw_events[i].Names[j], Data.raw_events[i]);
                }
            }

            GmailDaemon.SetCredentials(Data.MailUsername, Encoding.UTF8.GetString(Convert.FromBase64String(Data.MailPassword)));
        }

        public static async Task Main()
        {
            StartTime = DateTime.Now;

            if (!File.Exists("./Data/config.txt"))
            {
                throw new Exception("Config file not found!");
            }

            Data = TarangData.Load("./Data/config.txt");

            Data.Init();
            Data.sheetAdapter.OnNewRecord = (new_record) =>
            {
                Data.MessageQueue.Dispatch("NewRegistration", new_record);
            };

            Data.MessageQueue.On("NewRegistration", (a) =>
             {
                 Data.Logger.Log("New Record Received!");
             });

            Data.display.Start();
            Task bot = Data.TarangBot.Start();

            Data.MessageQueue.Log = Data.Logger.Log;
            Data.sheetAdapter.Log = Data.Logger.Log;

            DateTime last_sheet_poll = DateTime.Now;
            bool end = false;

            Data.roleGiver.Init();

            double avg_time = 0;

            DateTime last_time_update = DateTime.Now;
            DateTime last_auto_save = DateTime.Now;
            DateTime last_easter_egg = DateTime.Now;

            while (!end)
            {
                DateTime tp = DateTime.Now;
                if ((DateTime.Now - last_sheet_poll) > Data.SheetPollInterval)
                {
                    await Data.sheetAdapter.Poll();
                    last_sheet_poll = DateTime.Now;
                }
                if ((DateTime.Now - last_time_update) > AvgLoopTimeUpdateInterval)
                {
                    Data.StatusDisp["Average loop time"] = Math.Round(avg_time, 2).ToString() + "ms";
                    last_time_update = DateTime.Now;
                }
                if ((DateTime.Now - last_easter_egg) > TimeSpan.FromMinutes(30))
                {
                    await Data.TarangBot.CycleStatus();
                    last_easter_egg = DateTime.Now;
                }
                if ((DateTime.Now - last_auto_save) > AutoSaveInterval)
                {
                    Data.Logger.Log("Auto saving...");

                    DateTime t = DateTime.Now;
                    Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                    settings.Formatting = Newtonsoft.Json.Formatting.Indented;

                    File.WriteAllText("./Data/config.txt", Newtonsoft.Json.JsonConvert.SerializeObject(Data, settings));
                    Data.Logger.Log($"Auto save completed in {Math.Round((DateTime.Now - t).TotalMilliseconds, 2)} ms");

                    last_auto_save = DateTime.Now;
                }

                Data.MessageQueue.HandleEvents();

                Thread.Sleep(10);

                if (Stop)
                {
                    await Data.TarangBot.UpdateDashboard();


                    await bot;
                    DestructionHandler.DestroyAll();

                    Data.display.Stop();

                    Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                    settings.Formatting = Newtonsoft.Json.Formatting.Indented;

                    File.WriteAllText("./Data/config.txt", Newtonsoft.Json.JsonConvert.SerializeObject(Data, settings));
                    end = true;

                    Console.Clear();

                    break;
                }

                avg_time = (avg_time + (DateTime.Now - tp).TotalMilliseconds) / 2;
            }
        }
    }
}
