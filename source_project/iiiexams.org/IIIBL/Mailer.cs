using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace IIIBL
{
    public class Mailer
    {
        private String SMTP;
        private Int32 SMTPPort;
        private String SMTPUser;
        private String SMTPPassword;
        private Boolean EnableSSL;

        public Mailer(String Server, Int32 Port, String User, String Password, Boolean NeedsSSL)
        {
            SMTP = Server;
            SMTPPort = Port;
            SMTPUser = User;
            SMTPPassword = Password;
            EnableSSL = NeedsSSL;
        }

        public void SendMail(String Subject, String From, String To, String CC, String Bcc, String Body, Boolean IsHtml)
        {
            SmtpClient objSmtpClient = new SmtpClient(SMTP, SMTPPort);
            if (SMTPUser != String.Empty)
            {
                objSmtpClient.Credentials = new System.Net.NetworkCredential(SMTPUser, SMTPPassword);
            }

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(From.Trim());
            String[] ad = null;
            if (To.Trim() != String.Empty)
            {
                ad = To.Split(';');
                foreach (String s in ad)
                {
                    mailMessage.To.Add(new MailAddress(s));
                }
            }

            if (CC.Trim() != String.Empty)
            {
                ad = CC.Split(';');
                foreach (String s in ad)
                {
                    mailMessage.CC.Add(new MailAddress(s));
                }
            }

            if (Bcc.Trim() != String.Empty)
            {
                ad = Bcc.Split(';');
                foreach (String s in ad)
                {
                    mailMessage.Bcc.Add(new MailAddress(s));
                }
            }

            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = IsHtml;
            mailMessage.Subject = Subject;
            objSmtpClient.EnableSsl = EnableSSL;
            objSmtpClient.Send(mailMessage);
        }
    }
}
