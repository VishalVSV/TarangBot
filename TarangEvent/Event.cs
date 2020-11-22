using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Discord;

namespace TarangBot.TarangEvent
{
    public enum EventType
    {
        Flagged,
        Non_Flagged,
        Sub
    }

    public class Event
    {
        //Help text
        [JsonIgnore]
        public string DescriptiveHelpText;//Desc
        [JsonIgnore]
        public string Rules;//Rules

        public string Path_to_Data = "";

        public string internal_id = "";

        public string[] Names;

        //Optionals
        public EventType eventType;
        public bool isTeam = false;

        //Must Set
        public int MaxParticipants;
        public bool IsTwoDays;

        public ulong Role_Id = 0;
        public ulong WaitingRoomId = 0;
        public ulong TextChannel = 0;

        [JsonIgnore]
        public SocketVoiceChannel WaitingVC
        {
            get
            {
                return Tarang.Data.TarangBot._client.GetGuild(Tarang.Data.GuildId).GetVoiceChannel(WaitingRoomId);
            }
        }

        public Event(string[] names, bool IsTwoDays, int MaxParticipants, EventType eventType, bool isTeam, string path_to_data)
        {
            Path_to_Data = path_to_data;

            this.IsTwoDays = IsTwoDays;
            this.MaxParticipants = MaxParticipants;
            this.eventType = eventType;
            this.isTeam = isTeam;

            Names = names;
        }

        public static implicit operator Embed(Event @event)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(@event.Names[0]);
            builder.WithDescription(@event.DescriptiveHelpText);

            builder.AddField("Rules", @event.Rules);

            builder.Color = Color.Blue;

            return builder.Build();
        }

        public void LoadData()
        {
            if (Path_to_Data == null) return;

            if (File.Exists(Path.Combine(Path_to_Data, "metadata.txt")))
            {
                string metadata = File.ReadAllText(Path.Combine(Path_to_Data, "metadata.txt"));

                DescriptiveHelpText = metadata.Substring(0, metadata.IndexOf("[Rules]"));
                Rules = metadata.Substring(metadata.IndexOf("[Rules]") + 7);
            }

        }
    }

}
