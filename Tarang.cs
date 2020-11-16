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

        public static async Task Main()
        {
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
            last_sheet_poll = last_sheet_poll.Subtract(TimeSpan.FromSeconds(10));
            bool end = false;
            
            Data.roleGiver.Init();

            while (!end)
            {
                if ((DateTime.Now - last_sheet_poll) > Data.SheetPollInterval)
                {
                    await Data.sheetAdapter.Poll();
                    last_sheet_poll = DateTime.Now;
                }

                Data.MessageQueue.HandleEvents();

                Thread.Sleep(10);

                if (Stop)
                {
                    Data.display.Stop();
                    await bot;
                    DestructionHandler.DestroyAll();

                    Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                    settings.Formatting = Newtonsoft.Json.Formatting.Indented;

                    File.WriteAllText("./Data/config.txt", Newtonsoft.Json.JsonConvert.SerializeObject(Data, settings));
                    end = true;

                    Console.Clear();

                    break;
                }
            }
        }
    }
}
