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
    Channels => Set up files (Load From JSON and attach to class)
    
    Event Attr:
    -------------------------------
    Max participants per school -> req
    isTeamEvent 
    isFlagged 
    EventDates DateTime[], Make Tuple of MaxSize 2


    Choices:
    CamelCase or Underscore(ew)
    Properties?

     */

    public abstract class Event
    {
        //Help text
        public abstract string SmallHelpText { get; }
        public abstract string DescriptiveHelpText { get;  } //Rules

        public virtual bool isFlagged { get; } = false;
        public virtual bool isTeam { get; } = false;

    }

    public class BnT : Event
    {
        public override string SmallHelpText { get; } = "Test";
        public override string DescriptiveHelpText { get; } = "Big Text";
        public override bool isFlagged { get; } = true;
    }
}
