using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarangBot.GeneralUtils;
using TarangBot.TarangEvent;

namespace TarangBot
{
    public static class Tarang
    {
        public static TarangData Data;

        public static bool Stop = false;

        public static TimeSpan AvgLoopTimeUpdateInterval = TimeSpan.FromSeconds(5);

        public static DateTime StartTime;

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

            while (!end)
            {
                DateTime tp = DateTime.Now;
                if ((DateTime.Now - last_sheet_poll) > Data.SheetPollInterval)
                {
                    await Data.sheetAdapter.Poll();
                    last_sheet_poll = DateTime.Now;
                }
                if((DateTime.Now - last_time_update) > AvgLoopTimeUpdateInterval)
                {
                    Data.StatusDisp["Average loop time"] = Math.Round(avg_time,2).ToString() + "ms";
                    last_time_update = DateTime.Now;
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
