using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.ConsoleDisplay
{
    public abstract class DisplayElement
    {
        public int Width, Height, Left, Top;

        public bool Outline = true;

        public abstract void Draw(CDisplay display);
    }
}
