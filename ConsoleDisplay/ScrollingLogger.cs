using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TarangBot.GeneralUtils;

namespace TarangBot.ConsoleDisplay
{
    public class ScrollingLogger : DisplayElement, IDestructible
    {
        public ScrollingLogger(int x,int y,int width,int height)
        {
            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        private Queue<string> lines = new Queue<string>();

        private int start_index = 0;

        private const int LINE_OVERHEAD = 50;

        public void Log(string s)
        {
            lines.Enqueue($"[{DateTime.Now.ToString("HH:mm:ss")}]: {s}");
            if (lines.Count - start_index > Height)
                start_index++;

            if(lines.Count > Height + LINE_OVERHEAD)
            {
                lines.Enqueue($"[{DateTime.Now.ToString("HH:mm:ss")}]: Saving Log");
                if (lines.Count - start_index > Height)
                    start_index++;

                string[] to_save = new string[lines.Count - Height - LINE_OVERHEAD];
                for (int i = 0; i < lines.Count - Height - LINE_OVERHEAD; i++)
                {
                    to_save[i] = lines.Dequeue();
                }
                File.AppendAllLines("./log.txt", to_save);
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
            File.AppendAllLines("./log.txt", lines);
        }
    }
}
