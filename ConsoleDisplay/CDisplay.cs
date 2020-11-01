﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TarangBot.ConsoleDisplay
{
    public partial class CDisplay
    {
        private StringBuilder buffer;
        private DisplayElement currently_drawing;

        /// <summary>
        /// Specifies the number of times the display will update per second
        /// </summary>
        public int DisplayRefreshRate = 60;

        private CancellationTokenSource draw_loop_cts;

        private List<DisplayElement> DisplayElements = new List<DisplayElement>();

        public void RegisterElement(DisplayElement element)
        {
            DisplayElements.Add(element);
        }

        public int Width, Height;

        public void Resize()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            buffer = new StringBuilder(Width * Height);
            for (int i = 0; i < Width * Height; i++)
            {
                buffer.Append(' ');
            }
        }

        public CDisplay()
        {
            Resize();
        }

        public void Start()
        {
            Resize();
            Console.CursorVisible = false;

            draw_loop_cts = new CancellationTokenSource();

            Task.Run(async () => {
                while (!draw_loop_cts.IsCancellationRequested)
                {
                    Draw();

                    await Task.Delay(1000 / DisplayRefreshRate);
                }
            }, draw_loop_cts.Token);
        }

        public void Stop()
        {
            draw_loop_cts.Cancel();

        }
        
        public void Draw()
        {
            try
            {
                Clear();

                Console.CursorVisible = false;
                for (int i = 0; i < DisplayElements.Count; i++)
                {
                    currently_drawing = DisplayElements[i];

                    DisplayElements[i].Draw(this);

                    if (DisplayElements[i].Outline)
                    {
                        SSDrawLine(DisplayElements[i].Left - 1, DisplayElements[i].Top - 1, DisplayElements[i].Left - 1, DisplayElements[i].Top + DisplayElements[i].Height + 1, '▒');
                        SSDrawLine(DisplayElements[i].Left - 1, DisplayElements[i].Top - 1, DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top - 1, '▒');
                        SSDrawLine(DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top - 1, DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top + 1 + DisplayElements[i].Height, '▒');
                        SSDrawLine(DisplayElements[i].Left - 1, DisplayElements[i].Top + DisplayElements[i].Height + 1, DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top + 1 + DisplayElements[i].Height, '▒');

                    }
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(buffer.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
