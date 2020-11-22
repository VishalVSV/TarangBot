using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.GeneralUtils
{
    public static class DestructionHandler
    {
        public static List<IDestructible> Destructibles = new List<IDestructible>();

        public static void RegisterDestructible(IDestructible destructible)
        {
            Destructibles.Add(destructible);
        }

        public static void DestroyAll()
        {
            for (int i = 0; i < Destructibles.Count; i++)
            {
                Tarang.Data.Logger.Log($"Destroying {Destructibles[i].GetType().Name}");
                Destructibles[i].OnDestroy();
            }
        }
    }

    public interface IDestructible
    {
        void OnDestroy();
    }
}
