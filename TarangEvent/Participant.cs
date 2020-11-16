using Discord.WebSocket;
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

        /// <summary>
        /// Their email id
        /// </summary>
        public string email_id;

        public List<Event> Registered_Events = new List<Event>(2);

        /// <summary> 
        /// Their School Name
        /// </summary>
        public string School_Name;

        //TODO: Constructor for participants and implementation for Teacher Coordinator

        public ulong[] Role_Ids
        {
            get
            {
                ulong[] Roles = new ulong[Registered_Events.Count];
                for (int i = 0; i < Roles.Length; i++)
                {
                    Roles[i] = Registered_Events[i].Role_Id;
                }
                return Roles;
            }
        }

        public override List<SocketRole> GetRoles()
        {
            List<SocketRole> roles = new List<SocketRole>();
            for (int i = 0; i < Role_Ids.Length; i++)
            {
                roles.Add(Tarang.Data.TarangBot._client.GetGuild(Guild_Id).GetRole(Role_Ids[i]));
            }

            return roles;
        }
    }
}
