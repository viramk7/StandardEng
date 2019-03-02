using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace StandardEng.Web.Common
{
    public class EmailHelper
    {
        #region Send Mail Method
        public static bool SendMail(string to, string subject, string bodyTemplate, bool isHtml = false, string bcc = "", string ccMail = "", string attachmentFileName = "")
        {
            var email = System.Configuration.ConfigurationManager.AppSettings["Email"];
            var password = System.Configuration.ConfigurationManager.AppSettings["passsword"];
            int PortNumber = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PortNumber"]);
            string HostName = System.Configuration.ConfigurationManager.AppSettings["HostName"];

            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(email);
            mail.Subject = subject;
            mail.Body = bodyTemplate;
            mail.IsBodyHtml = true;

            if (ccMail != "" && ccMail != null)
            {
                mail.CC.Add(ccMail);
            }

            SmtpClient smtp = new SmtpClient();
            smtp.Host = HostName;
            smtp.Port = PortNumber;

            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = false;

            smtp.Credentials = new System.Net.NetworkCredential(email, password);// Enter seders User name and password
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }


        public static void SendAsyncEmail(string to, string subject, string body, bool isHtml = false, string bcc = "",
            string cc = "", string attachmentFileName = "")
        {
            Task task = new Task(() => SendMail(to, subject, body, isHtml, bcc, cc, attachmentFileName));
            task.Start();
        }


        #endregion
    }
}