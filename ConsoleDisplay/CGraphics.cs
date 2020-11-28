using System;

namespace TarangBot.ConsoleDisplay
{
    public partial class CDisplay
    {
        /// <summary>
        /// Screen space clear
        /// </summary>
        /// <param name="c">Char to clear the screen with</param>
        private void Clear(char c = ' ')
        {
            for (int i = 0; i < Width * Height; i++)
            {
                buffer[i] = c;
            }
        }

        /// <summary>
        /// Element space draw pixel
        /// </summary>
        /// <param name="x">X Coordinate of the pixel</param>
        /// <param name="y">Y Coordinate of the pixel</param>
        /// <param name="c">Char to draw</param>
        public void DrawPixel(int x, int y, char c)
        {
            if (x >= 0 && x <= currently_drawing.Width && y >= 0 && y <= currently_drawing.Height)
            {
                int sx = x + currently_drawing.Left;
                int sy = y + currently_drawing.Top;
                if (sx >= 0 && sx < Width && sy >= 0 && sy < Height)
                {
                    buffer[sx + sy * Width] = c;
                }
            }
        }

        /// <summary>
        /// Screen space draw pixel
        /// </summary>
        /// <param name="x">X Coordinate of the pixel</param>
        /// <param name="y">Y Coordinate of the pixel</param>
        /// <param name="c">Char to draw</param>
        private void SSDrawPixel(int x, int y, char c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                buffer[x + y * Width] = c;
            }
        }

        /// <summary>
        /// Element space draw line
        /// </summary>
        /// <param name="x0">X Coordinate of the first point</param>
        /// <param name="y0">Y Coordinate of the first point</param>
        /// <param name="x1">X Coordinate of the second point</param>
        /// <param name="y1">Y Coordinate of the second point</param>
        /// <param name="c">Char to draw line with</param>
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


        /// <summary>
        /// Screen space draw line
        /// </summary>
        /// <param name="x0">X Coordinate of the first point</param>
        /// <param name="y0">Y Coordinate of the first point</param>
        /// <param name="x1">X Coordinate of the second point</param>
        /// <param name="y1">Y Coordinate of the second point</param>
        /// <param name="c">Char to draw line with</param>
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

        /// <summary>
        /// Draw string onto console
        /// </summary>
        /// <param name="x">X Coordinate of string</param>
        /// <param name="y">Y Coordinate of string</param>
        /// <param name="str">The string to draw</param>
        public void DrawString(int x, int y, string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                DrawPixel(x + i, y, str[i]);
            }
        }
    }
}
