using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.TarangEvent
{
    public class Registration
    {
        public string School_Name;

        public TeacherCoordinator TeacherCoordinator;

        public Dictionary<string, Participant> participants = new Dictionary<string, Participant>();

        public Registration(string[] rows)
        {
            List<string> row = new List<string>(98);
            row.AddRange(rows);
            if (row.Count != 98)
            {
                row.Add("");
            }

            School_Name = row[1];

            TeacherCoordinator teacherCoordinator = new TeacherCoordinator();
            teacherCoordinator.School_Name = School_Name;
            teacherCoordinator.Name = row[2];
            teacherCoordinator.UserName = row[3];

            teacherCoordinator.email_id = row[4];

            string[] events = new string[] { "One Mic Stand", "Fort Boyard", "Two Faced", "Whose Line is it anyways", "Fandomania", "COD", "Step Up", "Trailer it up", "Synthesize", "Meme-athon", "Pixel", "Craft a Block" };
            int event_ = 0;
            int i = 5;
            while (i < row.Count)
            {
                Event current_event = Tarang.Data.GetEvent(events[event_]);

                int num_participants;

                if (!int.TryParse(rows[i], out num_participants)) num_participants = 0;

                i++;

                int max_participants = current_event.MaxParticipants;

                for (int _ = 0; _ < max_participants; _++)
                {
                    if (_ < num_participants)
                    {
                        if (participants.ContainsKey(row[i + 1]))
                        {
                            participants[row[i + 1]].Registered_Events.Add(current_event.internal_id);
                        }
                        else
                        {
                            Participant participant = new Participant();
                            participant.Name = row[i];
                            participant.UserName = row[i + 1];
                            participant.PhoneNumber = row[i + 2];
                            participant.Registered_Events.Add(current_event.internal_id);

                            participants.Add(participant.UserName, participant);
                        }
                    }

                    i += 3;
                }

                event_++;
            }

            Tarang.Data.Logger.Log($"Registration with {participants.Count} participants from {School_Name} parsed!");
        }
    }
}
