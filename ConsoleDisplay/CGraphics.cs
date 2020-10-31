using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.ConsoleDisplay
{
    public partial class CDisplay
    {

        private void Clear(char c = ' ')
        {
            for (int i = 0; i < Width * Height; i++)
            {
                buffer[i] = c;
            }
        }

        public void DrawPixel(int x, int y, char c)
        {
            if (x >= 0 && x < currently_drawing.Width && y >= 0 && y < currently_drawing.Height)
            {
                int sx = x + currently_drawing.Left;
                int sy = y + currently_drawing.Top;
                if (sx >= 0 && sx < Width && sy >= 0 && sy < Height)
                {
                    buffer[sx + sy * Width] = c;
                }
            }
        }

        private void SSDrawPixel(int x, int y, char c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                buffer[x + y * Width] = c;
            }
        }

        public void DrawLine(int x0, int y0, int x1, int y1, char c)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (; ; )
            {
                DrawPixel(x0, y0, c);
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
        }

        private void SSDrawLine(int x0, int y0, int x1, int y1, char c)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (; ; )
            {
                SSDrawPixel(x0, y0, c);
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
        }

        public void DrawString(int x, int y, string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                DrawPixel(x + i, y, str[i]);
            }
        }
    }
}
