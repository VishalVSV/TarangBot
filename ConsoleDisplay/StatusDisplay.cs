﻿using System;
using System.Collections.Generic;

namespace TarangBot.ConsoleDisplay
{
    public class StatusDisplay : DisplayElement
    {
        public Dictionary<string, string> properties = new Dictionary<string, string>();

        public string this[string s]
        {
            get
            {
                if (properties.ContainsKey(s))
                    return properties[s];
                else return null;
            }
            set
            {

                if (properties.ContainsKey(s))
                {
                    properties[s] = value;
                }
                else
                {
                    properties.Add(s, value);
                }
            }
        }

        public StatusDisplay(int x, int y, int width, int height)
        {
            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        public override void Update()
        {
            var t = (DateTime.Now - Tarang.StartTime);
            Tarang.Data.StatusDisp["Uptime"] = $"{t.Days} Days {t.Hours} hrs {t.Minutes} mins {t.Seconds} secs";
        }

        public override void Draw(CDisplay display)
        {
            try
            {
                int y = 0;
                foreach (var (key, value) in properties)
                {
                    display.DrawString(0, y++, $"{key}: {value}");
                }
            }
            catch (InvalidOperationException)
            {

            }
        }
    }
}
