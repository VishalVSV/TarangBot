using System;
using System.Threading;
using System.Threading.Tasks;
using TarangBot.ConsoleDisplay;
using TarangBot.GSheetsAdapters;

namespace TarangBot
{
    class Program
    {
        static ScrollingLogger log = new ScrollingLogger(1, 1, Console.WindowWidth - 3, (Console.WindowHeight * 2) / 5);

        static void Main(string[] args)
        {
            Main().Wait();
        }

        static async Task Main()
        {
            CDisplay display = new CDisplay();
            GSheetAdapter adapter = new GSheetAdapter("1z17_SLl3-Aq1SnjmW7pD1FETX4ychvAlbjCwHehl8Ko", "");

            adapter.Log = log.Log;

            display.DisplayElements.Add(log);

            display.Resize();
            display.Start();

            await adapter.Poll();

            log.Log("Test");


            ConsoleKeyInfo s = Console.ReadKey(true);
            display.Stop();
        }


    }
}
