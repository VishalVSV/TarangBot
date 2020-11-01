using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TarangBot.GeneralUtils;

namespace TarangBot
{
    public static class Tarang
    {
        public static TarangData Data;

        public static async Task Main()
        {
            if(!File.Exists("./Data/config.txt"))
            {
                throw new Exception("Config file not found!");
            }

            Data = TarangData.Load("./Data/config.txt");

            Data.Init();
            Data.sheetAdapter.OnNewRecord = (string[] new_record) =>
            {
                Data.MessageQueue.Dispatch("NewRegistration", new_record);
            };
            Data.MessageQueue.On("NewRegistration", (a) =>
             {
                 Data.Logger.Log("New Record Received!");
             });

            Data.display.Start();

            DateTime last_sheet_poll = DateTime.Now;
            bool end = false;
            while (!end)
            {
                if ((DateTime.Now - last_sheet_poll) > Data.SheetPollInterval)
                {
                    last_sheet_poll = DateTime.Now;
                    await Data.sheetAdapter.Poll();
                }

                Data.MessageQueue.Log = Data.Logger.Log;
                Data.sheetAdapter.Log = Data.Logger.Log;

                Data.MessageQueue.HandleEvents();

                while (Console.KeyAvailable)
                {
                    if(Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Data.display.Stop();
                        File.WriteAllText("./Data/config.txt", Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented));
                        end = true;
                        break;
                    }
                }
            }
        }
    }
}
