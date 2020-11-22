using System;
using System.IO;
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
            string[] tmp = File.ReadAllText("startup_banner.txt").Split('$');

            for (int i = 0; i < tmp.Length; i++)
            {
                if (i % 2 == 0) Console.ForegroundColor = ConsoleColor.Blue;
                else Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(tmp[i]);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("Press any button to continue...");
            Console.ReadKey(true);

            while (!Tarang.Stop)
            {
                try
                {
                    Main().Wait();
                }
                catch (Exception e)
                {
                    Tarang.Data.SendDiscordLog($"FATAL: {e.Message}");
                }
            }
        }

        static async Task Main()
        {
            await Tarang.Main();
        }


    }
}
