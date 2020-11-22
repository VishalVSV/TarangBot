using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TarangBot.ConsoleDisplay
{
    public partial class CDisplay
    {
        /// <summary>
        /// Display buffer for the display
        /// </summary>
        private StringBuilder buffer;
        /// <summary>
        /// Stores the element currently being drawn. Used to access the draw commands.
        /// </summary>
        private DisplayElement currently_drawing;

        /// <summary>
        /// Specifies the number of times the display will update per second
        /// </summary>
        public int DisplayRefreshRate = 60;

        /// <summary>
        /// The cancellation token for the draw loop thread
        /// </summary>
        private CancellationTokenSource draw_loop_cts;

        /// <summary>
        /// The list of display elements.
        /// </summary>
        private List<DisplayElement> DisplayElements = new List<DisplayElement>();

        /// <summary>
        /// The list of keys that have been pressed in that loop. Hardcoded to a maximum of 5 keys per loop. I have no idea why I did that though.
        /// </summary>
        public List<ConsoleKeyInfo> Keys = new List<ConsoleKeyInfo>(5);

        /// <summary>
        /// Register an element to be drawn
        /// </summary>
        /// <param name="element">The element to be registered</param>
        public void RegisterElement(DisplayElement element)
        {
            DisplayElements.Add(element);
        }

        /// <summary>
        /// The width and height of the panel
        /// </summary>
        public int Width, Height;

        /// <summary>
        /// Resize the internal buffer
        /// </summary>
        public void Resize()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight - 1;
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

        /// <summary>
        /// Start the draw loop.
        /// </summary>
        public void Start()
        {
            Resize();
            Console.CursorVisible = false;

            draw_loop_cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!draw_loop_cts.IsCancellationRequested)
                {
                    Draw();

                    await Task.Delay(1000 / DisplayRefreshRate);
                }
            }, draw_loop_cts.Token);
        }

        /// <summary>
        /// Stop the draw loop
        /// </summary>
        public void Stop()
        {
            draw_loop_cts.Cancel();

        }

        /// <summary>
        /// Poll keys available in the console.
        /// </summary>
        private void PollKeys()
        {
            Keys.Clear();

            int i = 0;
            while (Console.KeyAvailable && i < 5)
            {
                Keys.Add(Console.ReadKey(true));
                i++;
            }
        }

        /// <summary>
        /// Draw the screen
        /// </summary>
        private void Draw()
        {
            try
            {
                Clear();

                PollKeys();

                Console.CursorVisible = false;
                for (int i = 0; i < DisplayElements.Count; i++)
                {
                    currently_drawing = DisplayElements[i];

                    DisplayElements[i].Update();
                    DisplayElements[i].Draw(this);

                    if (DisplayElements[i].Outline)
                    {
                        SSDrawLine(DisplayElements[i].Left - 1, DisplayElements[i].Top - 1, DisplayElements[i].Left - 1, DisplayElements[i].Top + DisplayElements[i].Height + 1, '│');//Left edge
                        SSDrawLine(DisplayElements[i].Left - 1, DisplayElements[i].Top - 1, DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top - 1, '─');//Top edge
                        SSDrawLine(DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top - 1, DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top + 1 + DisplayElements[i].Height, '│');//Right edge
                        SSDrawLine(DisplayElements[i].Left - 1, DisplayElements[i].Top + DisplayElements[i].Height + 1, DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top + 1 + DisplayElements[i].Height, '─');//Bottom edge

                        SSDrawPixel(DisplayElements[i].Left - 1, DisplayElements[i].Top - 1, '+');
                        SSDrawPixel(DisplayElements[i].Left - 1, DisplayElements[i].Top + 1 + DisplayElements[i].Height, '+');
                        SSDrawPixel(DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top + 1 + DisplayElements[i].Height, '+');
                        SSDrawPixel(DisplayElements[i].Left + 1 + DisplayElements[i].Width, DisplayElements[i].Top - 1, '+');
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
