using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Common.Helpers
{
    public class MailHelper
    {
        List<String>to = new List<String>();
        public static bool SendMail(string subject, string body, string to, bool isHtml = true)
        {
            return SendMail(subject, body,new List<string> { to }, isHtml);
        }
        public static bool SendMail(string subject, string body, List<string> to, bool isHtml = true)
        {
            bool result = false;
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(ConfigHelper.Get<string>("MailUser"));

                to.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x))
                    {
                        message.To.Add(new MailAddress(x));
                    }
                });
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                using (var smtp = new SmtpClient(
                    ConfigHelper.Get<string>("MailHost"),
                    ConfigHelper.Get<int>("MailPort")))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(ConfigHelper.Get<string>("MailUser"),
                        ConfigHelper.Get<string>("MailPass"));
                   
                    smtp.Send(message);
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
