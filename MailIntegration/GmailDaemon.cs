using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TarangBot.MailIntegration
{
    public static class GmailDaemon
    {
        private static SmtpClient client = new SmtpClient("smtp.gmail.com");

        public static bool Ready = false;

        private static string self;

        static GmailDaemon()
        {
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
        }

        public static void SetCredentials(string username, string password)
        {
            self = username;

            client.Credentials = new NetworkCredential(username, password);

            Ready = true;
        }

        public static async void SendMail(string to, string subject, string body, bool isHTML)
        {
            await Task.Run(() => {
                while (!Ready)
                {
                    Thread.SpinWait(20);
                }
            });

            MailMessage mail = new MailMessage(self, to);

            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;

            mail.Body = body;
            mail.BodyEncoding = Encoding.UTF8;

            mail.IsBodyHtml = isHTML;

            client.Send(mail);
        }
    }
}
