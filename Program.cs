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
