using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TarangBot.ConsoleDisplay
{
    public class TarangShell : ScrollingLogger
    {
        StringBuilder current_cmd = new StringBuilder();

        public TarangShell(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        public override void Draw(CDisplay display)
        {
            base.Draw(display);

            for (int i = 0; i < display.Keys.Count; i++)
            {
                if (!char.IsControl(display.Keys[i].KeyChar))
                {
                    current_cmd.Append(display.Keys[i].KeyChar);
                }
                else if (display.Keys[i].Key == ConsoleKey.Backspace)
                {
                    if (current_cmd.Length > 0)
                        current_cmd.Remove(current_cmd.Length - 1, 1);
                }
                else if (display.Keys[i].Key == ConsoleKey.Enter)
                {
                    HandleCommand(current_cmd.ToString());
                    current_cmd.Clear();
                }
            }

            display.DrawString(0, Height, ">" + current_cmd.ToString());
        }

        public override void Log(string s)
        {
            lines.Enqueue($"$TarangBot: {s}");
            if (lines.Count - start_index > Height)
                start_index++;

            if (lines.Count > Height + LINE_OVERHEAD)
            {
                if (lines.Count - start_index > Height)
                    start_index++;

                string[] to_save = new string[lines.Count - Height - LINE_OVERHEAD / 2];
                for (int i = 0; i < lines.Count - Height - LINE_OVERHEAD / 2; i++)
                {
                    to_save[i] = lines.Dequeue();
                }
                File.AppendAllLines(LogPath, to_save);
            }
        }

        private void HandleCommand(string cmd)
        {
            Log(cmd);
            if (cmd.Trim() == "stop")
            {
                Tarang.Data.Logger.Log("Stopping");
                Tarang.Stop = true;
            }
            else if (cmd.Trim() == "resize")
            {
                Tarang.Data.Resize();
                Tarang.Data.display.Resize();
            }
        }
    }
}
