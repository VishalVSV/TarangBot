using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

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
        public string SmallHelpText;
        public string DescriptiveHelpText;//Rules

        public string internal_id = "";

        public string[] Names;

        //Optionals
        public EventType eventType;
        public bool isTeam  = false;

        //Must Set
        public int MaxParticipants;
        public bool IsTwoDays;

        public ulong Role_Id = 0;
        public ulong WaitingRoomId = 0;

        [JsonIgnore]
        public SocketVoiceChannel WaitingVC
        {
            get
            {
                return Tarang.Data.TarangBot._client.GetGuild(Tarang.Data.GuildId).GetVoiceChannel(WaitingRoomId);
            }
        }

        public Event(string SmallHelpText,string DescHelpText,string[] names,bool IsTwoDays,int MaxParticipants,EventType eventType,bool isTeam)
        {
            this.SmallHelpText = SmallHelpText;
            DescriptiveHelpText = DescHelpText;
            this.IsTwoDays = IsTwoDays;
            this.MaxParticipants = MaxParticipants;
            this.eventType = eventType;
            this.isTeam = isTeam;

            Names = names;
        }
    }

}
