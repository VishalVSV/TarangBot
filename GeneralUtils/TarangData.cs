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
        public ScrollingLogger Logger = new ScrollingLogger(1, 1, Console.WindowWidth - 3, (Console.WindowHeight * 2) / 5);
        [JsonIgnore]
        public MessageQueueHandler MessageQueue = new MessageQueueHandler();
        public DiscordBot.Tarangbot TarangBot;


        public TimeSpan SheetPollInterval = TimeSpan.FromSeconds(5);
        public GSheetAdapter sheetAdapter;
        public string DiscordBotToken = "";


        public void Init()
        {
            display.RegisterElement(Logger);

            DestructionHandler.RegisterDestructible(Logger);


            DestructionHandler.RegisterDestructible(TarangBot);
        }

        public static TarangData Load(string path)
        {
            return JsonConvert.DeserializeObject<TarangData>(File.ReadAllText(path));
        }

    }
}
