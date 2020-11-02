using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.TarangEvent
{
    public class Participant : RoleAssignable
    {
        /// <summary>
        /// Their IRL Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Their Discord Username with discriminator
        /// </summary>
        public string UserName;
        
        public List<Event> Registered_Events = new List<Event>(2);

        /// <summary> 
        /// Their School Name
        /// </summary>
        public string School_Name;

        //TODO: Constructor for participants and implementation for Teacher Coordinator
    }
}
