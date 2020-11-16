using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TarangBot.ConsoleDisplay;
using TarangBot.GSheetsAdapters;
using TarangBot.MessagingUtils;
using Newtonsoft.Json;
using TarangBot.TarangEvent;
using TarangBot.DiscordBot;
using System.Linq;

namespace TarangBot.GeneralUtils
{
    public class TarangData
    {
        [JsonIgnore]
        public CDisplay display = new CDisplay();

        [JsonIgnore]
        public ScrollingLogger Logger = new ScrollingLogger(1, 1, Console.WindowWidth - 3, (Console.WindowHeight / 2) - 2);
        [JsonIgnore]
        public ScrollingLogger QueueLog = new ScrollingLogger(1, (Console.WindowHeight / 2) + 1, Console.WindowWidth / 2 - 2, Console.WindowHeight / 2 - 3);
        [JsonIgnore]
        public TarangShell Shell = new TarangShell(Console.WindowWidth / 2 + 1, (Console.WindowHeight / 2) + 1, Console.WindowWidth / 2 - 2, Console.WindowHeight / 2 - 3);

        public List<PollSystem> PollSystem = new List<PollSystem>();

        [JsonIgnore]
        public MessageQueueHandler MessageQueue = new MessageQueueHandler();
        public Tarangbot TarangBot;

        public RegistrationRoleGiver roleGiver;

        public TimeSpan SheetPollInterval = TimeSpan.FromSeconds(5);
        public GSheetAdapter sheetAdapter;
        public string DiscordBotToken = "";
        public string DiscordBotPrefix = "-";
        
        public List<Event> raw_events = new List<Event>();
        public bool Events_Init = false;

        [JsonIgnore]
        public Dictionary<string, Event> Events = new Dictionary<string, Event>();

        //Credit to Tbone1983 - https://www.codeproject.com/Questions/419563/Get-the-nearest-Match-of-the-string-in-list-of-str
        private static int LevenshteinDistance(string s,string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public Event GetEvent(string name)
        {
            Event event_ = null;

            var t = Events.Keys.Select(s => (s, LevenshteinDistance(name, s))).ToList();

            string i = "";
            int min = int.MaxValue;
            for (int _ = 0; _ < t.Count; _++)
            {
                if (t[_].Item2 < min)
                {
                    i = t[_].s;
                    min = t[_].Item2;
                }
            }

            event_ = Events[i];

            return event_;
        }

        public void Init()
        {
            if (!Events_Init)
            {
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "One Mic Stand" },          true, 1, EventType.Flagged, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Fort Boyard" },            true, 3, EventType.Flagged, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Two Faced" },              true, 2, EventType.Flagged, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Whose Line is it anyway" },true, 2, EventType.Flagged, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Fandomania" },             true, 1, EventType.Flagged, false));

                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "COD" },                    true, 6, EventType.Non_Flagged, true));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Step Up" },                true, 2, EventType.Non_Flagged, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Trailer it up" },          true, 2, EventType.Non_Flagged, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Synthesize" },             true, 2, EventType.Non_Flagged, false));

                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Meme-athon" },             true, 2, EventType.Sub, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Pixel" },                  true, 2, EventType.Sub, false));
                raw_events.Add(new Event("TOFILL", "TOFILL", new string[] { "Craft a Block" },          true, 2, EventType.Sub, false));

                Events_Init = true;
            }

            for (int i = 0; i < raw_events.Count; i++)
            {
                for (int j = 0; j < raw_events[i].Names.Length; j++)
                {
                    Events.Add(raw_events[i].Names[j], raw_events[i]);
                }
            }

            QueueLog.LogPath = "./dispatch_log.txt";

            MessageQueue.OnDispatch = (msg) =>
            {
                QueueLog.Log(msg.EventName + " has been dispatched!");
            };

            display.RegisterElement(Logger);
            display.RegisterElement(QueueLog);
            display.RegisterElement(Shell);

            for (int i = 0; i < PollSystem.Count; i++)
            {
                PollSystem[i].Init();
            }

            DestructionHandler.RegisterDestructible(Logger);
            DestructionHandler.RegisterDestructible(QueueLog);


            DestructionHandler.RegisterDestructible(TarangBot);

        }

        public void Resize()
        {
            //Logger = new ScrollingLogger(1, 1, Console.WindowWidth - 3, (Console.WindowHeight / 2) - 2);
            Logger.Left = 1;
            Logger.Top = 1;
            Logger.Width = Console.WindowWidth - 3;
            Logger.Height = (Console.WindowHeight / 2) - 2;

            //public ScrollingLogger QueueLog = new ScrollingLogger(1, (Console.WindowHeight / 2) + 1, Console.WindowWidth / 2 - 2, Console.WindowHeight / 2 - 3);
            QueueLog.Left = 1;
            QueueLog.Top = (Console.WindowHeight / 2) + 1;
            QueueLog.Width = Console.WindowWidth / 2 - 2;
            QueueLog.Height = Console.WindowHeight / 2 - 3;

            //public TarangShell Shell = new TarangShell(Console.WindowWidth / 2 + 1, (Console.WindowHeight / 2) + 1, Console.WindowWidth / 2 - 2, Console.WindowHeight / 2 - 3);
            Shell.Left = Console.WindowWidth / 2 + 1;
            Shell.Top = (Console.WindowHeight / 2) + 1;
            Shell.Width = Console.WindowWidth / 2 - 2;
            Shell.Height = Console.WindowHeight / 2 - 3;
        }

        public static TarangData Load(string path)
        {
            return JsonConvert.DeserializeObject<TarangData>(File.ReadAllText(path));
        }

    }
}
