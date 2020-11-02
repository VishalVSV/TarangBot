using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TarangBot.ConsoleDisplay;
using TarangBot.GSheetsAdapters;
using TarangBot.MessagingUtils;
using Newtonsoft.Json;

namespace TarangBot.GeneralUtils
{
    public class TarangData
    {
        [JsonIgnore]
        public CDisplay display = new CDisplay();

        [JsonIgnore]
        public ScrollingLogger Logger = new ScrollingLogger(1, 1, Console.WindowWidth - 3, (Console.WindowHeight /2) - 4);
        [JsonIgnore]
        public ScrollingLogger QueueLog = new ScrollingLogger(1, (Console.WindowHeight / 2) - 1, Console.WindowWidth / 2 - 3, Console.WindowHeight - (Console.WindowHeight /2) - 1);
        [JsonIgnore]
        public TarangShell Shell = new TarangShell(Console.WindowWidth / 2 + 1, (Console.WindowHeight / 2) - 1, Console.WindowWidth / 2 - 2, Console.WindowHeight - (Console.WindowHeight / 2) - 1);


        [JsonIgnore]
        public MessageQueueHandler MessageQueue = new MessageQueueHandler();
        public DiscordBot.Tarangbot TarangBot;


        public TimeSpan SheetPollInterval = TimeSpan.FromSeconds(5);
        public GSheetAdapter sheetAdapter;
        public string DiscordBotToken = "";
        public string DiscordBotPrefix = "-";


        public void Init()
        {
            QueueLog.LogPath = "./dispatch_log.txt";

            MessageQueue.OnDispatch = (msg) =>
            {
                QueueLog.Log(msg.EventName + " has been dispatched!");
            };

            display.RegisterElement(Logger);
            display.RegisterElement(QueueLog);
            display.RegisterElement(Shell);

            DestructionHandler.RegisterDestructible(Logger);
            DestructionHandler.RegisterDestructible(QueueLog);


            DestructionHandler.RegisterDestructible(TarangBot);
        }

        public static TarangData Load(string path)
        {
            return JsonConvert.DeserializeObject<TarangData>(File.ReadAllText(path));
        }

    }
}
