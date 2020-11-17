using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TarangBot.GeneralUtils;

namespace TarangBot.ConsoleDisplay
{
    public class ScrollingLogger : DisplayElement, IDestructible
    {
        public string LogPath = "./log.txt";

        public bool Save = true;

        //TODO: Add title bar to scrolling logger so that polling can be a status rather than a log
        public string Title = "";

        public ScrollingLogger(int x, int y, int width, int height)
        {
            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        protected Queue<string> lines = new Queue<string>();

        protected int start_index = 0;

        protected const int LINE_OVERHEAD = 100;

        public virtual void Log(string s)
        {
            lines.Enqueue($"[{DateTime.Now.ToString("HH:mm:ss")}]: {s}");
            if (lines.Count - start_index > Height)
                start_index++;

            if (lines.Count > Height + LINE_OVERHEAD)
            {
                lines.Enqueue($"[{DateTime.Now.ToString("HH:mm:ss")}]: Saving Log");
                if (lines.Count - start_index > Height)
                    start_index++;

                string[] to_save = new string[lines.Count - Height - LINE_OVERHEAD / 2];
                for (int i = 0; i < lines.Count - Height - LINE_OVERHEAD / 2; i++)
                {
                    to_save[i] = lines.Dequeue();
                }
                if (Save)
                    File.AppendAllLines(LogPath, to_save);
            }
        }

        public override void Draw(CDisplay display)
        {
            string[] lin = lines.ToArray();
            for (int i = start_index; i < lines.Count; i++)
            {
                display.DrawString(0, i - start_index, lin[i]);
            }
        }

        public void OnDestroy()
        {
            if (Save)
                File.AppendAllLines(LogPath, lines);
        }
    }
}
