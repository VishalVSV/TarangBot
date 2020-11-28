namespace TarangBot.ConsoleDisplay
{
    public abstract class DisplayElement
    {
        public int Width, Height, Left, Top;

        public bool Outline = true;

        public virtual void Update() { }

        public abstract void Draw(CDisplay display);
    }
}
