using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.TarangEvent
{
    public class Registration
    {
        public string School_Name;

        public TeacherCoordinator TeacherCoordinator;

        public List<Participant> participants = new List<Participant>();

        public Registration(string[] row)
        {
            if (row.Length != 12)
                return;

            School_Name = row[1];

            TeacherCoordinator teacherCoordinator = new TeacherCoordinator();
            teacherCoordinator.School_Name = School_Name;
            teacherCoordinator.Name = row[2];
            teacherCoordinator.UserName = row[3];

            teacherCoordinator.email_id = row[4];

            string[] events = new string[] { "Block and Tackle", "" };
            int event_ = 0;
            int i = 5;
            while (i < row.Length)
            {
                int num_participants = int.Parse(row[i]);
                i++;

                for (int _ = 0; _ < num_participants; _++)
                {
                    Participant participant = new Participant();
                    participant.Name = row[i];
                    participant.UserName = row[i + 1];
                    participant.email_id = row[i + 2];
                    participant.Role_Id = new ulong[] { 772702101482373150 };

                    participants.Add(participant);

                    i += 3;
                }

                event_++;
            }


        }
    }
}
