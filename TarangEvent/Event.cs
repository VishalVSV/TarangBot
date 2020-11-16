using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public string[] Names;

        //Optionals
        public EventType eventType;
        public bool isTeam  = false;

        //Must Set
        public int MaxParticipants;
        public bool IsTwoDays;

        public ulong Role_Id = 0;

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
