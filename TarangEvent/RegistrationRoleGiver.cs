using Discord.WebSocket;
using System.Collections.Generic;
using TarangBot.MailIntegration;
using System.IO;

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

                GmailDaemon.SendMail(registration.TeacherCoordinator.email_id, "Tarang 2020 Discord Invite",File.ReadAllText("./InviteMail.html").Replace("$--link--$", Tarang.Data.DiscordInvite),true);

                foreach (Participant participant in registration.participants.Values)
                {
                    UsernamesToAssign.Add(participant.UserName.Split('#')[0].Trim()+"#"+ participant.UserName.Split('#')[1].Trim(), participant);
                    Tarang.Data.participants.Add(participant);
                }
            });

            Tarang.Data.MessageQueue.On("OnUserJoin", async (a) =>
            {
                SocketGuildUser arg = (SocketGuildUser)a[0];
                string user = arg.Username + "#" + arg.Discriminator;
                if (UsernamesToAssign.ContainsKey(user))
                {
                    UsernamesToAssign[user].Guild_Id = arg.Guild.Id;

                    //Welcome messages?
                    for (int i = 0; i < UsernamesToAssign[user].Registered_Events.Count; i++)
                    {
                        Event @event = Tarang.Data.GetEventById(UsernamesToAssign[user].Registered_Events[i]);

                        await (Tarang.Data.TarangBot._client.GetChannel(@event.TextChannel) as SocketTextChannel).SendMessageAsync($"Welcome to {@event.Names[0]}, {UsernamesToAssign[user].Name}");
                    }

                    await arg.AddRolesAsync(UsernamesToAssign[user].GetRoles());
                }
            });
        }
    }
}
