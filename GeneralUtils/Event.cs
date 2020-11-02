using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TarangBot.GeneralUtils
{


    public abstract class Event
    {
        //Help text
        public abstract string SmallHelpText { get; }
        public abstract string DescriptiveHelpText { get; } //Rules

        //Disc.NET Attrs
        //Get Back to this when Server Structure is figured out
        //public abstract ulong[] channels { get; }
        //public abstract var ChannelConfig { get; } //Depending on format of config => Assuming JSON
        //Event Attrs

        //Optionals
        public virtual bool isFlagged { get; } = false;
        public virtual bool isTeam { get; } = false;

        //Must Set
        public abstract int MaxParticipants { get; }
        public abstract DateTime[] EventDates { get; }

    }

    public class Template : Event
    {
        public override string SmallHelpText { get; } = "Test";
        public override string DescriptiveHelpText { get; } = "Big Text";

        public override bool isFlagged { get; } = true;
        public override int MaxParticipants { get; } = 2;

        public override DateTime[] EventDates { get; } = new DateTime[]{
            new DateTime(2020,12,4),
            new DateTime(2020,12,5)
        };

        //public override ulong[] channels => throw new NotImplementedException();
    }
}
