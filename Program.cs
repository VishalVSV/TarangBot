using System;
using System.Threading;
using TarangBot.ConsoleDisplay;

namespace TarangBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CDisplay display = new CDisplay();

            ScrollingLogger log = new ScrollingLogger(1, 1, Console.WindowWidth - 3, 10);

            display.DisplayElements.Add(log);

            display.Resize();
            display.Start();

            ConsoleKeyInfo s = Console.ReadKey(true);
            while (s.Key != ConsoleKey.Escape)
            {
                log.Log("Test");
                Thread.Sleep(1000);
            }
            display.Stop();
        }
    }
}
