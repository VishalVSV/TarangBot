using System;
using System.Collections.Generic;
using System.Text;

namespace TarangBot.TarangEvent
{
    public class RegistrationRoleGiver
    {
        public RegistrationRoleGiver()
        {
            Tarang.Data.MessageQueue.On("NewRegistration", (new_registration) =>
             {

             });
        }
    }
}
