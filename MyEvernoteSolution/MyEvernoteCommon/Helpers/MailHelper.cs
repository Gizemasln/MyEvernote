using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteCommon.Helpers
{
    public class MailHelper
    {
        public static bool SendMail(string body,string to,string subject,bool isHtml = true)
        {
            return SendMail(body,new List<string> { to},subject,isHtml);
        }

        public static bool SendMail(string body, List<string> to, string subject, bool isHtml = true)
        {
            bool result = false;

            try
            {
                var meessage = new MailMessage();
                meessage.From = new MailAddress(ConfigHelper.Get<string>("MailUser"));

                to.ForEach(x => {
                    meessage.To.Add(new MailAddress(x));
                });
                meessage.Subject = subject;
                meessage.Body = body;
                meessage.IsBodyHtml = isHtml;
                using(var smtp = new SmtpClient(
                 ConfigHelper.Get<string>("MailHost"),
                 ConfigHelper.Get<int>("MailPort")))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(
                        ConfigHelper.Get<string>("MailUser"),
                        ConfigHelper.Get<string>("MailPass"));
                    smtp.Send(meessage);
                    return true;
                }

            }
            catch (Exception) 
            {

          
            }
            return result;
    }
    }
}
