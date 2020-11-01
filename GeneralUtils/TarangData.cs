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

        public TimeSpan SheetPollInterval = TimeSpan.FromSeconds(5);

        [JsonIgnore]
        public MessageQueueHandler MessageQueue = new MessageQueueHandler();
        public GSheetAdapter sheetAdapter;

        public void Init()
        {
            display.RegisterElement(Logger);
        }

        public static TarangData Load(string path)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TarangData>(File.ReadAllText(path));
        }

    }
}
