using System;
using System.Collections.Generic;

namespace TarangBot.TarangEvent
{
    public class Registration
    {
        public string School_Name;

        public TeacherCoordinator TeacherCoordinator;

        public Dictionary<string, Participant> participants = new Dictionary<string, Participant>();

        public Registration(string[] rows)
        {
            try
            {
                List<string> row = new List<string>(119);
                row.AddRange(rows);
                while (row.Count != 105)
                {
                    row.Add("");
                }

                School_Name = row[1];

                TeacherCoordinator = new TeacherCoordinator();

                TeacherCoordinator.School_Name = School_Name;
                TeacherCoordinator.Name = row[2];
                TeacherCoordinator.UserName = row[3];

                TeacherCoordinator.email_id = row[4];

                string[] events = new string[] { "One Mic Stand", "Fort Boyard", "Two Faced", "Whose Line is it anyways", "Fandomania", "COD", "Step Up", "Trailer it up", "Synthesize", "Meme-athon", "Pixel", "Craft a Block", "Shark Tank" };
                int event_ = 0;
                int i = 5;
                while (i < row.Count)
                {
                    Event current_event = Tarang.Data.GetEvent(events[event_]);

                    int num_participants;

                    if (!int.TryParse(row[i], out num_participants)) num_participants = 0;

                    i++;

                    int max_participants = current_event.MaxParticipants;

                    for (int _ = 0; _ < max_participants; _++)
                    {
                        if (_ < num_participants && row[i].Trim() != "")
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
            catch(Exception e)
            {
                Tarang.Data.Logger.Log($"Something went wrong parsing the registration: {e.Message}");
            }
        }
    }
}
