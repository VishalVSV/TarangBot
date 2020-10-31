using System;
using System.Collections.Generic;
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

        private List<string> lines = new List<string>();

        private int start_index = 0;

        public void Log(string s)
        {
            lines.Add($"[{DateTime.Now.ToString("HH:mm:ss")}]: {s}");
            if (lines.Count - start_index > Height)
                start_index++;
            
        }

        public override void Draw(CDisplay display)
        {
            for (int i = start_index; i < lines.Count; i++)
            {
                display.DrawString(0, i - start_index, lines[i]);
            }
        }
    }
}
