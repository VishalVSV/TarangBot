using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.GeneralUtils
{
    /*
    Derive Via event Name 

    Meta Data:
    ---------------------
    Small Help Text -> req
    Descriptive Help Text(Rules)-> req
    
    Disc API Stuff - Discuss with Vert when back:
    -------------------------------
    Voice Channels => Set up files (Load From JSON and attach to class)
        => Make this ulong list
    
    Event Attr:
    -------------------------------
    Max participants per school -> req
    isTeamEvent 
    isFlagged 
    EventDates DateTime[]

     */

    public abstract class Event
    {
        //Help text
        public abstract string SmallHelpText { get; }
        public abstract string DescriptiveHelpText { get; } //Rules

        //Disc.NET Attrs
        //Get Back to this when Server Structure is figured out
        //public abstract ulong[] channels { get; }

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
