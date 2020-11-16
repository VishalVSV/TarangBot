using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.TarangEvent
{
    public abstract class RoleAssignable
    {
        public ulong User_Id;
        public ulong[] Role_Id;
        public ulong Guild_Id = 771986800083337216;

        public virtual List<SocketRole> GetRoles()
        {
            List<SocketRole> roles = new List<SocketRole>();
            for (int i = 0; i < Role_Id.Length; i++)
            {
                roles.Add(Tarang.Data.TarangBot._client.GetGuild(Guild_Id).GetRole(Role_Id[i]));
            }

            return roles;
        }
    }
}
