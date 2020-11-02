using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TarangBot.TarangEvent
{
    public class RegistrationRoleGiver
    {
        public Dictionary<string, Participant> UsernamesToAssign = new Dictionary<string, Participant>();

        public RegistrationRoleGiver()
        {
        }

        public void Init()
        {
            Tarang.Data.MessageQueue.On("NewRegistration", (new_registration) =>
            {
                Registration registration = (Registration)new_registration[0];

                for (int i = 0; i < registration.participants.Count; i++)
                {
                    UsernamesToAssign.Add(registration.participants[i].UserName, registration.participants[i]);
                }
            });

            Tarang.Data.MessageQueue.On("OnUserJoin", async (a) =>
            {
                SocketGuildUser arg = (SocketGuildUser)a[0];
                string user = arg.Username + "#" + arg.Discriminator;
                if (UsernamesToAssign.ContainsKey(user))
                {
                    UsernamesToAssign[user].Guild_Id = arg.Guild.Id;

                    await arg.AddRolesAsync(UsernamesToAssign[user].GetRoles());
                }
            });
        }
    }
}
