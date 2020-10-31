using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TarangBot.ConsoleDisplay
{
    public class ScrollingLogger : DisplayElement
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

        public void Log(string s)
        {
            lines.Enqueue($"[{DateTime.Now.ToString("HH:mm:ss")}]: {s}");
            if (lines.Count - start_index > Height)
                start_index++;


        }

        public override void Draw(CDisplay display)
        {
            string[] lin = lines.ToArray();
            for (int i = start_index; i < lines.Count; i++)
            {
                display.DrawString(0, i - start_index, lin[i]);
            }
        }
    }
}
