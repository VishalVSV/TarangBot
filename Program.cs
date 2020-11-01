using System;
using System.Threading;
using System.Threading.Tasks;
using TarangBot.ConsoleDisplay;
using TarangBot.GeneralUtils;
using TarangBot.GSheetsAdapters;
using TarangBot.MessagingUtils;

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
            await Tarang.Main();
        }


    }
}
